using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LanguageGrid _languageGrid;

        public MainWindow()
        {
            InitializeComponent();
            InitLanguageComponents();
        }

        private void InitLanguageComponents()
        {
            _languageGrid = new LanguageGrid();
            ContentPanel.Child = _languageGrid;

            /** DEBUG
            VocabularySet set = new VocabularySet("English");
            set.Description = "My English dirty vocabularies";

            Word w1 = new Word { Level = 1, Timestamp = 0, Term = "fuck", Translation = new string[] { "ficken", "scheisse", "beschissen" } };
            Word w2 = new Word { Level = 1, Timestamp = 0, Term = "suck", Translation = new string[] { "saugen", "lutschen" } };
            Word w3 = new Word { Level = 1, Timestamp = 0, Term = "servant", Translation = new string[] { "Diener" } };

            set.Add(w1);
            set.Add(w2);
            set.Add(w3);
            set.Save();
            */
        }
    }
}