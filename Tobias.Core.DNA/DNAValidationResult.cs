using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobias.Core.DNA
{
    public class DNAValidationResult
    {
        /// <summary>
        /// Tells whether the validated DNA string is valid, according to rules at level 1
        /// </summary>
        public bool Level1IsValid = false;

        /// <summary>
        /// Tells whether the validated DNA string is valid, according to rules at level 2
        /// </summary>
        public bool Level2IsValid = false;
    }
}
