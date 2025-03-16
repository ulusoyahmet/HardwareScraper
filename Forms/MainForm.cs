using HardwareScrapper.Business.Abstractions;
using HardwareScrapper.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace HardwareScrapper.UI.Forms
{
    public partial class MainForm : Form
    {
        private readonly IHardwareService _hardwareService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ICategoryService _categoryService;
        private readonly IScrapingService _scrapingService;

        private Form _activeForm;

        public MainForm(
            IHardwareService hardwareService,
            IManufacturerService manufacturerService,
            ICategoryService categoryService,
            IScrapingService scrapingService)
        {
            InitializeComponent();
            InitializeComponents();
            _hardwareService = hardwareService;
            _manufacturerService = manufacturerService;
            _categoryService = categoryService;
            _scrapingService = scrapingService;

            // Set up the UI
            this.Text = "Hardware Scraper - PC Components Analyzer";
            this.Size = new System.Drawing.Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Create the menu
            CreateMenu();

            // Open dashboard by default
            OpenDashboard();
        }

        private void CreateMenu()
        {
            // Create menu strip
            MenuStrip menuStrip = new MenuStrip();
            menuStrip.Dock = DockStyle.Top;

            // Dashboard
            ToolStripMenuItem dashboardMenuItem = new ToolStripMenuItem("Dashboard");
            dashboardMenuItem.Click += (sender, e) => OpenDashboard();

            // Hardware
            ToolStripMenuItem hardwareMenuItem = new ToolStripMenuItem("Hardware");

            ToolStripMenuItem allHardwareMenuItem = new ToolStripMenuItem("All Components");
            allHardwareMenuItem.Click += (sender, e) => OpenHardwareList();

            ToolStripMenuItem cpuMenuItem = new ToolStripMenuItem("CPUs");
            cpuMenuItem.Click += (sender, e) => OpenComponentList("CPU");

            ToolStripMenuItem gpuMenuItem = new ToolStripMenuItem("GPUs");
            gpuMenuItem.Click += (sender, e) => OpenComponentList("GPU");

            ToolStripMenuItem motherboardMenuItem = new ToolStripMenuItem("Motherboards");
            motherboardMenuItem.Click += (sender, e) => OpenComponentList("Motherboard");

            ToolStripMenuItem ramMenuItem = new ToolStripMenuItem("RAM");
            ramMenuItem.Click += (sender, e) => OpenComponentList("RAM");

            ToolStripMenuItem storageMenuItem = new ToolStripMenuItem("Storage");
            storageMenuItem.Click += (sender, e) => OpenComponentList("Storage");

            hardwareMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                allHardwareMenuItem, cpuMenuItem, gpuMenuItem, motherboardMenuItem, ramMenuItem, storageMenuItem
            });

            // Settings
            ToolStripMenuItem settingsMenuItem = new ToolStripMenuItem("Settings");

            ToolStripMenuItem manufacturersMenuItem = new ToolStripMenuItem("Manufacturers");
            manufacturersMenuItem.Click += (sender, e) => OpenManufacturerForm();

            ToolStripMenuItem categoriesMenuItem = new ToolStripMenuItem("Categories");
            categoriesMenuItem.Click += (sender, e) => OpenCategoryForm();

            settingsMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                manufacturersMenuItem, categoriesMenuItem
            });

            // Scraping
            ToolStripMenuItem scrapingMenuItem = new ToolStripMenuItem("Scraping");

            ToolStripMenuItem runScrapingMenuItem = new ToolStripMenuItem("Run Scraping");
            runScrapingMenuItem.Click += (sender, e) => OpenScrapingForm();

            ToolStripMenuItem viewLogsMenuItem = new ToolStripMenuItem("View Logs");
            viewLogsMenuItem.Click += (sender, e) => OpenLogViewerForm();

            scrapingMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                runScrapingMenuItem, viewLogsMenuItem
            });

            // Help
            ToolStripMenuItem helpMenuItem = new ToolStripMenuItem("Help");

            ToolStripMenuItem aboutMenuItem = new ToolStripMenuItem("About");
            aboutMenuItem.Click += (sender, e) => ShowAboutDialog();

            helpMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                aboutMenuItem
            });

            // Add all items to menu
            menuStrip.Items.AddRange(new ToolStripItem[] {
                dashboardMenuItem, hardwareMenuItem, settingsMenuItem, scrapingMenuItem, helpMenuItem
            });

            // Add menu to form
            this.Controls.Add(menuStrip);
            this.MainMenuStrip = menuStrip;
        }

        private void OpenChildForm(Form childForm)
        {
            if (_activeForm != null)
            {
                _activeForm.Close();
            }

            _activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            // Create a panel to host the child form if it doesn't exist
            if (!Controls.Contains(panelContent))
            {
                panelContent = new Panel();
                panelContent.Dock = DockStyle.Fill;
                this.Controls.Add(panelContent);
            }

            panelContent.Controls.Add(childForm);
            panelContent.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void OpenDashboard()
        {
            var dashboardForm = Program.ServiceProvider.GetRequiredService<DashboardForm>();
            OpenChildForm(dashboardForm);
        }

        private void OpenHardwareList()
        {
            var hardwareListForm = Program.ServiceProvider.GetRequiredService<HardwareListForm>();
            OpenChildForm(hardwareListForm);
        }

        private void OpenComponentList(string componentType)
        {
            var hardwareListForm = Program.ServiceProvider.GetRequiredService<HardwareListForm>();
            // hardwareListForm.FilterByComponentType(componentType);
            OpenChildForm(hardwareListForm);
        }

        private void OpenManufacturerForm()
        {
            var manufacturerForm = Program.ServiceProvider.GetRequiredService<ManufacturerForm>();
            OpenChildForm(manufacturerForm);
        }

        private void OpenCategoryForm()
        {
            var categoryForm = Program.ServiceProvider.GetRequiredService<CategoryForm>();
            OpenChildForm(categoryForm);
        }

        private void OpenScrapingForm()
        {
            var scrapingForm = Program.ServiceProvider.GetRequiredService<ScrapingForm>();
            OpenChildForm(scrapingForm);
        }

        private void OpenLogViewerForm()
        {
            var logViewerForm = Program.ServiceProvider.GetRequiredService<LogViewerForm>();
            OpenChildForm(logViewerForm);
        }

        private void ShowAboutDialog()
        {
            MessageBox.Show(
                "Hardware Scraper\nVersion 1.0\n\nA tool for scraping and analyzing hardware component data.",
                "About Hardware Scraper",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void InitializeComponents()
        {
            this.panelContent = new System.Windows.Forms.Panel();
            this.SuspendLayout();

            // panelContent
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 24);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1200, 776);
            this.panelContent.TabIndex = 1;

            // MainForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.panelContent);
            this.Name = "MainForm";
            this.Text = "Hardware Scraper - PC Components Analyzer";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel panelContent;
    }
}