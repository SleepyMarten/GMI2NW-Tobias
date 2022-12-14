using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobias.Core.DNA
{
    public class DNAString
    {
        public Guid Guid;
        public string String;
        public DNAValidator Validator;

        public DNAString() : this(Guid.Empty)
        {
        }

        public DNAString(Guid guid) : this(guid, String.Empty)
        {
        }

        public DNAString(string dnaString) : this(Guid.Empty, dnaString)
        {
        }

        public DNAString(string dnaString, DNAValidator validator) : this(Guid.Empty, dnaString, validator)
        {
        }

        public DNAString(Guid guid, DNAValidator validator) : this(guid, String.Empty, validator)
        {
        }

        public DNAString(Guid guid, string dnaString) : this(guid, dnaString, DNAValidator.DefaultValidator) 
        {
        }

        public DNAString(Guid guid, string dnaString, DNAValidator validator)
        {
            Guid = guid;
            String = dnaString;
            Validator = validator;
        }

        public DNAValidationResult Validate()
        {
            return Validator.Validate(String);
        }
    }
}
