using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using BananaBridge.UI.Models;

namespace BananaBridge.UI
{
	public partial class BananaDialog : System.Windows.Forms.Form
	{
		private List<StyleItem> _loadedStyles = new List<StyleItem>();
		private List<ChatTurn> _sessionHistory = new List<ChatTurn>();
		private string _selectionPixelHash = string.Empty;
		private Bitmap _generatedResult = null;
		private List<string> _appliedStylesHistory = new List<string>();
		private int _updatingTurnIndex = -1;

		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public Bitmap SourceCrop { get; set; }

		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public Bitmap MaskCrop { get; set; }

		private string _apiKey = string.Empty;

		// Expose properties for the main plugin project
		public string ApiKey => _apiKey;
		public string Model => cbModel.SelectedItem?.ToString() ?? "";
		public string Prompt => txtPrompt.Text.Trim();
		public Bitmap GeneratedResult => _generatedResult;
		public List<ChatTurn> ChatHistory => _sessionHistory;
		public List<string> AppliedStyles => _appliedStylesHistory;
		public bool CopyToClipboard => chkCopyToClipboard.Checked;
		public bool ScaleToFit => chkScaleToFit.Checked;
		public bool GenerateFromScratch => chkGenerateFromScratch.Checked;

		public BananaDialog(
			string initialApiKey,
			string initialModel,
			Bitmap sourceCrop,
			Bitmap maskCrop,
			string selectionPixelHash,
			Bitmap initialResult,
			List<ChatTurn> initialChatHistory,
			List<string> initialAppliedStyles,
			bool initialScaleToFit,
			bool initialGenerateFromScratch)
		{
			InitializeComponent();

			_apiKey = initialApiKey;

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

			SourceCrop = sourceCrop;
			MaskCrop = maskCrop;
			_selectionPixelHash = selectionPixelHash;
			chkScaleToFit.Checked = initialScaleToFit;
			chkGenerateFromScratch.Checked = initialGenerateFromScratch;
			chkGenerateFromScratch.CheckedChanged += ChkGenerateFromScratch_CheckedChanged;

			if (initialResult != null)
			{
				_generatedResult = initialResult;
				pbPreview.Image = new Bitmap(_generatedResult);
			}
			else if (sourceCrop != null && !initialGenerateFromScratch)
			{
				pbPreview.Image = new Bitmap(sourceCrop);
			}

			if (initialChatHistory != null)
			{
				_sessionHistory = new List<ChatTurn>(initialChatHistory);
			}

			if (initialAppliedStyles != null)
			{
				_appliedStylesHistory = new List<string>(initialAppliedStyles);
			}

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
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine($"Failed to load models cache: {ex.Message}");
				}
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
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine($"Failed to load embedded default models: {ex.Message}");
				}
			}

			if (models == null || models.Count == 0)
			{
				// Hardcoded absolute fallback if resource loading fails
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
				LoadStylesFromResource();
				LoadApiKey();

				// Align legacy/non-aligned history
				int turnCount = _sessionHistory.Count / 2;
				if (_appliedStylesHistory.Count < turnCount)
				{
					var newAppliedHistory = new List<string>();
					for (int k = 0; k < turnCount; k++)
					{
						string userPrompt = _sessionHistory[2 * k].Parts != null && _sessionHistory[2 * k].Parts.Count > 0 
							? _sessionHistory[2 * k].Parts[0].Text 
							: string.Empty;
						string rawPrompt = ExtractRawPrompt(userPrompt);
						
						string styleId = "None";
						string styleName = "None";
						if (userPrompt.StartsWith("Style: "))
						{
							int delimiterIdx = userPrompt.IndexOf("\r\nUser prompt: ");
							if (delimiterIdx > 7)
							{
								string imgPrompt = userPrompt.Substring(7, delimiterIdx - 7);
								var matchedStyle = _loadedStyles.Find(s => s.ImagePrompt == imgPrompt);
								if (matchedStyle != null)
								{
									styleId = matchedStyle.Id;
									styleName = matchedStyle.Name;
								}
							}
						}
						newAppliedHistory.Add($"{styleId}:{styleName}:false:{rawPrompt}");
					}
					_appliedStylesHistory = newAppliedHistory;
				}

				RebuildChatHistoryText();

				if (!string.IsNullOrEmpty(_selectionPixelHash))
				{
					LoadRestyleHistoryFromDb();
				}
			}
		}

		private void LoadStylesFromResource()
		{
			try
			{
				using (Stream stream = ConfigService.GetConfigStream("styles.json"))
				{
					if (stream == null)
					{
						cbStyles.Items.Add("No styles found (configuration missing)");
						cbStyles.SelectedIndex = 0;
						return;
					}

					using (StreamReader reader = new StreamReader(stream))
					{
						string json = reader.ReadToEnd();
						_loadedStyles = JsonSerializer.Deserialize<List<StyleItem>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
					}
				}

				cbStyles.Items.Clear();
				cbStyles.Items.Add(new StyleItem { Name = "None (Plain Edit)", Description = "Performs natural language edits without applying preset style guides." });

				Logger.Log($"Successfully loaded {_loadedStyles.Count} styles from embedded resources.");
				foreach (var style in _loadedStyles)
				{
					Logger.Log($"Loaded style: ID='{style.Id}', Name='{style.Name}', HasPrompt={!string.IsNullOrEmpty(style.ImagePrompt)}");
					cbStyles.Items.Add(style);
				}
				cbStyles.SelectedIndex = 0;
			}
			catch (Exception ex)
			{
				Logger.Log($"Error loading styles: {ex}");
				MessageBox.Show($"Failed to load style presets: {ex.Message}", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void LoadApiKey()
		{
			_apiKey = SecureStore.LoadApiKey();
		}

		private void LoadRestyleHistoryFromDb()
		{
			try
			{
				string historyPath = Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
					"BananaBridge",
					"document_history.json"
				);

				if (File.Exists(historyPath))
				{
					string json = File.ReadAllText(historyPath);
					var dict = JsonSerializer.Deserialize<Dictionary<string, List<StyleHistoryRecord>>>(json);
					if (dict != null && dict.TryGetValue(_selectionPixelHash, out var list))
					{
						txtChatHistory.AppendText("=== Selection Restyle History ===\r\n");
						foreach (var rec in list)
						{
							txtChatHistory.AppendText($"[{rec.Timestamp:g}] Style: {rec.StyleName}\r\nPrompt: {rec.Prompt}\r\n\r\n");
						}
						txtChatHistory.AppendText("=================================\r\n\r\n");
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Failed to load history: {ex.Message}");
			}
		}

		private void CbStyles_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbStyles.SelectedItem is StyleItem selectedStyle)
			{
				txtStyleDesc.Text = selectedStyle.Description;
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

		private void BtnSettings_Click(object sender, EventArgs e)
		{
			using (var settingsDlg = new SettingsDialog(_apiKey))
			{
				if (settingsDlg.ShowDialog(this) == DialogResult.OK)
				{
					_apiKey = settingsDlg.ApiKey;
					// Reload models cache in case they refreshed them
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
				if (_updatingTurnIndex >= 0)
				{
					_sessionHistory.RemoveRange(_updatingTurnIndex, _sessionHistory.Count - _updatingTurnIndex);
					int appliedStartIndex = _updatingTurnIndex / 2;
					if (appliedStartIndex < _appliedStylesHistory.Count)
					{
						_appliedStylesHistory.RemoveRange(appliedStartIndex, _appliedStylesHistory.Count - appliedStartIndex);
					}
					_updatingTurnIndex = -1;
					btnGenerate.Text = chkGenerateFromScratch.Checked ? "Generate Image" : "Generate Edit";
					RebuildChatHistoryText();
				}

				string finalPrompt = prompt;
				string selectedStyleId = string.Empty;
				string selectedStyleName = "None";

				if (cbStyles.SelectedItem is StyleItem styleItem && styleItem.Id != null)
				{
					selectedStyleId = styleItem.Id;
					selectedStyleName = styleItem.Name;
					finalPrompt = $"Style: {styleItem.ImagePrompt}\r\nUser prompt: {prompt}";
				}

				Logger.Log($"Style Selected: ID='{selectedStyleId}', Name='{selectedStyleName}'");
				Logger.Log($"Constructed Final Prompt:\n{finalPrompt}");

				txtChatHistory.AppendText($"User: {prompt} " + (selectedStyleName != "None" ? $"[Style: {selectedStyleName}]" : "") + (chkGenerateFromScratch.Checked ? " [Scratch]" : "") + "\r\n");
				txtPrompt.Clear();

				Bitmap sourceCrop = chkGenerateFromScratch.Checked ? null : this.SourceCrop;
				Bitmap maskCrop = chkGenerateFromScratch.Checked ? null : this.MaskCrop;
				string model = cbModel.SelectedItem?.ToString() ?? "";

				List<ChatTurn> activeHistory = _sessionHistory;
				string aspectRatio = this.SourceCrop != null ? GeminiClient.GetClosestAspectRatio(this.SourceCrop.Width, this.SourceCrop.Height) : null;

				string provider = SecureStore.LoadProvider();
				// Run API request on background thread
				Bitmap result = await Task.Run(async () =>
				{
					IAiClient client = AiClientFactory.CreateClient(provider);
					return await client.SendEditRequestAsync(
						apiKey,
						model,
						activeHistory,
						sourceCrop,
						maskCrop,
						finalPrompt,
						aspectRatio
					);
				});

				if (result != null)
				{
					_generatedResult = result;
					pbPreview.Image = new Bitmap(result);

					// Add to session history
					_sessionHistory.Add(new ChatTurn
					{
						Role = "user",
						Parts = new List<ChatPart> { new ChatPart { Text = finalPrompt } }
					});
					_sessionHistory.Add(new ChatTurn
					{
						Role = "model",
						Parts = new List<ChatPart> { new ChatPart { Text = "[Generated Image Result]" } }
					});

					// Track style applied
					string styleId = string.IsNullOrEmpty(selectedStyleId) ? "None" : selectedStyleId;
					string styleName = string.IsNullOrEmpty(selectedStyleName) ? "None" : selectedStyleName;
					_appliedStylesHistory.Add($"{styleId}:{styleName}:{chkGenerateFromScratch.Checked}:{prompt}");

					RebuildChatHistoryText();
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

			if (_generatedResult != null)
			{
				SaveRestyleHistoryToDb();
			}
		}

		private void SaveRestyleHistoryToDb()
		{
			if (_appliedStylesHistory.Count == 0 || string.IsNullOrEmpty(_selectionPixelHash))
			{
				return;
			}

			try
			{
				string folder = Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
					"BananaBridge"
				);
				if (!Directory.Exists(folder))
				{
					Directory.CreateDirectory(folder);
				}

				string historyPath = Path.Combine(folder, "document_history.json");
				Dictionary<string, List<StyleHistoryRecord>> dict = null;

				if (File.Exists(historyPath))
				{
					try
					{
						string json = File.ReadAllText(historyPath);
						dict = JsonSerializer.Deserialize<Dictionary<string, List<StyleHistoryRecord>>>(json);
					}
					catch { }
				}

				if (dict == null)
				{
					dict = new Dictionary<string, List<StyleHistoryRecord>>();
				}

				if (!dict.TryGetValue(_selectionPixelHash, out var list))
				{
					list = new List<StyleHistoryRecord>();
					dict[_selectionPixelHash] = list;
				}

				foreach (var recStr in _appliedStylesHistory)
				{
					int firstColon = recStr.IndexOf(':');
					int secondColon = firstColon >= 0 ? recStr.IndexOf(':', firstColon + 1) : -1;
					int thirdColon = secondColon >= 0 ? recStr.IndexOf(':', secondColon + 1) : -1;
					if (firstColon >= 0 && secondColon >= 0)
					{
						string styleId = recStr.Substring(0, firstColon);
						string styleName = recStr.Substring(firstColon + 1, secondColon - firstColon - 1);
						string prompt = "";
						if (thirdColon >= 0)
						{
							prompt = recStr.Substring(thirdColon + 1);
						}
						else
						{
							prompt = recStr.Substring(secondColon + 1);
						}

						if (styleId == "None" && styleName == "None")
						{
							continue;
						}

						list.Add(new StyleHistoryRecord
						{
							StyleId = styleId,
							StyleName = styleName,
							Prompt = prompt,
							Timestamp = DateTime.Now
						});
					}
				}

				string outputJson = JsonSerializer.Serialize(dict, new JsonSerializerOptions { WriteIndented = true });
				File.WriteAllText(historyPath, outputJson);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Failed to write history: {ex.Message}");
			}
		}

		public static string ExtractRawPrompt(string text)
		{
			if (string.IsNullOrEmpty(text)) return string.Empty;
			string delimiter = "\r\nUser prompt: ";
			int index = text.IndexOf(delimiter);
			if (index >= 0)
			{
				return text.Substring(index + delimiter.Length);
			}
			return text;
		}

		private void RebuildChatHistoryText()
		{
			txtChatHistory.Clear();
			int turnCount = _sessionHistory.Count / 2;
			for (int k = 0; k < turnCount; k++)
			{
				var userTurn = _sessionHistory[2 * k];
				string rawPrompt = "";
				if (userTurn.Parts != null && userTurn.Parts.Count > 0)
				{
					rawPrompt = ExtractRawPrompt(userTurn.Parts[0].Text);
				}

				string styleName = "None";
				bool isScratch = false;
				if (k < _appliedStylesHistory.Count)
				{
					string recStr = _appliedStylesHistory[k];
					int firstColon = recStr.IndexOf(':');
					int secondColon = firstColon >= 0 ? recStr.IndexOf(':', firstColon + 1) : -1;
					int thirdColon = secondColon >= 0 ? recStr.IndexOf(':', secondColon + 1) : -1;
					if (firstColon >= 0 && secondColon >= 0)
					{
						styleName = recStr.Substring(firstColon + 1, secondColon - firstColon - 1);
						if (thirdColon >= 0)
						{
							string scratchStr = recStr.Substring(secondColon + 1, thirdColon - secondColon - 1);
							bool.TryParse(scratchStr, out isScratch);
						}
					}
				}

				txtChatHistory.AppendText($"User: {rawPrompt} " + 
					(styleName != "None" ? $"[Style: {styleName}]" : "") + 
					(isScratch ? " [Scratch]" : "") + "\r\n");
				txtChatHistory.AppendText("Model: [Image Generated successfully]\r\n\r\n");
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
					btnGenerate.Text = chkGenerateFromScratch.Checked ? "Generate Image" : "Generate Edit";
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
					string rawPromptText = ExtractRawPrompt(promptText);
					int index = i;
					string display = rawPromptText.Length > 30 ? rawPromptText.Substring(0, 27) + "..." : rawPromptText;
					var itemUpdate = new ToolStripMenuItem($"Update Edit: \"{display}\"", null, (s, ev) =>
					{
						_updatingTurnIndex = index;
						txtPrompt.Text = rawPromptText;
						btnGenerate.Text = "Update & Regenerate";
						
						// Try to select the style that was used on this turn
						string styleId = "None";
						if (index / 2 < _appliedStylesHistory.Count)
						{
							string recStr = _appliedStylesHistory[index / 2];
							int firstColon = recStr.IndexOf(':');
							if (firstColon >= 0)
							{
								styleId = recStr.Substring(0, firstColon);
							}
						}

						bool found = false;
						for (int sIdx = 0; sIdx < cbStyles.Items.Count; sIdx++)
						{
							if (cbStyles.Items[sIdx] is StyleItem styleItem && styleItem.Id == styleId)
							{
								cbStyles.SelectedIndex = sIdx;
								found = true;
								break;
							}
						}
						if (!found && cbStyles.Items.Count > 0)
						{
							cbStyles.SelectedIndex = 0;
						}
					});
					menu.Items.Add(itemUpdate);
				}
			}

			if (menu.Items.Count <= 2)
			{
				menu.Items.RemoveAt(menu.Items.Count - 1);
			}
		}

		private void ChkGenerateFromScratch_CheckedChanged(object sender, EventArgs e)
		{
			if (chkGenerateFromScratch.Checked)
			{
				pbPreview.Image = null; // Clear background crop preview in scratch mode
				btnGenerate.Text = "Generate Image";
			}
			else
			{
				pbPreview.Image = _generatedResult != null ? new Bitmap(_generatedResult) : (SourceCrop != null ? new Bitmap(SourceCrop) : null);
				btnGenerate.Text = "Generate Edit";
			}
		}
	}
}

