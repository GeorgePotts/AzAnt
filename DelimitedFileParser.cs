using System;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using System.Linq;

namespace AzAnt
{
    public class DelimitedFileParser
    {
        public string[] Columns = null;
        public string Separator = ",";
        public string NameField = "Name";
        public bool HasFieldsEnclosedInQuotes = true;
        public bool TrimWhiteSpace = true;

        public delegate void WriteMessage(string message);

        private Dictionary<string, string[]> AllData = new Dictionary<string, string[]>();
        private int NameIndex = -1;

        public bool ParseFile(string filename)
        {
            using (TextFieldParser parser = new TextFieldParser(filename))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(Separator);
                parser.HasFieldsEnclosedInQuotes = true;
                parser.TrimWhiteSpace = false;

                while (parser.PeekChars(1) != null)
                {
                    string[] cells = parser.ReadFields().ToArray();
                    if (Columns == null)
                    {
                        if (cells[0].Length >= 4 && (cells[0].Substring(0, 4).ToUpper() == "SEP="))
                        {
                            // We accept a "sep=<char>" first line to set the separator.
                            // This is used by Microsoft Excel in order to better handle delimited files.
                            // (You can use \t or an actual embedded tab for tab delimiting.)
                            if (cells[0].Length > 4)
                            {
                                // If the separator is anything then it was different than the default of ','
                                Separator = cells[0].Substring(4);
                                if (Separator == "\\t") { Separator = "\t"; }    // Handle tabs via "sep=\t"
                                parser.SetDelimiters(Separator);
                            }
                        }
                        else
                        {
                            Columns = cells.Select(x => x.Trim()).ToArray();
                            NameIndex = GetFieldIndex(NameField);
                        }
                    }
                    else
                    {
                        if(cells.Length < Columns.Length)
                        {
                            throw new Exception("Incomplete information for data row: " + string.Join(" | ", cells));
                        }

                        if(TrimWhiteSpace)
                        {
                            var data = cells.Select(x => x.Trim()).ToArray();
                            AllData.Add(cells[NameIndex], data);
                        }
                        else
                        {
                            AllData.Add(cells[NameIndex], cells.ToArray());
                        }
                    }
                }
            }

            return true;
        }

        private int GetFieldIndex(string field)
        {
            int fieldIndex = -1;

            for (int idx = 0; idx < Columns.Count(); idx++)
            {
                if (Columns[idx].ToUpper() == field.ToUpper())
                {
                    fieldIndex = idx;
                    break;
                }
            }

            if(fieldIndex == -1)
            {
                throw new Exception("Field not found : " + field);
            }

            return fieldIndex;
        }

        public (string Name, string Value)[] GetFieldValues(string field)
        {
            int fieldIndex = GetFieldIndex(field);

            List<(string Name, string Value)> Values = new List<(string Name, string Value)>();

            foreach (KeyValuePair<string, string[]> kvp in AllData)
            {
                Values.Add((kvp.Key, kvp.Value[fieldIndex]));
            }

            return Values.ToArray();
        }
    }
}
