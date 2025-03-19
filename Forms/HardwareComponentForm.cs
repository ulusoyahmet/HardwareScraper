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
    public partial class HardwareComponentForm : Form
    {
        private readonly IHardwareService _hardwareService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ICategoryService _categoryService;
        private HardwareComponent _currentComponent;
        private bool _isNew = true;

        public HardwareComponentForm(
            IHardwareService hardwareService,
            IManufacturerService manufacturerService,
            ICategoryService categoryService)
        {
            InitializeComponent();
            _hardwareService = hardwareService;
            _manufacturerService = manufacturerService;
            _categoryService = categoryService;

            // Set up event handlers
            cboComponentType.SelectedIndexChanged += CboComponentType_SelectedIndexChanged;

            LoadFormData();
        }

        private async void LoadFormData()
        {
            try
            {
                // Load manufacturers
                var manufacturers = await _manufacturerService.GetAllManufacturersAsync();
                cboManufacturer.DataSource = manufacturers.ToList();
                cboManufacturer.DisplayMember = "Name";
                cboManufacturer.ValueMember = "Id";

                // Load categories
                var categories = await _categoryService.GetAllCategoriesAsync();
                cboCategory.DataSource = categories.ToList();
                cboCategory.DisplayMember = "Name";
                cboCategory.ValueMember = "Id";

                // Set component types
                cboComponentType.Items.Clear();
                cboComponentType.Items.Add("CPU");
                cboComponentType.Items.Add("GPU");
                cboComponentType.Items.Add("Motherboard");
                cboComponentType.Items.Add("RAM");
                cboComponentType.Items.Add("Storage");
                cboComponentType.SelectedIndex = 0;

                // Default values
                dtpReleaseDate.Value = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading form data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CboComponentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // When component type changes, show or hide the specific component properties panel
            string componentType = cboComponentType.SelectedItem?.ToString();

            // Reset all panels visibility
            pnlCPU.Visible = false;
            pnlGPU.Visible = false;
            pnlMotherboard.Visible = false;
            pnlRAM.Visible = false;
            pnlStorage.Visible = false;

            // Show the appropriate panel
            switch (componentType)
            {
                case "CPU":
                    pnlCPU.Visible = true;
                    break;
                case "GPU":
                    pnlGPU.Visible = true;
                    break;
                case "Motherboard":
                    pnlMotherboard.Visible = true;
                    break;
                case "RAM":
                    pnlRAM.Visible = true;
                    break;
                case "Storage":
                    pnlStorage.Visible = true;
                    break;
            }
        }

        public void SetComponent(HardwareComponent component)
        {
            _currentComponent = component;
            _isNew = component == null || component.Id == Guid.Empty;

            if (!_isNew)
            {
                // Populate the form with existing component data
                txtModelName.Text = component.ModelName;
                txtDescription.Text = component.Description;
                txtImageUrl.Text = component.ImageUrl;
                numCurrentPrice.Value = component.CurrentPrice > 0 ? (decimal)component.CurrentPrice : 0;
                dtpReleaseDate.Value = component.ReleaseDate.HasValue ? component.ReleaseDate.Value : DateTime.Now;
                chkIsActive.Checked = component.IsActive;

                // Set manufacturer and category
                if (component.ManufacturerId.HasValue && cboManufacturer.Items.Count > 0)
                {
                    for (int i = 0; i < cboManufacturer.Items.Count; i++)
                    {
                        var manufacturer = (Manufacturer)cboManufacturer.Items[i];
                        if (manufacturer.Id == component.ManufacturerId.Value)
                        {
                            cboManufacturer.SelectedIndex = i;
                            break;
                        }
                    }
                }

                if (component.CategoryId.HasValue && cboCategory.Items.Count > 0)
                {
                    for (int i = 0; i < cboCategory.Items.Count; i++)
                    {
                        var category = (Category)cboCategory.Items[i];
                        if (category.Id == component.CategoryId.Value)
                        {
                            cboCategory.SelectedIndex = i;
                            break;
                        }
                    }
                }

                // Set component type
                string componentType = DetermineComponentType(component);
                for (int i = 0; i < cboComponentType.Items.Count; i++)
                {
                    if (cboComponentType.Items[i].ToString() == componentType)
                    {
                        cboComponentType.SelectedIndex = i;
                        break;
                    }
                }

                // Set specific component properties
                if (component is CPU cpu)
                {
                    numCPUCores.Value = cpu.CoreCount > 0 ? cpu.CoreCount : 0;
                    numCPUThreads.Value = cpu.ThreadCount > 0 ? cpu.ThreadCount : 0;
                    numCPUBaseFreq.Value = cpu.BaseClock > 0 ? (decimal)cpu.BaseClock : 0;
                    numCPUBoostFreq.Value = cpu.BoostClock > 0 ? (decimal)cpu.BoostClock : 0;
                    txtCPUSocket.Text = cpu.Socket;
                    numCPUTDP.Value = cpu.TDP > 0 ? cpu.TDP : 0;
                }
                else if (component is GPU gpu)
                {
                    numGPUMemory.Value = gpu.MemorySize > 0 ? gpu.MemorySize : 0;
                    txtGPUMemoryType.Text = gpu.MemoryType;
                    numGPUCoreClock.Value = gpu.CoreClock > 0 ? (decimal)gpu.CoreClock : 0;
                    numGPUBoostClock.Value = gpu.BoostClock > 0 ? (decimal)gpu.BoostClock : 0;
                    txtGPUInterface.Text = gpu.Interface;
                    numGPUTDP.Value = gpu.TDP > 0 ? gpu.TDP : 0;
                }
                else if (component is Motherboard motherboard)
                {
                    txtMBFormFactor.Text = motherboard.FormFactor;
                    txtMBSocket.Text = motherboard.Socket;
                    txtMBChipset.Text = motherboard.Chipset;
                    numMBMemorySlots.Value = motherboard.MemorySlots > 0 ? motherboard.MemorySlots : 0;
                    txtMBMemoryType.Text = motherboard.MemoryType;
                    numMBMaxMemory.Value = motherboard.MaxMemory > 0 ? motherboard.MaxMemory : 0;
                }
                else if (component is RAM ram)
                {
                    numRAMCapacity.Value = ram.Capacity > 0 ? ram.Capacity : 0;
                    numRAMSpeed.Value = ram.Speed > 0 ? ram.Speed : 0;
                    txtRAMType.Text = ram.Type;
                    numRAMModules.Value = ram.ModuleCount > 0 ? ram.ModuleCount : 0;
                    txtRAMCasLatency.Text = ram.CasLatency;
                }
                else if (component is Storage storage)
                {
                    numStorageCapacity.Value = storage.Capacity > 0 ? storage.Capacity : 0;
                    txtStorageType.Text = storage.Type;
                    txtStorageInterface.Text = storage.Interface;
                    numStorageReadSpeed.Value = storage.ReadSpeed > 0 ? storage.ReadSpeed : 0;
                    numStorageWriteSpeed.Value = storage.WriteSpeed > 0 ? storage.WriteSpeed : 0;
                }
            }
            else
            {
                // Clear form for new component
                txtModelName.Text = string.Empty;
                txtDescription.Text = string.Empty;
                txtImageUrl.Text = string.Empty;
                numCurrentPrice.Value = 0;
                dtpReleaseDate.Value = DateTime.Now;
                chkIsActive.Checked = true;

                // Clear specific component fields
                ClearSpecificComponentFields();
            }
        }

        private string DetermineComponentType(HardwareComponent component)
        {
            if (component is CPU) return "CPU";
            if (component is GPU) return "GPU";
            if (component is Motherboard) return "Motherboard";
            if (component is RAM) return "RAM";
            if (component is Storage) return "Storage";
            return "CPU"; // Default
        }

        private void ClearSpecificComponentFields()
        {
            // CPU
            numCPUCores.Value = 0;
            numCPUThreads.Value = 0;
            numCPUBaseFreq.Value = 0;
            numCPUBoostFreq.Value = 0;
            txtCPUSocket.Text = string.Empty;
            numCPUTDP.Value = 0;

            // GPU
            numGPUMemory.Value = 0;
            txtGPUMemoryType.Text = string.Empty;
            numGPUCoreClock.Value = 0;
            numGPUBoostClock.Value = 0;
            txtGPUInterface.Text = string.Empty;
            numGPUTDP.Value = 0;

            // Motherboard
            txtMBFormFactor.Text = string.Empty;
            txtMBSocket.Text = string.Empty;
            txtMBChipset.Text = string.Empty;
            numMBMemorySlots.Value = 0;
            txtMBMemoryType.Text = string.Empty;
            numMBMaxMemory.Value = 0;

            // RAM
            numRAMCapacity.Value = 0;
            numRAMSpeed.Value = 0;
            txtRAMType.Text = string.Empty;
            numRAMModules.Value = 0;
            txtRAMCasLatency.Text = string.Empty;

            // Storage
            numStorageCapacity.Value = 0;
            txtStorageType.Text = string.Empty;
            txtStorageInterface.Text = string.Empty;
            numStorageReadSpeed.Value = 0;
            numStorageWriteSpeed.Value = 0;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtModelName.Text))
            {
                MessageBox.Show("Model name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string componentType = cboComponentType.SelectedItem.ToString();
            HardwareComponent component = null;

            try
            {
                // Create component based on type
                switch (componentType)
                {
                    case "CPU":
                        component = _isNew ? new CPU() : _currentComponent as CPU ?? new CPU();
                        break;
                    case "GPU":
                        component = _isNew ? new GPU() : _currentComponent as GPU ?? new GPU();
                        break;
                    case "Motherboard":
                        component = _isNew ? new Motherboard() : _currentComponent as Motherboard ?? new Motherboard();
                        break;
                    case "RAM":
                        component = _isNew ? new RAM() : _currentComponent as RAM ?? new RAM();
                        break;
                    case "Storage":
                        component = _isNew ? new Storage() : _currentComponent as Storage ?? new Storage();
                        break;
                }

                // Set common properties
                component.ModelName = txtModelName.Text;
                component.Description = txtDescription.Text;
                component.ImageUrl = txtImageUrl.Text;
                component.CurrentPrice = (double)numCurrentPrice.Value;
                component.ReleaseDate = dtpReleaseDate.Value;
                component.IsActive = chkIsActive.Checked;

                if (cboManufacturer.SelectedItem != null)
                {
                    var manufacturer = (Manufacturer)cboManufacturer.SelectedItem;
                    component.ManufacturerId = manufacturer.Id;
                }

                if (cboCategory.SelectedItem != null)
                {
                    var category = (Category)cboCategory.SelectedItem;
                    component.CategoryId = category.Id;
                }

                // Set specific component properties
                if (component is CPU cpu)
                {
                    cpu.CoreCount = (int)numCPUCores.Value;
                    cpu.ThreadCount = (int)numCPUThreads.Value;
                    cpu.BaseClock = (double)numCPUBaseFreq.Value;
                    cpu.BoostClock = (double)numCPUBoostFreq.Value;
                    cpu.Socket = txtCPUSocket.Text;
                    cpu.TDP = (int)numCPUTDP.Value;
                }
                else if (component is GPU gpu)
                {
                    gpu.MemorySize = (int)numGPUMemory.Value;
                    gpu.MemoryType = txtGPUMemoryType.Text;
                    gpu.CoreClock = (double)numGPUCoreClock.Value;
                    gpu.BoostClock = (double)numGPUBoostClock.Value;
                    gpu.Interface = txtGPUInterface.Text;
                    gpu.TDP = (int)numGPUTDP.Value;
                }
                else if (component is Motherboard motherboard)
                {
                    motherboard.FormFactor = txtMBFormFactor.Text;
                    motherboard.Socket = txtMBSocket.Text;
                    motherboard.Chipset = txtMBChipset.Text;
                    motherboard.MemorySlots = (int)numMBMemorySlots.Value;
                    motherboard.MemoryType = txtMBMemoryType.Text;
                    motherboard.MaxMemory = (int)numMBMaxMemory.Value;
                }
                else if (component is RAM ram)
                {
                    ram.Capacity = (int)numRAMCapacity.Value;
                    ram.Speed = (int)numRAMSpeed.Value;
                    ram.Type = txtRAMType.Text;
                    ram.ModuleCount = (int)numRAMModules.Value;
                    ram.CasLatency = txtRAMCasLatency.Text;
                }
                else if (component is Storage storage)
                {
                    storage.Capacity = (int)numStorageCapacity.Value;
                    storage.Type = txtStorageType.Text;
                    storage.Interface = txtStorageInterface.Text;
                    storage.ReadSpeed = (int)numStorageReadSpeed.Value;
                    storage.WriteSpeed = (int)numStorageWriteSpeed.Value;
                }

                // Save the component
                if (_isNew)
                {
                    await _hardwareService.AddHardwareComponentAsync(component);
                    MessageBox.Show("Component added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    await _hardwareService.UpdateHardwareComponentAsync(component);
                    MessageBox.Show("Component updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving component: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
} 