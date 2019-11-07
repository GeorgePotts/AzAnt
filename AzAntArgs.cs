namespace AzAnt
{
    public class AzAntArgs
    {
        public string VariablesFile = "*.csv";
        public string TokenizedFileSearchString = ".token";
        public string SubstitutionGeneratedFile = "";
        public string GeneratedFileRelativeOutputPath = "";
        public string RootGeneratedPath = ".";
        public string Environment = "PROD";
        public string Prefix = "__";
        public string Postfix = null;
        public string NameColumn = "NAME";
        public string WorkingDir = ".";
        public bool RecursiveTokenFileSearch = false;
        public bool WhatIf = false;
        public bool Verbose = false;
        public bool QueryOnly = false;
        public string Error = null;

        public AzAntArgs() { }

        public static string Usage()
        {
            string name = System.AppDomain.CurrentDomain.FriendlyName;
            string str =
                $"Usage: {name} [i:varsfile] [t:tokenized] [s:sub] [e:environment] [x:prefix] [y:postfix] [d:dir] [h] [q] [r] [w] [v]\n" +
                "               [g:genfolder] [f:outrootfolder] [n:name column]\n\n" +

                " d The working directory. Default: .\n" +
                " e The column name of the environment to use. Default: PROD\n" +
                " f The output root folder of the directory structure for output files. Default: .\n" +
                " g The generated files directory relative to the working directory. Default: .\n" +
                " h Display this help message\n" +
                " i The file containing the variables. The first one found will be used. Default: *.csv\n" +
                " n The column to use to identify the tokens. Default: Name\n" +
                " q Query just the output tokens and skip processing.\n" +
                " r Perform a recursive search for tokenization files.\n" +
                " s The substitute substring of the generated output files. Default is an empty string.\n" +
                " t The unique substring of the tokenized filename(s) to process. Default: .token\n" +
                " w 'What if' - print any files to the console instead of writing to disk.\n" +
                " x The prefix for the tokens. Default: __\n" +
                " y The postfix for the tokens if different than the prefix.\n" +
                " v Verbose output. Default: False\n\n" +

                "Examples:\n" +
                $" {name} e:test\n" +
                $"      The first *.csv will be used to process [a].token[b] into [a].[b] for column 'test'\n" +
                $" {name} i:tokens.csv t:web.token.config o:web.config e:dev\n" +
                $"      Tokens.csv column 'dev' will be used to process web.token.config into web.config\n" +
                $" {name} n:token t:.repl. o:.\n" +
                $"     The first *.csv will transform [a].repl.[b] into [a].[b] using column name 'token' for values in column 'PROD'\n" +
                $" {name} q e:test\n" +
                $"     The token values for environment 'test' will be displayed, then exit with no files written.'\n" +
                "\n";
            return str;
        }

        public bool ParseArgs(string[] args)
        {
            string err = "Invalid command: ";
            bool help = false;
            bool ret = true;
            foreach (string arg in args)
            {
                string[] split = arg.Split(new char[] { ':' }, 2);
                if (split.Length == 2)
                {
                    switch (split[0].ToUpper())
                    {
                        case "I":
                            VariablesFile = split[1];
                            break;
                        case "T":
                            TokenizedFileSearchString = split[1];
                            break;
                        case "S":
                            SubstitutionGeneratedFile = split[1];
                            break;
                        case "G":
                            GeneratedFileRelativeOutputPath = split[1];
                            break;
                        case "F":
                            RootGeneratedPath = split[1];
                            break;
                        case "E":
                            Environment = split[1];
                            break;
                        case "X":
                            Prefix = split[1];
                            break;
                        case "Y":
                            Postfix = split[1];
                            break;
                        case "N":
                            NameColumn = split[1];
                            break;
                        case "D":
                            WorkingDir = split[1];
                            break;
                        default:
                            ret = false;
                            break;
                    }
                }
                else // Simple parameters
                {
                    switch (split[0].ToUpper())
                    {
                        case "H":
                            ret = false;
                            help = true;
                            break;
                        case "Q":
                            QueryOnly = true;
                            break;
                        case "R":
                            RecursiveTokenFileSearch = true;
                            break;
                        case "W":
                            WhatIf = true;
                            break;
                        case "V":
                            Verbose = true;
                            break;
                        default:
                            ret = false;
                            break;
                    }
                }

                if (ret == false)
                {
                    err += arg;
                    break;
                }
            }

            if (ret == false)
            {
                if(help == false)
                {
                    Error = err;
                }
            }

            // If Postfix wasn't set then default it to the value of Prefix.
            Postfix ??= Prefix;

            return ret;
        }

        public string GetSettingsString()
        {
            string str =
                $"Settings:\n" +
                $"VariablesFile: {VariablesFile}\n" +
                $"TokenizedFileSearchString: {TokenizedFileSearchString}\n" +
                $"SubstitutionGeneratedFile: {SubstitutionGeneratedFile}\n" +
                $"GenereatedFileRelativeOutputPath: {GeneratedFileRelativeOutputPath}\n" +
                $"RootGeneratedPath: {RootGeneratedPath}\n" +
                $"Environment: {Environment}\n" +
                $"Prefix: {Prefix}\n" +
                $"Postfix: {Postfix}\n" +
                $"NameColumn: {NameColumn}\n" +
                $"WorkingDir: {WorkingDir}\n" +
                $"RecursiveTokenFileSearch: {RecursiveTokenFileSearch}\n" +
                $"WhatIf: {WhatIf}\n" +
                $"Verbose: {Verbose}\n" +
                $"QueryOnly: {QueryOnly}\n";

            return str;
        }
    }
}
