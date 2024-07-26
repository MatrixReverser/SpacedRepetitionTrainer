using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Data;

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
            Translation = new string[] { String.Empty };
            Timestamp = 0;
            Level = 0;
        }
    }

    class VocabularySet
    {
        public static readonly string DATA_PATH = "SpacedRepetitionTrainer_Data";

        private string _setName;
        private Boolean _markedForDeletion = false;

        public ObservableCollection<Word> Words { get;  set; }
        public string Description { get; set; }
        public string Name
        {
            get { return _setName; }
        }

        private string SetName {  
            get { return _setName; }
            set { _setName = value; }
        }

        public VocabularySet()
        {
            _setName = string.Empty;
        }

        public VocabularySet(string setName)
        {
            Words = new ObservableCollection<Word>();
            _setName = setName;
            Description = string.Empty;
        }

        public void Add(Word word)
        {
            Words.Add(word);
        }

        private string CreateFilename()
        {
            string homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string jsonDirectory = System.IO.Path.Combine(homeDirectory, VocabularySet.DATA_PATH);
            Directory.CreateDirectory(jsonDirectory);

            string filename = System.IO.Path.Combine(jsonDirectory, _setName + ".json");

            return filename;
        }

        public void Save()
        {
            // must not be saved if set is marked for deletion! Otherwise the set would be recreated!
            if (!_markedForDeletion)
            {
                string filename = CreateFilename();
                JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
                string output = JsonSerializer.Serialize(this /*_words*/, options);

                File.WriteAllText(filename, output);
            }
        }

        public int GetWordCount()
        {
            return Words.Count;
        }

        public void Load()
        {
            string filename = CreateFilename();

            // return with an empty word list if language file does not exist
            if (!File.Exists(filename))
            {
                Words = new ObservableCollection<Word>();
                return;
            }

            string jsonString = File.ReadAllText(filename);
            VocabularySet? loadedSet = JsonSerializer.Deserialize<VocabularySet>(jsonString);

            if (loadedSet != null)
            {                
                this.Words = loadedSet.Words;
                this.Description = loadedSet.Description;
            }
        }

        public void Delete()
        {
            File.Delete(CreateFilename());
            _markedForDeletion = true;
        }
    }
}
