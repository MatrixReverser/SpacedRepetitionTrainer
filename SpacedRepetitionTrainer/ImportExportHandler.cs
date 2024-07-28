using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpacedRepetitionTrainer
{
    internal class ImportExportHandler
    {
        private VocabularySet _vocabularySet;

        /**
         * Constructor
         */
        public ImportExportHandler(VocabularySet vocabularySet)
        {
            _vocabularySet = vocabularySet;
        }

        /**
         * Exports the current vocabulary set to a csv file
         */
        public void Export(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename, false))
            {
                foreach (Word word in _vocabularySet.Words) 
                {
                    string colonSeparatedTranlsations = string.Empty;
                    foreach (string translation in word.Translation)
                    {
                        if (colonSeparatedTranlsations != string.Empty)
                        {
                            colonSeparatedTranlsations += ";";
                        }
                        colonSeparatedTranlsations += translation;
                    }

                    string line = string.Format("{0};{1};{2};{3}",
                        word.Level,
                        word.Timestamp,
                        word.Term,
                        colonSeparatedTranlsations);

                    writer.WriteLine(line);

                }
            }
        }

        /**
         * Imports words froma CSV file. If append is true, the new words will be added to the 
         * current vocabulary set. If false, rthe vocabulary set will be cleared before importing.
         */
        public void Import(string filename, bool append)
        {
            // delete old words if not in append mode
            if (!append)
            {
                _vocabularySet.Words.Clear();
            }

            using(StreamReader reader = new StreamReader(filename))
            {
                string? line = reader.ReadLine();

                while (line != null)
                {
                    Word word = BuildWord(line);
                    _vocabularySet.Words.Add(word);

                    line = reader.ReadLine();
                }
            }
        }

        /**
         * Creates a Word from a single CSV line
         */
        private Word BuildWord(string csvLine)
        {
            Word word = new Word();
            
            string[] columns = csvLine.Split(';');
            if (columns.Length >= 4)
            {
                word.Level = int.Parse(columns[0]);
                word.Timestamp = long.Parse(columns[1]);
                word.Term = columns[2].Trim();

                // only keep relevant trimmed words
                List<string> trimmedTranslations = new List<string>();

                for (int i=3; i<columns.Length; i++)
                {
                    string col = columns[i].Trim();
                    if (col.Length > 0)
                    {
                        trimmedTranslations.Add(col);
                    }
                }

                word.Translation = trimmedTranslations.ToArray();
            }

            return word;
        }
    }
}
