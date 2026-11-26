using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using BananaBridge.UI.Models;

namespace BananaBridge.UI
{
	public partial class AiEditDialog : Form
	{
		private List<ChatTurn> _sessionHistory = new List<ChatTurn>();
		private Bitmap _generatedResult = null;
		private string _selectionPixelHash = string.Empty;
		private string _apiKey = string.Empty;
		private Bitmap _editableMask = null;
		private int _updatingTurnIndex = -1;

		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public Bitmap SourceCrop { get; set; }

		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public Bitmap MaskCrop => _editableMask;

		public string ApiKey => _apiKey;
		public string Model => cbModel.SelectedItem?.ToString() ?? "";
		public Bitmap GeneratedResult => _generatedResult;
		public List<ChatTurn> ChatHistory => _sessionHistory;
		public bool CopyToClipboard => chkCopyToClipboard.Checked;
		public bool ScaleToFit => chkScaleToFit.Checked;

		public AiEditDialog(
			string initialApiKey,
			string initialModel,
			Bitmap sourceCrop,
			Bitmap maskCrop,
			string selectionPixelHash,
			Bitmap initialResult,
			List<ChatTurn> initialChatHistory,
			bool initialScaleToFit)
		{
			InitializeComponent();

			_apiKey = initialApiKey;
			SourceCrop = sourceCrop;
			
			// Create a copy of maskCrop that we can safely edit
			if (maskCrop != null)
			{
				_editableMask = new Bitmap(maskCrop);
			}
			else if (sourceCrop != null)
			{
				// Default to all selected (transparent mask)
				_editableMask = new Bitmap(sourceCrop.Width, sourceCrop.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				using (Graphics g = Graphics.FromImage(_editableMask))
				{
					g.Clear(Color.Transparent);
				}
			}

			_selectionPixelHash = selectionPixelHash;
			chkScaleToFit.Checked = initialScaleToFit;

			LoadModels();

			if (!string.IsNullOrEmpty(initialModel))
			{
				string normalizedModel = initialModel.StartsWith("models/") ? initialModel.Substring(7) : initialModel;
				int index = cbModel.FindStringExact(normalizedModel);
				if (index != -1)
				{
					cbModel.SelectedIndex = index;
				}
			}

			if (initialResult != null)
			{
				_generatedResult = initialResult;
				// Display result on canvas
				canvas.SourceImage = new Bitmap(_generatedResult);
			}
			else if (sourceCrop != null)
			{
				canvas.SourceImage = sourceCrop;
			}

			canvas.MaskImage = _editableMask;

			if (initialChatHistory != null)
			{
				_sessionHistory = new List<ChatTurn>(initialChatHistory);
				foreach (var turn in _sessionHistory)
				{
					string prefix = turn.Role == "user" ? "User" : "Model";
					string text = turn.Parts != null && turn.Parts.Count > 0 ? turn.Parts[0].Text : "";
					txtChatHistory.AppendText($"{prefix}: {text}\r\n\r\n");
				}
			}

			// Initialize Brush Colors
			cbBrushColor.Items.Add("White");
			cbBrushColor.Items.Add("Red");
			cbBrushColor.Items.Add("Green");
			cbBrushColor.Items.Add("Blue");
			cbBrushColor.Items.Add("Yellow");
			cbBrushColor.Items.Add("Custom...");
			cbBrushColor.SelectedIndex = 0;
			cbBrushColor.SelectedIndexChanged += CbBrushColor_SelectedIndexChanged;

			// CRITICAL: Disable Commit Layer until something is generated
			btnOK.Enabled = (_generatedResult != null);

			// Setup ContextMenu for Chat History to allow editing previous prompts
			ContextMenuStrip ctxMenu = new ContextMenuStrip();
			ctxMenu.Opening += CtxMenu_Opening;
			txtChatHistory.ContextMenuStrip = ctxMenu;
		}

		private void LoadModels()
		{
			cbModel.Items.Clear();
			string folder = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"BananaBridge"
			);
			string cachePath = Path.Combine(folder, "models_cache.json");

			List<string> models = null;
			if (File.Exists(cachePath))
			{
				try
				{
					string json = File.ReadAllText(cachePath);
					models = JsonSerializer.Deserialize<List<string>>(json);
				}
				catch { }
			}

			if (models == null || models.Count == 0)
			{
				try
				{
					using (Stream stream = ConfigService.GetConfigStream("default_models.json"))
					{
						if (stream != null)
						{
							using (StreamReader reader = new StreamReader(stream))
							{
								string json = reader.ReadToEnd();
								models = JsonSerializer.Deserialize<List<string>>(json);
							}
						}
					}
				}
				catch { }
			}

			if (models == null || models.Count == 0)
			{
				models = new List<string> { "gemini-2.5-flash" };
			}

			foreach (var model in models)
			{
				cbModel.Items.Add(model);
			}

			if (cbModel.Items.Count > 0)
			{
				cbModel.SelectedIndex = 0;
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (!this.DesignMode)
			{
				_apiKey = SecureStore.LoadApiKey();
			}
		}

		private void BtnSettings_Click(object sender, EventArgs e)
		{
			using (var settingsDlg = new SettingsDialog(_apiKey))
			{
				if (settingsDlg.ShowDialog(this) == DialogResult.OK)
				{
					_apiKey = settingsDlg.ApiKey;
					string selectedModel = cbModel.SelectedItem?.ToString();
					LoadModels();
					if (!string.IsNullOrEmpty(selectedModel))
					{
						int index = cbModel.FindStringExact(selectedModel);
						if (index != -1)
						{
							cbModel.SelectedIndex = index;
						}
					}
				}
			}
		}

		private void TxtPrompt_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter && !e.Shift)
			{
				e.SuppressKeyPress = true;
				BtnGenerate_Click(this, EventArgs.Empty);
			}
		}

