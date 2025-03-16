using HardwareScrapper.Business.Abstractions;
using HardwareScrapper.Services.Abstractions;

namespace HardwareScrapper.UI.Forms
{
    public partial class DashboardForm : Form
    {
        private readonly IHardwareService _hardwareService;
        private readonly IScrapingService _scrapingService;

        public DashboardForm(IHardwareService hardwareService, IScrapingService scrapingService)
        {
            InitializeComponent();
            _hardwareService = hardwareService;
            _scrapingService = scrapingService;

            this.Load += DashboardForm_Load;
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            try
            {
                // Get component counts by category
                var categoryData = _hardwareService.GetComponentCountsByCategory();

                // Get component counts by manufacturer
                var manufacturerData = _hardwareService.GetComponentCountsByManufacturer();

                // Update UI with data
                //UpdateCategoryChart(categoryData);
                //UpdateManufacturerChart(manufacturerData);
                //UpdateSummaryStats(categoryData, manufacturerData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //private void UpdateCategoryChart(Dictionary<string, int> categoryData)
        //{
        //    // Clear existing items
        //    listViewCategories.Items.Clear();

        //    // Add each category to the list view
        //    foreach (var category in categoryData)
        //    {
        //        var item = new ListViewItem(category.Key);
        //        item.SubItems.Add(category.Value.ToString());
        //        listViewCategories.Items.Add(item);
        //    }

        //    // Update chart
        //    chartCategories.Series["Categories"].Points.Clear();
        //    foreach (var category in categoryData)
        //    {
        //        chartCategories.Series["Categories"].Points.AddXY(category.Key, category.Value);
        //    }
        //}

    }
}