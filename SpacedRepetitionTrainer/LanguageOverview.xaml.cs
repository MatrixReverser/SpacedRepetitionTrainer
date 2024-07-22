using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpacedRepetitionTrainer
{
    /// <summary>
    /// Interaktionslogik für LanguageOverview.xaml
    /// </summary>
    public partial class LanguageOverview : UserControl
    {
        private string _language;
        private VocabularySet _vocabularySet;

        public LanguageOverview(string language)
        {
            InitializeComponent();
            
            _language = language;
            _vocabularySet = new VocabularySet(language);
            _vocabularySet.Load();

            InitInfoBlock();
        }

        private void InitInfoBlock()
        {
            Description.Text = _vocabularySet.Description;
            WordCount.Text = "Wörter: " + _vocabularySet.GetWordCount();
        }
    }
}
