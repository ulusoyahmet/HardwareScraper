using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HardwareScrapper.Business.Abstractions;
using HardwareScrapper.Domain.Entities;

namespace HardwareScrapper.UI.Forms
{
    public partial class ScrapingSourceForm : Form
    {
        private readonly IScrapingService _scrapingService;
        private List<ScrapingSource> _sources;
        private ScrapingSource _currentSource;

        public ScrapingSourceForm(IScrapingService scrapingService)
        {
            InitializeComponent();
            _scrapingService = scrapingService;
            LoadSources();
        }

        private async void LoadSources()
        {
            try
            {
                _sources = (await _scrapingService.GetAllSourcesAsync()).ToList();
                lstSources.DataSource = null;
                lstSources.DisplayMember = "Name";
                lstSources.ValueMember = "Id";
                lstSources.DataSource = _sources;
                
                EnableDisableControls(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading sources: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnableDisableControls(bool enabled)
        {
            txtName.Enabled = enabled;
            txtBaseUrl.Enabled = enabled;
            txtLogoUrl.Enabled = enabled;
            chkIsEnabled.Enabled = enabled;
            txtConfiguration.Enabled = enabled;
            btnSave.Enabled = enabled;
            btnDelete.Enabled = enabled && _currentSource != null;
            
            btnAdd.Enabled = !enabled;
            btnEdit.Enabled = !enabled && lstSources.SelectedItem != null;
        }

        private void ClearForm()
        {
            txtName.Text = string.Empty;
            txtBaseUrl.Text = string.Empty;
            txtLogoUrl.Text = string.Empty;
            chkIsEnabled.Checked = true;
            txtConfiguration.Text = string.Empty;
            _currentSource = null;
        }

        private void PopulateForm(ScrapingSource source)
        {
            if (source == null) return;
            
            txtName.Text = source.Name;
            txtBaseUrl.Text = source.BaseUrl;
            txtLogoUrl.Text = source.LogoUrl;
            chkIsEnabled.Checked = source.IsEnabled;
            txtConfiguration.Text = source.ScrapeConfiguration;
            _currentSource = source;
        }

        private void lstSources_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSources.SelectedItem is ScrapingSource source)
            {
                PopulateForm(source);
                btnEdit.Enabled = true;
            }
            else
            {
                btnEdit.Enabled = false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            EnableDisableControls(true);
            _currentSource = new ScrapingSource();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lstSources.SelectedItem is ScrapingSource source)
            {
                _currentSource = source;
                PopulateForm(source);
                EnableDisableControls(true);
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtBaseUrl.Text))
            {
                MessageBox.Show("Name and Base URL are required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _currentSource.Name = txtName.Text;
                _currentSource.BaseUrl = txtBaseUrl.Text;
                _currentSource.LogoUrl = txtLogoUrl.Text;
                _currentSource.IsEnabled = chkIsEnabled.Checked;
                _currentSource.ScrapeConfiguration = txtConfiguration.Text;

                if (_currentSource.Id == Guid.Empty)
                {
                    await _scrapingService.AddSourceAsync(_currentSource);
                }
                else
                {
                    await _scrapingService.UpdateSourceAsync(_currentSource);
                }

                MessageBox.Show("Source saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSources();
                EnableDisableControls(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving source: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (_currentSource == null || _currentSource.Id == Guid.Empty) return;

            if (MessageBox.Show("Are you sure you want to delete this source?", "Confirm Delete", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    await _scrapingService.DeleteSourceAsync(_currentSource.Id);
                    MessageBox.Show("Source deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSources();
                    ClearForm();
                    EnableDisableControls(false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting source: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EnableDisableControls(false);
            if (lstSources.SelectedItem is ScrapingSource source)
            {
                PopulateForm(source);
            }
            else
            {
                ClearForm();
            }
        }
    }
} 