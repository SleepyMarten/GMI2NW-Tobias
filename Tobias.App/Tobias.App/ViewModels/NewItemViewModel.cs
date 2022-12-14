using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Tobias.App.Models;
using Xamarin.Forms;

namespace Tobias.App.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private string m_firstName;
        private string m_lastName;
        private string m_socialSecurityNumber;
        private string m_bloodGroupRh;
        private string m_bloodGroupAB0;

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(m_firstName)
                && !String.IsNullOrWhiteSpace(m_lastName)
                && !String.IsNullOrWhiteSpace(m_socialSecurityNumber)
                && !String.IsNullOrWhiteSpace(m_bloodGroupRh)
                && !String.IsNullOrWhiteSpace(m_bloodGroupAB0);
        }

        #region Public properties
        public string FirstName
        {
            get => m_firstName;
            set => SetProperty(ref m_firstName, value);
        }

        public string LastName
        {
            get => m_lastName;
            set => SetProperty(ref m_lastName, value);
        }
        public string SocialSecurityNumber
        {
            get => m_socialSecurityNumber;
            set => SetProperty(ref m_socialSecurityNumber, value);
        }
        public string BloodGroupRh
        {
            get => m_bloodGroupRh;
            set => SetProperty(ref m_bloodGroupRh, value);
        }
        public string BloodGroupAB0
        {
            get => m_bloodGroupAB0;
            set => SetProperty(ref m_bloodGroupAB0, value);
        }
        #endregion

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Item newItem = new Item()
            {
                Guid = Guid.NewGuid().ToString(),
                FirstName = FirstName,
                LastName = LastName,
                SocialSecurityNumber = SocialSecurityNumber,
                BloodGroupRh = BloodGroupRh,
                BloodGroupAB0 = BloodGroupAB0
            };

            await DataStore.AddItemAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
