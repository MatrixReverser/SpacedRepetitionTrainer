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
            _languageGrid.TileClicked += LanguageGrid_TileClicked;
            ContentPanel.Child = _languageGrid;
        }

        // is called if the user clicked on a language tile in the Main screen
        private void LanguageGrid_TileClicked(object? sender, string language)
        {
            InitLanguagePack(language);
            _languageGrid.TileClicked -= LanguageGrid_TileClicked;
        }

        private void InitLanguagePack(string language)
        {
            ContentPanel.Child = null;
        }
    }
}