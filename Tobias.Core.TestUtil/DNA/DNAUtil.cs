using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobias.Core.TestUtil.ConsoleDriver;

namespace Tobias.Core.TestUtil.DNA
{
    public static class DNAUtil
    {
        public const uint DefaultSeed = 12345;
        public static bool CreateDNAFiles(string directory, uint nOfFiles, uint seed = DefaultSeed)
        {
            //1. Construct argument list
            List<string> arguments = new List<string>();
            arguments.Add("-command generate");
            arguments.Add($"-destination {directory}");
            arguments.Add("-filepattern String_*.dna");
            arguments.Add($"-numberofdnastrings {nOfFiles}");
            arguments.Add($"-seed {seed}");

            //2. Create and start the process
            return ConsoleUtil.ExecuteTobiasConsole(arguments);
        }
    }
}
