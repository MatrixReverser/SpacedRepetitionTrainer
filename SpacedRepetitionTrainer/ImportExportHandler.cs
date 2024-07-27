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
    }
}
