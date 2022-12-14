using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NDesk.Options;
using Tobias.DatabaseMgmt;
using Tobias.General.Util;
using Tobias.UI.Util;
using Tobias.Core.DNA;
using Tobias.Core.DNA.Util;
using System.Diagnostics;


namespace Tobias.ConsoleApp
{
    public class Parameters
    {
        #region Constants
        public const string DefaultDNAStringOutputFilePattern = "DNA_*.data";
        public const int DefaultDNAStringOutputFileCount = 1;
        #endregion

        #region Parameters as class properties
        public bool Help { get; private set; }
        public bool Version { get; private set; }

        public List<string> Origins { get; private set; } = new List<string>();

        public string Destination { get; private set; }
        public List<string> FilesOrFileTypes { get; private set; } = new List<string>();

        #region Random seed, when generating DNA string(s)
        private string m_seedString = "";

        public int Seed
        {
            get
            {
                int parsedNumber = DateTime.Now.Millisecond / 1000;
                Int32.TryParse(m_seedString, out parsedNumber);
                return parsedNumber;
            }
            set
            {
                m_seedString = value.ToString();
            }
        }
        #endregion

        #region Number of output files, when generating DNA string(s)
        private string m_numberOfOutputFiles = DefaultDNAStringOutputFileCount.ToString();
        public int NumberOfOutputFiles
        {
            get
            {
                int parsedNumber = 0;
                int.TryParse(m_numberOfOutputFiles, out parsedNumber);
                return parsedNumber;
            }
            set
            {
                m_numberOfOutputFiles = value.ToString();
            }
        }
        #endregion

        #region Command, represented as both string and enum
        public enum CommandEnum
        {
            Unspecified,
            Build,
            Teardown,
            GenerateDNA,
            ValidateDNA
        }

        private CommandEnum Command { get; set; } = CommandEnum.Unspecified;

        private string m_commandString = null;
        public string CommandString
        {
            get { return m_commandString; }

            set
            {
                if (value is null)
                {
                    m_commandString = null;
                    Command = CommandEnum.Unspecified;
                }
                else
                {
                    m_commandString = value.ToLower();
                    if (m_commandString.Equals("b") || m_commandString.Equals("build"))
                    {
                        Command = CommandEnum.Build;
                    }
                    else if (m_commandString.Equals("t") || m_commandString.Equals("teardown"))
                    {
                        Command = CommandEnum.Teardown;
                    }
                    else if (m_commandString.Equals("g") || m_commandString.Equals("generate"))
                    {
                        Command = CommandEnum.GenerateDNA;
                    }
                    else if (m_commandString.Equals("v") || m_commandString.Equals("validate"))
                    {
                        Command = CommandEnum.ValidateDNA;
                    }
                    else
                    {
                        Command = CommandEnum.Unspecified;
                    }
                }
            }
        }
        #endregion

        #endregion

        #region Static functions
        public static bool TryParse(IList<string> args, out Parameters parameters)
        {
            //Write welcome message
            var programAssembly = typeof(Parameters).Assembly;
            var programFullPath = programAssembly.Location.ToLowerInvariant();
            var programName = Path.GetFileNameWithoutExtension(programFullPath).ToLowerInvariant();
            var programVersion = programAssembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false).Cast<AssemblyFileVersionAttribute>().Select(a => a.Version).FirstOrDefault();
            Console.WriteLine("- - - Welcome to Tobias Console Utility - - -");
            Console.WriteLine(programFullPath);
            Console.WriteLine();

            parameters = null;

            var p = new Parameters
            {
                // Insert default values here
                Destination = null,
                Command = CommandEnum.Unspecified,
                CommandString = null,
            };

