using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public event EventHandler<string>? HomeScreenRequested;

        private string _language;
        private VocabularySet _vocabularySet;

        public LanguageOverview(string language, string description = "")
        {
            InitializeComponent();
            
            _language = language;            
            _vocabularySet = new VocabularySet(language);
            _vocabularySet.Description = description;
            _vocabularySet.Load();

            InitInfoBlock();
            InitDataGrid();
        }

        private void InitInfoBlock()
        {
            Description.Text = _vocabularySet.Description;
            WordCount.Text = "Wörter: " + _vocabularySet.GetWordCount();
        }

        private void InitDataGrid()
        {
            VocabularyGrid.ItemsSource = _vocabularySet.Words;
        }

        /**
         * Saves the current vocabulary set
         */
        public void SaveCurrentVocabularySet()
        {
            _vocabularySet?.Save();
        }

        /**
         * Is called if the user clicks the back button
         */
        private void LeftMouseButtonDown_Back(object sender, MouseButtonEventArgs args)
        {
            HomeScreenRequested?.Invoke(this, string.Empty);
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

        /**
         * Is called if the user clicks on delete
         */
        private void LeftMouseButtonDown_Delete(object sender, MouseButtonEventArgs args)
        {
            string title = "Sprachpaket löschen?";
            string message = "Möchtest Du das Sprachpaket '" + _language + "' wirklich löschen?\n\nDieser Vorgang kann nicht rückgängig gemacht werden!";

            MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _vocabularySet.Delete();
                HomeScreenRequested?.Invoke(this, string.Empty);
            }
        }

        /**
         * Is called if the user wants to export the current language pack to a CSV file
         */
        private void LeftMouseButtonDown_Export(object sender, MouseButtonEventArgs args)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Vokabeln in Datei exportieren";
            saveFileDialog.Filter = "CSV Datei (*.csv)|*.csv";
            saveFileDialog.FileName = _vocabularySet.Name + ".csv";

            if (saveFileDialog.ShowDialog() == true)
            {
                ImportExportHandler exporter = new ImportExportHandler(_vocabularySet);
                exporter.Export(saveFileDialog.FileName);
            }
        }

        /**
         * Is called if the user wants to import words from a CSV file to the current language pack
         */
        private void LeftMouseButtonDown_Import(object sender, MouseButtonEventArgs args)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Vokabeln aus Datei importieren";
            openFileDialog.Filter = "CSV Datei (*.csv)|*.csv";
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == true)
            {
                bool appendMode = false;

                // ask if previous words should be overwritten or appended, if there are words in the current language pack
                if (_vocabularySet.Words.Count > 0)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Möchtest Du die vorhandenen Vokabeln überschreiben?\n(Falls Du nein wählst, werden die neuen Vokabeln\nan die existierenden Vokabeln angehängt.)",
                        "Vokabeln überschreiben",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question
                    );
                    if (result == MessageBoxResult.No)
                    {
                        appendMode = true;
                    }
                }

                ImportExportHandler importer = new ImportExportHandler(_vocabularySet); 
                importer.Import(openFileDialog.FileName, appendMode);
            }
        }

        /** 
         * Is called if the user wants to delete the current row of the data grid
         */
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            var item = frameworkElement?.DataContext as Word;
            
            if (item != null && item is Word)
            {
                Word word = (Word)item;
                if (word.Term != null && word.Term.Length > 0)
                {
                    _vocabularySet.Words.Remove(item);
                }
            }
        }
    }

    /** **************************************************************************
     * HELPER CLASS: converting string array to string and vice versa
     **************************************************************************** */
    public class StringArrayToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string[] array)
            {
                return string.Join(", ", array);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                return str.Split(new[] { ", " }, StringSplitOptions.None);
            }
            return new string[0];
        }
    }

    /** **************************************************************************
     * HELPER CLASS: converting unix timestamp to date and vice versa
     **************************************************************************** */
    public class UnixTimestampToDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long unixTimestamp)
            {
                // Unix timestamp is seconds past epoch
                DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;
                // Return only the date part in German format
                return dateTime.ToLocalTime().ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str && DateTime.TryParseExact(str, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {   
                return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
            }
            return 0L;
        }        
    }

    /** *************************************************************************
     * Controls visibility of the thrash icon for empty rows
     * ***************************************************************************/
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == CollectionView.NewItemPlaceholder.GetType())
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
