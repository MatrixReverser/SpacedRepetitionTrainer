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
        private string _language;
        private VocabularySet _vocabularySet;

        public LanguageOverview(string language)
        {
            InitializeComponent();
            
            _language = language;
            _vocabularySet = new VocabularySet(language);
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
    }

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

    public class UnixTimestampToDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is long unixTimestamp)
            {
                // Unix timestamp is seconds past epoch
                DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;
                // Return only the date part in German format
                return dateTime.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
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
}
