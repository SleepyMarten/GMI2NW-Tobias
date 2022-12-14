using System;

namespace Tobias.Core
{
    /// <summary>
    /// Class representing a stem cell recipient in the system.
    /// </summary>
    public class Recipient
    {
        #region Private constants
        private const string DefaultFirstName = "Unassigned";
        private const string DefaultLastName = "Unassigned";
        private const string DefaultSocialSecurityNumber = "Unassigned";
        private const string DefaultBloodGroupRh = "Unassigned";
        private const string DefaultBloodGroupAB0 = "Unassigned";
        #endregion

        #region Private member variables
        private Guid m_Guid;
        private string m_firstName;
        private string m_lastName;
        private string m_socialSecurityNumber;
        private string m_bloodGroupRh;
        private string m_bloodGroupAB0;

        private bool m_isDirty = false;
        #endregion

        #region Public properties

        /// <summary>
        /// Unique identifier of the <c>Recipient</c>. 
        /// </summary>
        public Guid Guid
        {
            get
            { return m_Guid; }
            set
            { m_isDirty = true; m_Guid = value; }
        }

        /// <summary>
        /// The given first name of the <c>Recipient</c>. 
        /// </summary>
        public string FirstName
        {
            get
            { return m_firstName; }
            set
            { m_isDirty = true; m_firstName = value; }
        }

        /// <summary>
        /// The given last name of the <c>Recipient</c>. 
        /// </summary>
        public string LastName
        {
            get
            { return m_lastName; }
            set
            { m_isDirty = true; m_lastName = value; }
        }

        /// <summary>
        /// The social security number (personnummer) of the <c>Recipient</c>, 
        /// on the form "YYYYMMDD".
        /// </summary>
        public string SocialSecurityNumber
        {
            get
            { return m_socialSecurityNumber; }
            set
            { m_isDirty = true; m_socialSecurityNumber = value; }
        }

        /// <summary>
        /// The blood group of the <c>Recipient</c>, according to the Rh system.
        /// Should be one of: 
        ///   "Rh+" 
        ///   "Rh-"
        ///   "Unassigned" (meaning that the actual blood group is still unkown to the system)
        /// </summary>
        public string BloodGroupRh
        {
            get
            { return m_bloodGroupRh; }
            set
            { m_isDirty = true; m_bloodGroupRh = value; }
        }

        /// <summary>
        /// The blood group of the <c>Recipient</c>, according to the AB0 system.
        /// Should be one of: 
        ///   "A" 
        ///   "B"
        ///   "AB"
        ///   "0"
        ///   "Unassigned" (meaning that the actual blood group is still unkown to the system)
        /// </summary>
        public string BloodGroupAB0
        {
            get
            { return m_bloodGroupAB0; }
            set
            { m_isDirty = true; m_bloodGroupAB0 = value; }
        }

        /// <summary>
        /// Flag that tells if the <c>Recipient</c> has unsaved changed.
        /// </summary>
        public bool IsDirty
        {
            get
            { return m_isDirty; }
            set
            { m_isDirty = value; }
        }
        #endregion

        #region Ctor(s)

        /// <summary>
        /// Creates a <c>Recipient</c> with default values and an empty Guid.
        /// </summary>
        public Recipient() : this(Guid.Empty)
        { }

        /// <summary>
        /// Creates a <c>Recipient</c> with default values.
        /// </summary>
        /// <param name="guid"></param>
        public Recipient(Guid guid) : this(guid, DefaultFirstName, DefaultLastName,
                                           DefaultSocialSecurityNumber,
                                           DefaultBloodGroupRh, DefaultBloodGroupAB0)
        { }

        /// <summary>
        /// Creates a <c>Recipient</c> with specified values.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="socialSecurityNumber"></param>
        /// <param name="bloodGroupRh"></param>
        /// <param name="bloodGroupAB0"></param>
        public Recipient(Guid guid, string firstName, string lastName,
                         string socialSecurityNumber,
                         string bloodGroupRh, string bloodGroupAB0)
        {
            m_isDirty = false;
            m_Guid = guid;
            m_firstName = firstName;
            m_lastName = lastName;
            m_socialSecurityNumber = socialSecurityNumber;
            m_bloodGroupRh = bloodGroupRh;
            m_bloodGroupAB0 = bloodGroupAB0;
        }

        /// <summary>
        /// Creates a <c>Recipient</c> where members are initialized with the values of <paramref name="recipient"/>. 
        /// </summary>
        /// <param name="recipient"></param>
        public Recipient(Recipient recipient) : this(recipient.Guid, recipient.FirstName, recipient.LastName,
                                                     recipient.SocialSecurityNumber,
                                                     recipient.BloodGroupRh, recipient.BloodGroupAB0)
        { }

        /// <summary>
        /// Creates a <c>Recipient</c> where a new unique Guid is generated, members are initialized with the parameter values. 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="socialSecurityNumber"></param>
        /// <param name="bloodGroupRh"></param>
        /// <param name="bloodGroupAB0"></param>
        public Recipient(string firstName, string lastName,
                         string socialSecurityNumber,
                         string bloodGroupRh, string bloodGroupAB0)
                        : this(Guid.NewGuid(), firstName, lastName,
                               socialSecurityNumber,
                               bloodGroupRh, bloodGroupAB0)
        { }
        #endregion

        #region ICloneable interface methods
        public object Clone()
        {
            return new Recipient(this);
        }

        #endregion
    }
}