            var optionSet = new OptionSet
                {
                    {"H|h|?|help", "Show this help.", v => p.Help = (v != null)},
                    {"V|v|version", "Show the version.", v => p.Version = (v != null)},
                    //{"sp|stringparameter=", "The {VALUE} for a string parameter", v => p.StringParameter = v},
                    //{"ip|intparameter=", "The {VALUE} for an int parameter", v => p.IntParameter = int.Parse(v)},
                    //{"ep|enumparameter=", "The {VALUE} for an enum parameter.", v =>
                    //{"ovsp|optionalvaluestringparameter:", "A parameter that can optionally take a {VALUE}, which will be null otherwise", v => { p.OptionalValueSpecified = true; p.OptionalValue = v } },
                    //    {
                    //            EnumType e;
                    //            Enum.TryParse(v, true, out e);
                    //            p.EnumValue |= e; // You can specify multiple flag values in separate parameters
                    //    }},
                    {"C|c|command=", "Execute a command: B[uild] (database), T[eardown] (database), G[enerate] (valid DNA string), V[alidate] (DNA string), E[xecuteserver].", v => p.CommandString = v },
                    {"O|o|origin=", "If -command is Build or Teardown: Folder to copy files from. Copy occurs before executing the -command. If not specified, no files will be copied. If -command is Validate: location of source DNA string files.", v => p.Origins.Add(v) },
                    {"D|d|destination=", "If -command is Build or Teardown: Folder to copy files to. If command is Generate: Folder to write generated files to. If not specified, files are copied to the current working directory.", v => p.Destination = v },
                    {"F|f|filepattern|filetype=", "If -command is Build or Teardown: File type to copy (e.g. 'mdf' or 'sql').\nIf command is Generate or Validate: File (pattern) to write DNA string to, or read DNA string from. If -command is Generate: Any '*' will be replaced with a number, for each generated file according to the -numberofdnastrings parameter. If -command is Validate, '*' is a wildcard character that matches any characters.", v => p.FilesOrFileTypes.Add(v) },
                    {"N|n|numberofdnastrings=", "If -command is Generate: Number of files to write (different) DNA strings to.", v => p.m_numberOfOutputFiles = v },
                    {"S|s|seed=", "If -command is Generate: Random seed (float number) for DNA generation.", v => p.m_seedString = v },
                };
            var extraArgs = optionSet.Parse(args);

            #region Validation of parameters and parameter combinations
            string error = String.Empty;

            #region General validation
            if (extraArgs.Count > 0)
            {
                error += "Unknown parameter: " + extraArgs[0] + ".\n";
            }
            if (p.CommandString is null)
            {
                error += "You must specify --command.\n";
            }
            if (p.Command == CommandEnum.Unspecified)
            {
                error += "Invalid command specified for --command. Pass --help for usage information.\n";
            }
            #endregion

            #region Validation of additional parameters for command Build and Teardown
            if ((p.Command == CommandEnum.Build || p.Command == CommandEnum.Teardown)
                && (!String.IsNullOrEmpty(p.Destination) && p.Origins.Count == 0))
            {
                error += "If you specify --destination, you must also specify --origin.\n";
            }
            #endregion

            #region Validation of additional parameters for Generate/Validate DNA
            string generateDNAFilePattern = String.Empty;
            List<string> dnaFilePatterns = new List<string>();
            if (p.Command == CommandEnum.GenerateDNA)
            {
                switch (p.FilesOrFileTypes.Count)
                {
                    case 0:
                        generateDNAFilePattern = DefaultDNAStringOutputFilePattern;
                        break;
                    case 1:
                        generateDNAFilePattern = p.FilesOrFileTypes[0];
                        break;
                    default:
                        error += "For command -Generate, you cannot specify more than one name/pattern as DNA -file.\n";
                        break;
                }
                if (p.NumberOfOutputFiles > 1
                    && !generateDNAFilePattern.Contains("*"))
                {
                    generateDNAFilePattern += "*";
                }
            }
            if (p.Command == CommandEnum.ValidateDNA)
            {
                switch (p.FilesOrFileTypes.Count)
                {
                    case 0:
                        error += "For validation, you must specify at least one name/pattern as DNA -file.\n";
                        break;
                }
            }
            #endregion

