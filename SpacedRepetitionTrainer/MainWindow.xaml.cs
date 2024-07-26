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
        private LanguageOverview _languageOverview;

        public MainWindow()
        {
            InitializeComponent();
            InitLanguageComponents();

            this.Closing += MainWindow_Closing;
        }

        /**
         * Initializes the home screen with the language grid
         */
        private void InitLanguageComponents()
        {
            _languageGrid = new LanguageGrid();
            _languageGrid.TileClicked += LanguageGrid_TileClicked;
            ContentPanel.Child = _languageGrid;
            AppTitle.Text = "Vokabel Sets";
        }

        /**
         * Initializes the screen that handles one language pack
         */
        private void InitLanguagePack(string language)
        {
            _languageOverview = new LanguageOverview(language);
            _languageOverview.HomeScreenRequested += LanguageOverview_HomeScreenRequested;
            ContentPanel.Child = _languageOverview;
            AppTitle.Text = language;
        }

        /**
         * Is called if the user has clicked the back button in the language overiew screen
         */
        private void LanguageOverview_HomeScreenRequested(object? sender, string e)
        {
            _languageOverview.SaveCurrentVocabularySet();
            _languageOverview.HomeScreenRequested -= LanguageOverview_HomeScreenRequested;
           
            InitLanguageComponents();
        }

        // is called if the user clicked on a language tile in the Main screen
        private void LanguageGrid_TileClicked(object? sender, string language)
        {
            InitLanguagePack(language);
            _languageGrid.TileClicked -= LanguageGrid_TileClicked;
        }

        /**
         * Application is about to close
         */
        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _languageOverview?.SaveCurrentVocabularySet();
        }

        
    }
}