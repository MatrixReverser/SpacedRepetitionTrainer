using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacedRepetitionTrainer
{
    class Word
    {
        public string Term { get; set; }
        public string[] Translation { get; set; }
        public long Timestamp { get; set; }
        public int Level { get; set; }

        public Word() 
        {
            Term = string.Empty;
            Translation = new string[] { String.Empty, String.Empty, String.Empty };
            Timestamp = 0;
            Level = 0;
        }
    }

    class VocabularySet
    {
        private List<Word> words;

        public VocabularySet()
        {
            words = new List<Word>();
        }
    }
}
