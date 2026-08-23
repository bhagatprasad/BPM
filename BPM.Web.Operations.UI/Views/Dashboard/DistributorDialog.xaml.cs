using System;
using System.Net.Mail;
using System.Windows;
using BPM.Web.Operations.UI.Models;
using BPM.Web.Operations.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BPM.Web.Operations.UI.Views.Dashboard
{
    public partial class DistributorDialog : Window
    {
        private readonly IDistributorService _distributorService;
        private readonly DistributorDto _existingDistributor;
        private bool _isEditMode;

        public DistributorDialog() : this(null)
        {
        }

        public DistributorDialog(DistributorDto distributor = null)
        {
            InitializeComponent();
            _distributorService = ((App)Application.Current).ServiceProvider.GetRequiredService<IDistributorService>();
            _existingDistributor = distributor;
            _isEditMode = distributor != null;

            if (_isEditMode)
            {
                TitleText.Text = "Edit Distributor";
                LoadDistributorData();
            }
            else
            {
                TitleText.Text = "Add Distributor";
                GenerateDistributorCode();
            }
        }

        private void LoadDistributorData()
        {
            if (_existingDistributor == null) return;

            DistributorCodeTextBox.Text = _existingDistributor.DistributorCode;
            DistributorCodeTextBox.IsEnabled = false; // Code cannot be changed in edit mode

            DistributorNameTextBox.Text = _existingDistributor.DistributorName;
            RegistrationNumberTextBox.Text = _existingDistributor.RegistrationNumber;
            DrugLicenseNumberTextBox.Text = _existingDistributor.DrugLicenseNumber;
            GSTNumberTextBox.Text = _existingDistributor.GSTNumber;
            ContactPersonTextBox.Text = _existingDistributor.ContactPerson;
            EmailTextBox.Text = _existingDistributor.Email;
            PhoneTextBox.Text = _existingDistributor.Phone;
            AlternatePhoneTextBox.Text = _existingDistributor.AlternatePhone;
            AddressLine1TextBox.Text = _existingDistributor.AddressLine1;
            AddressLine2TextBox.Text = _existingDistributor.AddressLine2;
            CityTextBox.Text = _existingDistributor.City;
            StateTextBox.Text = _existingDistributor.State;
            CountryTextBox.Text = _existingDistributor.Country;
            PostalCodeTextBox.Text = _existingDistributor.PostalCode;
            WebsiteTextBox.Text = _existingDistributor.Website;
            IsActiveCheckBox.IsChecked = _existingDistributor.IsActive;
        }

        private void GenerateDistributorCode()
        {
            var random = new Random();
            var code = $"DIST-{random.Next(1000, 9999)}";
            DistributorCodeTextBox.Text = code;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(DistributorCodeTextBox.Text))
            {
                MessageBox.Show("Distributor Code is required.", "Validation Error",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                DistributorCodeTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(DistributorNameTextBox.Text))
            {
                MessageBox.Show("Distributor Name is required.", "Validation Error",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                DistributorNameTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(ContactPersonTextBox.Text))
            {
                MessageBox.Show("Contact Person is required.", "Validation Error",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                ContactPersonTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                MessageBox.Show("Email is required.", "Validation Error",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                EmailTextBox.Focus();
                return;
            }

            // Validate email format
            try
            {
                var addr = new MailAddress(EmailTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                EmailTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(PhoneTextBox.Text))
            {
                MessageBox.Show("Phone is required.", "Validation Error",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                PhoneTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(CityTextBox.Text))
            {
                MessageBox.Show("City is required.", "Validation Error",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                CityTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(StateTextBox.Text))
            {
                MessageBox.Show("State is required.", "Validation Error",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
                StateTextBox.Focus();
                return;
            }

            try
            {
                if (_isEditMode)
                {
                    // Update existing distributor
                    var updateDto = new UpdateDistributorDto
                    {
                        DistributorName = DistributorNameTextBox.Text,
                        RegistrationNumber = RegistrationNumberTextBox.Text,
                        DrugLicenseNumber = DrugLicenseNumberTextBox.Text,
                        GSTNumber = GSTNumberTextBox.Text,
                        ContactPerson = ContactPersonTextBox.Text,
                        Email = EmailTextBox.Text,
                        Phone = PhoneTextBox.Text,
                        AlternatePhone = AlternatePhoneTextBox.Text,
                        AddressLine1 = AddressLine1TextBox.Text,
                        AddressLine2 = AddressLine2TextBox.Text,
                        City = CityTextBox.Text,
                        State = StateTextBox.Text,
                        Country = CountryTextBox.Text,
                        PostalCode = PostalCodeTextBox.Text,
                        Website = WebsiteTextBox.Text,
                        IsActive = IsActiveCheckBox.IsChecked ?? true
                    };

                    await _distributorService.UpdateDistributorAsync(_existingDistributor.Id, updateDto);
                    MessageBox.Show("Distributor updated successfully!", "Success",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Create new distributor
                    var createDto = new CreateDistributorDto
                    {
                        DistributorCode = DistributorCodeTextBox.Text,
                        DistributorName = DistributorNameTextBox.Text,
                        RegistrationNumber = RegistrationNumberTextBox.Text,
                        DrugLicenseNumber = DrugLicenseNumberTextBox.Text,
                        GSTNumber = GSTNumberTextBox.Text,
                        ContactPerson = ContactPersonTextBox.Text,
                        Email = EmailTextBox.Text,
                        Phone = PhoneTextBox.Text,
                        AlternatePhone = AlternatePhoneTextBox.Text,
                        AddressLine1 = AddressLine1TextBox.Text,
                        AddressLine2 = AddressLine2TextBox.Text,
                        City = CityTextBox.Text,
                        State = StateTextBox.Text,
                        Country = CountryTextBox.Text,
                        PostalCode = PostalCodeTextBox.Text,
                        Website = WebsiteTextBox.Text,
                        IsActive = IsActiveCheckBox.IsChecked ?? true
                    };

                    await _distributorService.InsertDistributorAsync(createDto);
                    MessageBox.Show("Distributor created successfully!", "Success",
                                  MessageBoxButton.OK, MessageBoxImage.Information);
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save distributor: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}