		private async void BtnGenerate_Click(object sender, EventArgs e)
		{
			string apiKey = _apiKey;
			string prompt = txtPrompt.Text.Trim();

			if (string.IsNullOrEmpty(apiKey))
			{
				MessageBox.Show("Please enter an API Key in Settings.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			if (string.IsNullOrEmpty(prompt))
			{
				MessageBox.Show("Please enter a prompt.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			btnGenerate.Enabled = false;
			prgLoading.Visible = true;

			try
			{
				Bitmap sourceCrop = this.SourceCrop;
				Bitmap maskCrop = this.MaskCrop; // Contains user edits from InteractiveCanvas
				string model = cbModel.SelectedItem?.ToString() ?? "";
				string aspectRatio = this.SourceCrop != null ? GeminiClient.GetClosestAspectRatio(this.SourceCrop.Width, this.SourceCrop.Height) : null;

				List<ChatTurn> activeHistory;
				if (_updatingTurnIndex >= 0)
				{
					activeHistory = _sessionHistory.GetRange(0, _updatingTurnIndex);
				}
				else
				{
					activeHistory = _sessionHistory;
				}

				string provider = SecureStore.LoadProvider();
				Bitmap result = await Task.Run(async () =>
				{
					IAiClient client = AiClientFactory.CreateClient(provider);
					return await client.SendEditRequestAsync(
						apiKey,
						model,
						activeHistory,
						sourceCrop,
						maskCrop,
						prompt,
						aspectRatio
					);
				});

				if (result != null)
				{
					_generatedResult = result;
					canvas.SourceImage = new Bitmap(result);

					if (_updatingTurnIndex >= 0)
					{
						_sessionHistory.RemoveRange(_updatingTurnIndex, _sessionHistory.Count - _updatingTurnIndex);
						_updatingTurnIndex = -1;
						btnGenerate.Text = "Generate Edit";
					}

					_sessionHistory.Add(new ChatTurn
					{
						Role = "user",
						Parts = new List<ChatPart> { new ChatPart { Text = prompt } }
					});
					_sessionHistory.Add(new ChatTurn
					{
						Role = "model",
						Parts = new List<ChatPart> { new ChatPart { Text = "[Generated Image Result]" } }
					});

					txtPrompt.Clear();
					RebuildChatHistoryText();
					btnOK.Enabled = true; // Enable Commit Layer now that we have a result
				}
			}
			catch (Exception ex)
			{
				txtChatHistory.AppendText($"Error: {ex.Message}\r\n\r\n");
				MessageBox.Show($"Request failed: {ex.Message}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				btnGenerate.Enabled = true;
				prgLoading.Visible = false;
			}
		}

		private void BtnOK_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(_apiKey))
			{
				SecureStore.SaveApiKey(_apiKey);
			}
		}

		private void RbTool_CheckedChanged(object sender, EventArgs e)
		{
			canvas.IsBrushMode = rbBrush.Checked;
			cbBrushColor.Enabled = rbBrush.Checked;
		}

		private void TbBrushSize_Scroll(object sender, EventArgs e)
		{
			canvas.BrushSize = tbBrushSize.Value;
			lblBrushSize.Text = $"Size: {tbBrushSize.Value} px";
		}

		private void BtnResetMask_Click(object sender, EventArgs e)
		{
			if (_editableMask != null && SourceCrop != null)
			{
				using (Graphics g = Graphics.FromImage(_editableMask))
				{
					g.Clear(Color.Transparent);
				}
				canvas.Invalidate();
			}
		}

		private void BtnZoomFit_Click(object sender, EventArgs e)
		{
			canvas.ZoomFit();
		}

		private void CbBrushColor_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selected = cbBrushColor.SelectedItem?.ToString();
			if (selected == "Custom...")
			{
				using (ColorDialog dlg = new ColorDialog())
				{
					dlg.Color = canvas.BrushColor;
					if (dlg.ShowDialog(this) == DialogResult.OK)
					{
						canvas.BrushColor = dlg.Color;
					}
					else
					{
						// Revert selection to match canvas color
						UpdateBrushColorDropdownSelection();
					}
				}
			}
			else
			{
				switch (selected)
				{
					case "White": canvas.BrushColor = Color.White; break;
					case "Red": canvas.BrushColor = Color.Red; break;
					case "Green": canvas.BrushColor = Color.Green; break;
					case "Blue": canvas.BrushColor = Color.Blue; break;
					case "Yellow": canvas.BrushColor = Color.Yellow; break;
				}
			}
		}

		private void UpdateBrushColorDropdownSelection()
		{
			if (canvas.BrushColor == Color.White) cbBrushColor.SelectedIndex = 0;
			else if (canvas.BrushColor == Color.Red) cbBrushColor.SelectedIndex = 1;
			else if (canvas.BrushColor == Color.Green) cbBrushColor.SelectedIndex = 2;
			else if (canvas.BrushColor == Color.Blue) cbBrushColor.SelectedIndex = 3;
			else if (canvas.BrushColor == Color.Yellow) cbBrushColor.SelectedIndex = 4;
			else cbBrushColor.SelectedIndex = 5; // Custom
		}

		private void RebuildChatHistoryText()
		{
			txtChatHistory.Clear();
			foreach (var turn in _sessionHistory)
			{
				string prefix = turn.Role == "user" ? "User" : "Model";
				string text = turn.Parts != null && turn.Parts.Count > 0 ? turn.Parts[0].Text : "";
				txtChatHistory.AppendText($"{prefix}: {text}\r\n\r\n");
			}
			txtChatHistory.SelectionStart = txtChatHistory.TextLength;
			txtChatHistory.ScrollToCaret();
		}

		private void CtxMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (txtChatHistory.ContextMenuStrip == null) return;
			var menu = txtChatHistory.ContextMenuStrip;
			menu.Items.Clear();

			var itemCopy = new ToolStripMenuItem("Copy", null, (s, ev) => { txtChatHistory.Copy(); });
			itemCopy.Enabled = (txtChatHistory.SelectionLength > 0);
			menu.Items.Add(itemCopy);

			if (_updatingTurnIndex >= 0)
			{
				var itemCancel = new ToolStripMenuItem("Cancel Update Prompt", null, (s, ev) =>
				{
					_updatingTurnIndex = -1;
					btnGenerate.Text = "Generate Edit";
					txtPrompt.Clear();
				});
				menu.Items.Add(itemCancel);
			}

			menu.Items.Add(new ToolStripSeparator());

			for (int i = 0; i < _sessionHistory.Count; i++)
			{
				var turn = _sessionHistory[i];
				if (turn.Role == "user")
				{
					string promptText = turn.Parts != null && turn.Parts.Count > 0 ? turn.Parts[0].Text : string.Empty;
					int index = i;
					string display = promptText.Length > 30 ? promptText.Substring(0, 27) + "..." : promptText;
					var itemUpdate = new ToolStripMenuItem($"Update Edit: \"{display}\"", null, (s, ev) =>
					{
						_updatingTurnIndex = index;
						txtPrompt.Text = promptText;
						btnGenerate.Text = "Update & Regenerate";
					});
					menu.Items.Add(itemUpdate);
				}
			}

			if (menu.Items.Count <= 2)
			{
				menu.Items.RemoveAt(menu.Items.Count - 1);
			}
		}
	}
}

