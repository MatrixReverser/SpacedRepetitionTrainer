using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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
using System.Xml.Linq;

namespace SpacedRepetitionTrainer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LanguageGrid _languageGrid;
        private LanguageOverview _languageOverview;
        private NewLanguagePanel _newLanguagePanel;
        private LearnConfigPanel _learnPanel;
        private SessionPanel _sessionPanel;

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
            _languageOverview.LearningSessionRequested += LanguageOverview_StartLearningSession;
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
            if (_languageOverview != null)
            {
                _languageOverview?.SaveCurrentVocabularySet();
            }
        }

        
        private void NewLanguagePack_Click(object sender, RoutedEventArgs e)
        {
            if (ContentPanel.Child == _languageOverview)
            {
                _languageOverview.SaveCurrentVocabularySet();
            }

            _newLanguagePanel = new NewLanguagePanel();
            ContentPanel.Child = _newLanguagePanel;
            AppTitle.Text = "Neues Sprachpaket";
            _newLanguagePanel.DialogClosed += HandleCloseNewLanguageDialog;
        }


        private void HandleCloseNewLanguageDialog(object? sender, bool confirmed)
        {
            if (confirmed)
            {
                string name = _newLanguagePanel.GetLanguageName();
                string description = _newLanguagePanel.GetLanguageDescription();

                _languageOverview = new LanguageOverview(name, description);
                ContentPanel.Child = _languageOverview;
                _languageOverview.HomeScreenRequested += LanguageOverview_HomeScreenRequested;
                _languageOverview.LearningSessionRequested += LanguageOverview_StartLearningSession;

                AppTitle.Text = name;
            }
            else
            {
                // if cancelled, just go back to the home screen
                _newLanguagePanel.DialogClosed -= HandleCloseNewLanguageDialog;
                InitLanguageComponents();
            }
        }

        /**
         * Is called if the user wants to start learning (config dialog is opened)
         */
        public void LanguageOverview_StartLearningSession(object? sender, string args)
        {
            _learnPanel = new LearnConfigPanel();
            ContentPanel.Child = _learnPanel;
            _learnPanel.ConfigConfirmed += ConfigConfirmed;
        }

        /**
         * Is called if the config learn has been confirmed (either with Ok or with cancel)
         */
        private void ConfigConfirmed(object? sender, bool arg)
        {
            _learnPanel.ConfigConfirmed -= ConfigConfirmed;

            if (!arg)
            {
                // config panel has been canceled
                if (_languageOverview != null)
                {
                    _languageOverview.HomeScreenRequested += LanguageOverview_HomeScreenRequested;
                    _languageOverview.LearningSessionRequested += LanguageOverview_StartLearningSession;
                    ContentPanel.Child = _languageOverview;
                    AppTitle.Text = _languageOverview.GetLanguageName();
                }
                else
                {
                    InitLanguageComponents();
                }
            }
            else
            {
                LearnConfig config = _learnPanel.GetConfiguration();
                VocabularySet vocabularySet = _languageOverview.GetVocabularySet();

                _sessionPanel = new SessionPanel(vocabularySet, config);
                ContentPanel.Child = _sessionPanel;
                _sessionPanel.CancelSessionEvent += CancelSession;
            }
        }

        /**
         * Is called if the session is cancelled or ends
         */
        private void CancelSession(object? sender, string args)
        {
            if (_languageOverview != null)
            {
                _languageOverview.HomeScreenRequested += LanguageOverview_HomeScreenRequested;
                _languageOverview.LearningSessionRequested += LanguageOverview_StartLearningSession;
                _languageOverview.Refresh();
                ContentPanel.Child = _languageOverview;
                AppTitle.Text = _languageOverview.GetLanguageName();
            }
            else
            {
                InitLanguageComponents();
            }
        }

        /**
         * Is called if the mouse enters ANY of the panels that represets buttons
         */
        private void MouseEnter_General(object sender, MouseEventArgs args)
        {
            this.Cursor = Cursors.Hand;
        }

        /**
         * Is called if the mouse leaves ANY of the panels that represent buttons
         */
        private void MouseLeave_General(object sender, MouseEventArgs args)
        {
            this.Cursor = Cursors.Arrow;
        }
    }
}