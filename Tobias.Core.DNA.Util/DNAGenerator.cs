using System;
using System.Text;
using Tobias.Core.DNA;

namespace Tobias.Core.DNA.Util
{
    public class DNAGenerator
    {
        private Random m_random = new Random(DateTime.Now.Second * 1000 + DateTime.Now.Millisecond);

        public DNAGenerator()
        {
        }

        public DNAGenerator(Random random)
        {
            m_random = random;
        }

        /// <summary>
        /// Generates a valid DNA string, of length DNAValidator.DefaultLengthMax
        /// </summary>
        /// <returns>The generated DNA string</returns>
        public string Generate()
        {
            StringBuilder builder = new StringBuilder();

            //1. Start randomly
            builder.Append(GetRandomAcid());

            //2. For each new position, 
            while (builder.Length < DNAValidator.DefaultLengthMax)
            {
                int currentLookahead = Math.Max(builder.Length - DNAValidator.ValidationLookAhead, 0);
                string lastLookahead = builder.ToString().Substring(builder.Length - currentLookahead, currentLookahead);
                builder.Append(GetRandomAcidOf(ValidAcidsInNextPosition(lastLookahead)));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Based on the sequence of previous amino acids and the validation rules, 
        /// returns valid amino acids for the next position
        /// </summary>
        /// <param name="previousAcids"></param>
        /// <returns></returns>
        private char[] ValidAcidsInNextPosition(string previousAcids)
        {
            //Not implemented yet
            return DNAAcids.Acids;
        }

        /// <summary>
        /// Returns one of the DNA amino acids randomly
        /// </summary>
        /// <returns>An amino acid</returns>
        private char GetRandomAcid()
        {
            return DNAAcids.Acids[m_random.Next(0, DNAAcids.Acids.Length)];
        }

        /// <summary>
        /// Randomly returns one of the amino acids in <paramref name="acids"/> randomly
        /// </summary>
        /// <param name="acids"></param>
        /// <returns>An amino acid</returns>
        private char GetRandomAcidOf(char[] acids)
        {
            if (DNAAcids.AreAcids(acids))
            {
                //todo: inject a fault here, shouldn't be minus one
                //return acids[m_random.Next(0, acids.Length - 1)];
                return acids[m_random.Next(0, acids.Length)];
            }
            else
            {
                throw new ArgumentException("Argument contains other characters than the amino acids", nameof(acids));
            }
        }
    }
}
