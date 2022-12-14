using System;
using System.Collections.Generic;
using Tobias.Core.DNA;

namespace Tobias.UI.Util
{
    /// <summary>
    /// Util class, to represent various Tobias objects to text, suitable to print in a User Interface.
    /// </summary>
    public static class PrettyPrinter
    {
        /// <summary>
        /// Converts a boolean to one of the strings "Pass" or "Fail"
        /// </summary>
        /// <param name="trueOrFalse"></param>
        /// <returns></returns>
        public static string BoolToPassFail(bool trueOrFalse)
        {
            return trueOrFalse ? "Pass" : "Fail";
        }

        /// <summary>
        /// Pretty prints a <c>DNAValidationResult</c>
        /// </summary>
        /// <param name="validationResult"></param>
        /// <returns>A list of strings, suitable to show in a User Interface.</returns>
        public static IEnumerable<string> PrettyPrint(DNAValidationResult validationResult)
        {
            List<string> result = new List<string>();


            //todo: inject fault, show Level1IsValid for both lines
            result.Add(String.Format("Level 1 Validation: {0}", BoolToPassFail(validationResult.Level1IsValid)));
            if (validationResult.Level1IsValid)
            {
                result.Add(String.Format("Level 2 Validation: {0}", BoolToPassFail(validationResult.Level2IsValid)));

                //TODO: move to Matching algorithm instead
                //if (validationResult.Level2IsValid)
                //{
                //    result.Add(String.Format("Level 3 Grade: {0}", validationResult.Level3Grade.ToString()));
                //    result.Add(String.Format("Level 3 Grade : {0}", validationResult.Level4Grade.ToString()));
                //}
            }
            return result;
        }
    }
}
