using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Tobias.App.Models;
using Xamarin.Forms;

namespace Tobias.App.ViewModels
{
    [QueryProperty(nameof(Guid), nameof(Guid))]
    public class ItemDetailViewModel : BaseViewModel
    {
        //private string itemId;


        //private string text;
        //private string description;

        //public string Id { get; set; }

        //TODO: remove
        /*
        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
        
        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }
        */

        #region Private member variables
        private string m_Guid;
        private string m_firstName;
        private string m_lastName;
        private string m_socialSecurityNumber;
        private string m_bloodGroupRh;
        private string m_bloodGroupAB0;

        #endregion

        #region Public properties

        /// <summary>
        /// Unique identifier of the <c>Donor</c>. 
        /// </summary>
        public string Guid
        {
            get => m_Guid; 
            set
            { m_Guid = value; LoadItemId(Guid); }
        }

        /// <summary>
        /// The given first name of the <c>Donor</c>. 
        /// </summary>
        public string FirstName
        {
            get => m_firstName;
            set => SetProperty(ref m_firstName, value);
        }

        /// <summary>
        /// The given last name of the <c>Donor</c>. 
        /// </summary>
        public string LastName
        {
            get => m_lastName; 
            set => SetProperty(ref m_lastName, value);
        }

        /// <summary>
        /// The social security number (personnummer) of the <c>Donor</c>, 
        /// on the form "YYYYMMDD".
        /// </summary>
        public string SocialSecurityNumber
        {
            get => m_socialSecurityNumber; 
            set => SetProperty(ref m_socialSecurityNumber, value); 
        }

        /// <summary>
        /// The blood group of the <c>Donor</c>, according to the Rh system.
        /// Should be one of: 
        ///   "Rh+" 
        ///   "Rh-"
        ///   "Unassigned" (meaning that the actual blood group is still unkown to the system)
        /// </summary>
        public string BloodGroupRh
        {
            get => m_bloodGroupRh;
            set => SetProperty(ref m_bloodGroupRh, value);
        }

        /// <summary>
        /// The blood group of the <c>Donor</c>, according to the AB0 system.
        /// Should be one of: 
        ///   "A" 
        ///   "B"
        ///   "AB"
        ///   "0"
        ///   "Unassigned" (meaning that the actual blood group is still unkown to the system)
        ///   </summary>
        public string BloodGroupAB0
        {
            get => m_bloodGroupAB0;
            set => SetProperty(ref m_bloodGroupAB0, value);
        }
        #endregion


        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                //Id = item.Id;
                //Text = item.Text;
                //Description = item.Description;

                FirstName = item.FirstName;
                LastName = item.LastName;
                SocialSecurityNumber = item.SocialSecurityNumber;
                BloodGroupRh = item.BloodGroupRh;
                BloodGroupAB0 = item.BloodGroupAB0;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
