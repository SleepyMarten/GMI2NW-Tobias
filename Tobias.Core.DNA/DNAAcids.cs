using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobias.Core.DNA
{
    /// <summary>
    /// Class that manages information about the valid amino acids
    /// </summary>
    public static class DNAAcids
    {
        /// <summary>
        /// Char array containing the valid amino acids
        /// </summary>
        public static readonly char[] Acids = { ((char)AcidEnum.A),
                                                ((char)AcidEnum.C),
                                                ((char)AcidEnum.G),
                                                ((char)AcidEnum.T)};

        /// <summary>
        /// Tells whether all of the characters in <paramref name="acids"/> are allowed amino acids.
        /// </summary>
        /// <param name="acids"></param>
        /// <returns></returns>
        public static bool AreAcids(char[] acids)
        {
            for (int i = 0; i < acids.Length; i++)
            {
                if (!IsAcid(acids[i]))
                {
                    return false;
                }
            }
            return true;
        }

        //TODO: use stupid implementation at first.

        /// <summary>
        /// Tells whether all of the characters in <paramref name="acids"/> are allowed amino acids.
        /// </summary>
        /// <param name="acids"></param>
        /// <returns></returns>
        //public static bool AreAcids(string acids)
        //{
        //    int nextLength = 0;
        //    bool nonAcidFound = false;
        //    while (nextLength < acids.Length)
        //    {
        //        for (int i = 0; i < acids.Length - nextLength; i++)
        //        {
        //            var currentAcids = acids.Substring(i, nextLength).ToCharArray();
        //            for (int j = currentAcids.Length - 1; j > 0; j--)
        //            {
        //                var currentAcid = currentAcids[j];
        //                if (!IsAcid(currentAcid))
        //                {
        //                    nonAcidFound = true;
        //                }
        //            }
        //        }
        //        nextLength++;
        //    }

        //    //TODO: here is a fault. Correct is to return !nonAcidFound.

        //    return !nonAcidFound;
        //}

        //TODO: This is the normal, simple, efficient method.
        /// <summary>
        /// Tells whether all of the characters in <paramref name="acids"/> are allowed amino acids.
        /// </summary>
        /// <param name="acids"></param>
        /// <returns></returns>
        public static bool AreAcids(string acids)
        {
            return AreAcids(acids.ToCharArray());
        }


        /// <summary>
        /// Tells whether <paramref name="acid"/> is one of the allowed amino acids.
        /// </summary>
        /// <param name="acid"></param>
        /// <returns></returns>
        public static bool IsAcid(char acid)
        {
            for (int i = 0; i < DNAAcids.Acids.Length; i++)
            {
                if (String.Equals(acid, DNAAcids.Acids[i]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
