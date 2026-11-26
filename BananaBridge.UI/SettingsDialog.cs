using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace BananaBridge.UI
{
    public partial class SettingsDialog : Form
    {
        public string ApiKey => txtApiKey.Text.Trim();
        public string Provider => cbProvider.SelectedItem?.ToString() ?? "Gemini";

        public SettingsDialog(string currentApiKey)
        {
            InitializeComponent();
            txtApiKey.Text = currentApiKey;
            
            cbProvider.Items.Add("Gemini");
            cbProvider.Items.Add("OpenAI (Stub)");
            
            string savedProvider = SecureStore.LoadProvider();
            int index = cbProvider.FindStringExact(savedProvider);
            cbProvider.SelectedIndex = index >= 0 ? index : 0;
        }

        private async void BtnRefreshModels_Click(object sender, EventArgs e)
        {
            string apiKey = txtApiKey.Text.Trim();
            if (string.IsNullOrEmpty(apiKey))
            {
                MessageBox.Show("Please enter an API Key to refresh models.", "API Key Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string provider = cbProvider.SelectedItem?.ToString() ?? "Gemini";
            btnRefreshModels.Enabled = false;
            lblStatus.Text = $"Refreshing {provider} models...";

            try
            {
                IAiClient client = AiClientFactory.CreateClient(provider);
                List<string> models = await client.ListModelsAsync(apiKey);
                if (models != null && models.Count > 0)
                {
                    string folder = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "BananaBridge"
                    );
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    string cachePath = Path.Combine(folder, "models_cache.json");
                    string json = JsonSerializer.Serialize(models, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(cachePath, json);

                    lblStatus.Text = $"Successfully refreshed {models.Count} models!";
                    MessageBox.Show($"Successfully refreshed and saved {models.Count} models to cache!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblStatus.Text = "No models returned from API.";
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Failed to refresh models.";
                MessageBox.Show($"Failed to refresh models: {ex.Message}", "Refresh Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRefreshModels.Enabled = true;
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            string apiKey = txtApiKey.Text.Trim();
            if (!string.IsNullOrEmpty(apiKey))
            {
                SecureStore.SaveApiKey(apiKey);
            }
            SecureStore.SaveProvider(cbProvider.SelectedItem?.ToString() ?? "Gemini");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

