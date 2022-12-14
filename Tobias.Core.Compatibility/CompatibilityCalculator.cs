using System;

namespace Tobias.Core.Compatibility
{
    public static class CompatibilityCalculator
    {
        #region Public methods
        public static CompatibilityResult ComputeCompatibilityScore(Donor donor, Recipient recipient)
        {
            CompatibilityResult compatibilityResult = new CompatibilityResult();

            compatibilityResult.BloodGroupRh = GetRhCompatibility(donor, recipient);
            compatibilityResult.BloodGroupAB0 = GetAB0Compatibility(donor, recipient);
            compatibilityResult.CompatibilityScore = (compatibilityResult.BloodGroupRh && compatibilityResult.BloodGroupRh) ? 100 : 0;
            return compatibilityResult;
        }
        #endregion

        #region Private sub-matching methods
        internal static bool GetRhCompatibility(Donor donor, Recipient recipient)
        {
            return (donor.BloodGroupRh == recipient.BloodGroupRh);
        }

        internal static bool GetAB0Compatibility(Donor donor, Recipient recipient)
        {
            return (donor.BloodGroupAB0 == recipient.BloodGroupAB0);
        }
        #endregion
    }
}
