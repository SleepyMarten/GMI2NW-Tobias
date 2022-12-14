using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tobias.Core;

namespace Tobias.RestInterface.Transporters
{
    /// <summary>
    /// Class representing a stem cell recipient in the system.
    /// </summary>
    public class RecipientTransporter
    {
        /// <summary>
        /// Unique identifier of the <c>Recipient</c>. 
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// The given first name of the <c>Recipient</c>. 
        /// </summary>
        public string FirstName
        { get; set; }

        /// <summary>
        /// The given last name of the <c>Recipient</c>. 
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The social security number (personnummer) of the <c>Recipient</c>, 
        /// on the form "YYYYMMDD".
        /// </summary>
        public string SocialSecurityNumber { get; set; }

        /// <summary>
        /// The blood group of the <c>Recipient</c>, according to the Rh system.
        /// Should be one of: 
        ///   "Rh+" 
        ///   "Rh-"
        ///   "Unassigned" (meaning that the actual blood group is still unkown to the system)
        /// </summary>
        public string BloodGroupRh { get; set; }

        /// <summary>
        /// The blood group of the <c>Recipient</c>, according to the AB0 system.
        /// Should be one of: 
        ///   "A" 
        ///   "B"
        ///   "AB"
        ///   "0"
        ///   "Unassigned" (meaning that the actual blood group is still unkown to the system)
        /// </summary>
        public string BloodGroupAB0 { get; set; }

        internal static RecipientTransporter FromRecipient(Recipient recipient)
        {
            var transporter = new RecipientTransporter();
            transporter.Guid = recipient.Guid.ToString();
            transporter.FirstName = recipient.FirstName;
            transporter.LastName = recipient.LastName;
            transporter.SocialSecurityNumber = recipient.SocialSecurityNumber;
            transporter.BloodGroupRh = recipient.BloodGroupRh;
            transporter.BloodGroupAB0 = recipient.BloodGroupAB0;

            return transporter;
        }

        internal static Recipient ToRecipient(RecipientTransporter transporter)
        {
            var recipient = new Recipient();
            try
            {
                recipient.Guid = System.Guid.Parse(transporter.Guid);
            }
            catch
            {
                recipient.Guid = System.Guid.Empty;
            }
            recipient.FirstName = transporter.FirstName;
            recipient.LastName = transporter.LastName;
            recipient.SocialSecurityNumber = transporter.SocialSecurityNumber;
            recipient.BloodGroupRh = transporter.BloodGroupRh;
            recipient.BloodGroupAB0 = transporter.BloodGroupAB0;
            return recipient;
        }
    }
}