﻿using System;
using System.IO;
using System.Linq;

namespace AzAnt
{
    class AzAnt
    {
        private readonly AzAntArgs _Args = null;
        public AzAnt(AzAntArgs args)
        {
            _Args = args;
        }

        private void LogInfo(string str)
        {
            Console.WriteLine(str);
        }

        private void LogDebug(string str)
        {
            if(_Args.Verbose)
            {
                Console.WriteLine(str);
            }
        }

        public void Run()
        {
            DelimitedFileParser parser;

            LogInfo("Copyright Potts Software, 2019\n");
            LogDebug(_Args.GetSettingsString());

            if(_Args.WorkingDir != ".")
            {
                LogDebug($"Changing working directory to: {GetFullFileName(_Args.WorkingDir)} ({_Args.WorkingDir})");
                Directory.SetCurrentDirectory(_Args.WorkingDir);
            }

            string tokensFile = GetTokensFile();
            if(tokensFile == null)
            {
                LogInfo("No tokens file found. Exiting.");
                return;
            }
            LogInfo("Using tokens input file: " + GetFullFileName(tokensFile));

            try
            {
                parser = new DelimitedFileParser
                {
                    NameField = _Args.NameColumn
                };

                parser.ParseFile(tokensFile);
                LogDebug("Available columns: [" + String.Join(" | ", parser.Columns) + "]");
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to parse input file:\n" + ex.Message);
            }

            var values = parser.GetFieldValues(_Args.Environment);
            if (_Args.Verbose || _Args.QueryOnly)
            {
                LogInfo($"Tokens for environment {_Args.Environment}:");
                foreach((string Name, string Value) in values)
                {
                    LogInfo($"{_Args.Prefix}{Name}{_Args.Postfix} => {Value}");
                }

                if(_Args.QueryOnly)
                {
                    return;
                }
            }

            // For each token file replace all of the tokens and rename it.
            string[] tokenizedFiles = GetTokenizedFiles();

            LogDebug("TargetFiles found: " + string.Join(", ", tokenizedFiles));

            for(int idx = 0; idx < tokenizedFiles.Length; idx++)
            {
                string tokenizedFile = tokenizedFiles[idx];
                string outFile = GetOutputFilename(tokenizedFile);

                tokenizedFile = GetFullFileName(tokenizedFile);
                outFile = GetFullFileName(outFile);

                LogInfo($"Parsing file [{idx}] {tokenizedFile}");
                string contents = File.ReadAllText(tokenizedFile);

                foreach((string Name, string Value) in values)
                {
                    string name = _Args.Prefix + Name + _Args.Postfix;
                    //contents = contents.Replace(name, Value, StringComparison.OrdinalIgnoreCase);
                    contents = contents.Replace(name, Value);
                }

                LogInfo($"Generating file [{idx}] {outFile}");
                if(_Args.WhatIf)
                {
                    LogInfo(contents + "<\n");  // Print a "<" to indicate the end of the file.
                }
                else
                {
                    File.WriteAllText(outFile, contents);
                }
            }
        }

        private string GetTokensFile()
        {
            var files = Directory.EnumerateFiles(".", _Args.VariablesFile).ToArray();
            if (files.Count() == 0)
            {
                return null;
            }

            return files[0];
        }

        private string[] GetTokenizedFiles()
        {
            // Recursive?
            SearchOption so = (_Args.RecursiveTokenFileSearch) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            string srch = "*" + _Args.TokenizedFileSearchString + "*";
            var files = Directory.EnumerateFiles(".", srch, so).ToArray();
            if (files.Count() == 0)
            {
                throw new Exception("No tokenized files found. Exiting.");
            }

            return files;
        }

        private string GetOutputFilename(string fileName)
        {
            string output = fileName.Replace(_Args.TokenizedFileSearchString, _Args.SubstitutionGeneratedFile);

            if(_Args.RootGeneratedPath != ".")
            {
                output = ".\\" + _Args.RootGeneratedPath + "\\" + output;
            }
            
            if(_Args.GeneratedFileRelativeOutputPath != ".")
            {
                FileInfo fi = new FileInfo(output);
                output = fi.DirectoryName + "\\" + _Args.GeneratedFileRelativeOutputPath + "\\" + fi.Name;
            }

            return GetFullFileName(output);
        }

        private string GetFullFileName(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            return fi.FullName;
        }
    }
}
