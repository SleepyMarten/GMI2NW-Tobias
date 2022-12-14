using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tobias.Core.DNA.Util;

namespace Tobias.Core.DNA.Test
{
    //Todo: nu skriver jag egna regler. Senare ta bort tester som
    //studenterna ska få hitta på själv
    [TestClass]
    public class DNAValidatorTests
    {
        private const double AllowedDelta = 10 ^-9;

        [TestMethod]
        public void ValidateTest()
        {
            //TODO: remove seed
            var dnaGenerator = new DNAGenerator(new Random(123456));

            var generatedDNAString = dnaGenerator.Generate();

            var dnaString = new DNAString(generatedDNAString);
            var result = dnaString.Validate();

            Assert.IsTrue(result.Level1IsValid);
            Assert.IsTrue(result.Level2IsValid);
        }
    }
}
