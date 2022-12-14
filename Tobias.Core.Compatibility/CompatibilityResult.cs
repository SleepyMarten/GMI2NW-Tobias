namespace Tobias.Core.Compatibility
{
    /// <summary>
    /// Describes whether whether the blood of a specific donor, and the blood of a specific 
    /// recipient, are compatible with each other, based on different parameters. 
    /// </summary>
    public class CompatibilityResult
    {
        /// <summary>
        /// Matching result, based on the blood group according to the Rh system
        /// </summary>
        public bool BloodGroupRh { get; set; } = false;

        /// <summary>
        /// Matching result, based on the blood group according to the AB0 system
        /// </summary>
        public bool BloodGroupAB0 { get; set; } = false;

        /// <summary>
        /// The total, weighted compatibility, where:
        ///   0   = entirely incompatible,
        ///   100 = entirely compatible
        /// </summary>
        public int CompatibilityScore { get; set; } = 0;
    }
}
