using System;

namespace Tobias.App.Models
{
    public class Item
    {
        #region Public properties

        /// <summary>
        /// Unique identifier of the <c>Donor</c>. 
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// The given first name of the <c>Donor</c>. 
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The given last name of the <c>Donor</c>. 
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The full name of the <c>Donor</c>. 
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// The social security number (personnummer) of the <c>Donor</c>, 
        /// on the form "YYYYMMDD".
        /// </summary>
        public string SocialSecurityNumber { get; set; }

        /// <summary>
        /// The blood group of the <c>Donor</c>, according to the Rh system.
        /// Should be one of: 
        ///   "Rh+" 
        ///   "Rh-"
        ///   "Unassigned" (meaning that the actual blood group is still unkown to the system)
        /// </summary>
        public string BloodGroupRh { get; set; }

        /// <summary>
        /// The blood group of the <c>Donor</c>, according to the AB0 system.
        /// Should be one of: 
        ///   "A" 
        ///   "B"
        ///   "AB"
        ///   "0"
        ///   "Unassigned" (meaning that the actual blood group is still unkown to the system)
        ///   </summary>
        public string BloodGroupAB0 { get; set; }

        /// <summary>
        /// GUI representation of the blood groups of the <c>Donor</c>. 
        /// </summary>
        public string BloodGroups => $"[{BloodGroupRh}]  [{BloodGroupAB0}]";


        #endregion
    }
}