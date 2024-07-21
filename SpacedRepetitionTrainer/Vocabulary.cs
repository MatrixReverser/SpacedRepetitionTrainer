using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        public static readonly string DATA_PATH = "SpacedRepetitionTrainer_Data";

        private List<Word> _words;
        private string _setName;

        private string SetName {  
            get { return _setName; }
            set { _setName = value; }
        }

        public VocabularySet(string setName)
        {
            _words = new List<Word>();
            _setName = setName;
        }

        public void Add(Word word)
        {
            _words.Add(word);
        }

        public void Save()
        {
            string homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string jsonDirectory = System.IO.Path.Combine(homeDirectory, VocabularySet.DATA_PATH);
            Directory.CreateDirectory(jsonDirectory);

            string filename = System.IO.Path.Combine(jsonDirectory, _setName + ".json");
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string output = JsonSerializer.Serialize(_words, options);

            File.WriteAllText(filename, output);
        }

        public int GetWordCount()
        {
            Load();
            return _words.Count;
        }

        public void Load()
        {
            string homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string jsonDirectory = System.IO.Path.Combine(homeDirectory, VocabularySet.DATA_PATH);
            Directory.CreateDirectory(jsonDirectory);

            string filename = System.IO.Path.Combine(jsonDirectory, _setName + ".json");

            // return with an empty word list if language file does not exist
            if (!File.Exists(filename))
            {
                _words = new List<Word>();
                return;
            }

            string jsonString = File.ReadAllText(filename);
            var words = JsonSerializer.Deserialize<List<Word>>(jsonString);

            if (words == null)
            {
                _words= new List<Word>();
            } else
            {
                _words = words;
            }
        }
    }
}