            #region Expand directories to be more informative when printed 
            if (String.IsNullOrEmpty(p.Destination))
            {
                p.Destination = Directory.GetCurrentDirectory();
            }
            else
            {
                p.Destination = Path.GetFullPath(p.Destination);
            }
            for (int i = 0; i < p.Origins.Count; i++)
            {
                p.Origins[i] = Path.GetFullPath(p.Origins[i]);
            }
            #endregion
            #endregion

            #region If no error found so far...
            if (String.IsNullOrEmpty(error))
            {
                //1. Any of the simple options? Perform and return.
                if (p.Help)
                {
                    ShowUsage(optionSet);
                    return false;
                }

                if (p.Version)
                {
                    var assembly = typeof(Parameters).Assembly;
                    var name = Path.GetFileNameWithoutExtension(assembly.Location).ToLowerInvariant();
                    var version = assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false).Cast<AssemblyFileVersionAttribute>().Select(a => a.Version).FirstOrDefault();
                    Console.Error.WriteLine("{0} version {1}", name, version);
                    return false;
                }

                //Switch on command
                switch (p.Command)
                {
                    case CommandEnum.Build:
                    case CommandEnum.Teardown:
                        return !ExecuteDatabaseCommands(p.Command, p.Origins, p.Destination, p.FilesOrFileTypes);
                    case CommandEnum.GenerateDNA:
                        GenerateDNAStrings(p.Destination, generateDNAFilePattern, p.NumberOfOutputFiles, p.Seed);
                        return false;
                    case CommandEnum.ValidateDNA:
                        ValidateDNAStrings(p.Origins, p.FilesOrFileTypes);
                        break;
                }

            }
            #endregion
            else
            {
                Console.Error.WriteLine(error);
                Console.Error.WriteLine("Pass --help for usage information.");
                return false;
            }
            parameters = p;
            return true;
        }

        #endregion

        #region Static helper functions
        private static bool ExecuteDatabaseCommands(CommandEnum command, IEnumerable<string> origins, string destination, IEnumerable<string> filesOrFileTypes)
        {
            //1. Make sure directory exists (to avoid errors further down).
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            //2. Delete and copy files.
            foreach (var fileType in filesOrFileTypes)
            {
                Console.WriteLine("Now attempting to delete all files of type {0} from folder {1}.", fileType, destination);
                var nOfilesDeleted = FileUtil.DeleteFiles(destination, fileType);
                Console.WriteLine("{0} files deleted.", nOfilesDeleted);
            }

            foreach (var originalPath in origins)
            {
                foreach (var fileType in filesOrFileTypes)
                {
                    Console.WriteLine("Now attempting to copy files of type {0} from folder {1} to folder {2}.", fileType, originalPath, destination);
                    string filePattern = String.Concat("*.", fileType);
                    var nOfFilesCopied = FileUtil.CopyFiles(originalPath, destination, filePattern);
                    Console.WriteLine("{0} files copied.", nOfFilesCopied);
                }
            }

            //3. Validate that there is one and only one mdf file in the destination folder
            var mdfFileNames = Directory.EnumerateFiles(destination, "*.mdf");
            if (mdfFileNames.Count() != 1)
            {
                Console.Error.WriteLine("There must be one and only one .mdf file in the destination folder {0}. The folder now contains {1} .mdf files.", destination, mdfFileNames.Count().ToString());
                return false;
            }

            //4. Execute the command, and return.
            string mdfFilePath = mdfFileNames.ElementAt(0);
            string connectionString = $"Server=(LocalDB)\\MSSQLLocalDB;"
                                      + $"Integrated Security = true;"
                                      + $"AttachDbFileName = {mdfFilePath}";

            if (command == CommandEnum.Build)
            {
                Console.WriteLine("Now attempting to build database, using SQL files in folder {0}.", destination);
                DbObjectManager.CreateDbObjects(connectionString, destination);
                Console.WriteLine("Build database operation succeeded.");
            }

            if (command == CommandEnum.Teardown)
            {
                Console.WriteLine("Now attempting to tear down database, using SQL files in folder {0}.", destination);
                DbObjectManager.DropDbObjects(connectionString, destination);
                Console.WriteLine("Tear down database operation succeeded.");
            }
            return true;
        }

        private static void GenerateDNAStrings(string destinationFolderPath, string filePattern, int numberOfDNAStrings, int seed)
        {
            //1. Create directory if nonexistent
            if (!Directory.Exists(destinationFolderPath))
            {
                Directory.CreateDirectory(destinationFolderPath);
            }

            //2. Prepare for filenames with number
            var nOfDigits = numberOfDNAStrings.ToString().Length;
            var formatString = "{0:D" + nOfDigits.ToString() + "}";

            //3. Prepare for using random library
            Random random = new Random(seed);

            //4. Loop through n times
            Console.WriteLine("Now generating {0} DNA string files.", numberOfDNAStrings);
            for (int i = 0; i < numberOfDNAStrings; i++)
            {
                //a. Generate file name
                var currentStringNumber = String.Format(formatString, i + 1);
                var currentFileName = filePattern.Replace("*", currentStringNumber);
                var currentFileNameWithPath = Path.Combine(destinationFolderPath, currentFileName);

                Console.WriteLine("  Now generating a DNA string and writing to file {0}.", currentFileNameWithPath);

                //b. Generate DNA string
                var dnaGenerator = new DNAGenerator(random);
                var dnaString = dnaGenerator.Generate();

                //c. Write to file
                File.WriteAllText(currentFileNameWithPath, dnaString);

                Console.WriteLine("  DNA String generated and written to file '{0}'.", currentFileNameWithPath);
            }
            Console.WriteLine("A total of {0} DNA Strings generated and written to file.", numberOfDNAStrings);
        }

        private static void ValidateDNAStrings(IEnumerable<string> originFolderPaths, IEnumerable<string> filePatterns)
        {
            List<string> fileNames = new List<string>();

            //1. First make a list of the files to validate
            if (originFolderPaths.Count() == 0)
            {
                foreach (var filePattern in filePatterns)
                {
                    fileNames.Concat(Directory.EnumerateFiles(Directory.GetCurrentDirectory(), filePattern));
                }
            }
            foreach (var originFolderPath in originFolderPaths)
            {
                foreach (var filePattern in filePatterns)
                {
                    fileNames.Concat(Directory.EnumerateFiles(originFolderPath, filePattern));
                }
            }

            Console.WriteLine("Now reading and validating {0} files.", fileNames.Count());

            //2. Call Validate() for each file
            var validationResults = new List<Tuple<string, DNAValidationResult>>();
            foreach (var fileName in fileNames)
            {
                Console.WriteLine("  Now reading and validating file '{0}'.", fileName);

                string fileContents = File.ReadAllText(fileName);
                Console.WriteLine("  File '{0}' read.", fileName);

                DNAString dnaString = new DNAString(fileContents, DNAValidator.DefaultValidator);
                var validationResult = dnaString.Validate();

                Console.WriteLine("  File '{0}' validated, with results:", fileName);
                foreach (var validationResultString in PrettyPrinter.PrettyPrint(validationResult))
                {
                    Console.WriteLine("    " + validationResultString);
                }
            }
            Console.WriteLine("{0} DNA string files read and validated.", fileNames.Count());
        }

        private static void ShowUsage(OptionSet optionSet)
        {
            optionSet.WriteOptionDescriptions(Console.Error);
            Console.Error.WriteLine("This utility app implements commands for Tobias system management, specified with option -command. In addition, with option -origin, files are copied before the command is executed.");
        }
        #endregion
    }
}