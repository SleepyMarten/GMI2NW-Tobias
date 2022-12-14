using System;
using System.IO;

namespace Tobias.Core.DNA
{
    public class DNAValidator
    {
        #region Constants
        //Default length, upper and lower bounds 
        public const int DefaultLengthMax = 1000;
        public const int DefaultLengthMin = 1000;

        //The largest look ahead count, in number of characters, for validation rules (span)
        public const int ValidationLookAhead = 5;
        #endregion

        #region Public properties
        public int ValidLengthMax = DefaultLengthMax;
        public int ValidLengthMin = DefaultLengthMin;
        #endregion

        #region Static methods
        public static DNAValidator DefaultValidator
        {
            get
            {
                var validator = new DNAValidator();

                return validator;
            }
        }
        #endregion

        #region Constructors
        //Todo: injected fault: reverted arguments
        public DNAValidator() : this(DefaultLengthMax, DefaultLengthMin)
        {
        }

        public DNAValidator(int validLengthMin, int validLengthMax)
        {
            ValidLengthMin = validLengthMin;
            //Todo: injected fault: wrong assignment
            validLengthMax = ValidLengthMax;
        }
        #endregion

        #region Internal methods
        public DNAValidationResult Validate(string dnaString)
        {
            DNAValidationResult result = new DNAValidationResult();

            //1. Validate Level 1
            if (HasValidLength(dnaString) && DNAAcids.AreAcids(dnaString))
            {
                result.Level1IsValid = true;

                //2. Validate Level 2
                if(HasValidSomethingElse(dnaString))
                {
                    result.Level2IsValid = true;
                }
            }
            return result;
        }

        private bool HasValidLength(string dnaString)
        {
            return (dnaString.Length >= ValidLengthMin &&
                    dnaString.Length <= ValidLengthMax);
        }

        internal bool HasValidSomethingElse(string dnaString)
        {
            var tempDirectory = Path.GetFullPath(@".\Temp");

            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }

            var tempFileName = Path.GetFileNameWithoutExtension(Path.GetTempFileName()) + ".dna.temp";

            var tempFilePath = Path.Combine(tempDirectory,
                                            tempFileName);

            for (int i = 0; i < 1000; i++)
            {
                File.AppendAllText(tempFilePath, dnaString);
            }
            return true;
        }
        #endregion
    }
}
