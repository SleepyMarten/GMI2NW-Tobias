using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tobias.Core;

namespace Tobias.RestInterface.Transporters
{
    /// <summary>
    /// Class representing a stem cell donor in the system.
    /// </summary>
    public class DonorTransporter
    {
        /// <summary>
        /// Unique identifier of the <c>Donor</c>. 
        /// </summary>
        public string Guid { get; set; }


        /// <summary>
        /// The given first name of the <c>Donor</c>. 
        /// </summary>
        public string FirstName
        { get; set; }

        /// <summary>
        /// The given last name of the <c>Donor</c>. 
        /// </summary>
        public string LastName { get; set; }

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
        /// </summary>
        public string BloodGroupAB0 { get; set; }

        internal static DonorTransporter FromDonor(Donor donor)
        {
            var transporter = new DonorTransporter();
            transporter.Guid = donor.Guid.ToString();
            transporter.FirstName = donor.FirstName;
            transporter.LastName = donor.LastName;
            transporter.SocialSecurityNumber = donor.SocialSecurityNumber;
            transporter.BloodGroupRh = donor.BloodGroupRh;
            transporter.BloodGroupAB0 = donor.BloodGroupAB0;
            return transporter;
        }

        internal static Donor ToDonor(DonorTransporter transporter)
        {
            var donor = new Donor();
            try
            {
                donor.Guid = System.Guid.Parse(transporter.Guid);
            }
            catch
            {
                donor.Guid = System.Guid.Empty;
            }
            donor.FirstName = transporter.FirstName;
            donor.LastName = transporter.LastName;
            donor.SocialSecurityNumber = transporter.SocialSecurityNumber;
            donor.BloodGroupRh = transporter.BloodGroupRh;
            donor.BloodGroupAB0 = transporter.BloodGroupAB0;
            return donor;
        }
    }
}