using System;
using System.Collections.Generic;
using System.IO;
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

namespace SpacedRepetitionTrainer
{
    /// <summary>
    /// Interaktionslogik für LanguageGrid.xaml
    /// </summary>
    public partial class LanguageGrid : UserControl
    {
        public event EventHandler<string>? TileClicked;

        private Dictionary<Border, VocabularySet> _vocSets = new Dictionary<Border, VocabularySet>();

        public LanguageGrid()
        {
            InitializeComponent();
            BuildLanguageGrid();
        }

        private void BuildLanguageGrid()
        {
            string homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string jsonDirectory = System.IO.Path.Combine(homeDirectory, VocabularySet.DATA_PATH);
            Directory.CreateDirectory(jsonDirectory);

            string[] files = Directory.GetFiles(jsonDirectory, "*.json");

            foreach (string languageFile in files) 
            {
                AddLanguageTile(languageFile);
            }
        }

        private void AddLanguageTile(string languageFile)
        {
            string language = Path.GetFileNameWithoutExtension(languageFile);
            VocabularySet set = new VocabularySet(language);
            set.Load();
            int wordCount = set.GetWordCount();

            // Erstelle den Border (Rahmen) für das Rechteck
            Border border = new Border
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(255, 140, 0)),  // Orange Border
                BorderThickness = new Thickness(2),
                Background = new SolidColorBrush(Color.FromRgb(26, 26, 26)),  // Dunkelgrauer Hintergrund
                Margin = new Thickness(10),
                Padding = new Thickness(10)
            };

            // Erstelle ein StackPanel für die Labels
            StackPanel stackPanel = new StackPanel();

            // Erstelle und füge die Labels hinzu
            TextBlock labelName = new TextBlock
            {
                Text = char.ToUpper(language[0]) + language.Substring(1),
                Foreground = Brushes.White,
                FontSize = 18,
                FontWeight = FontWeights.Bold
            };

            TextBlock labelFile = new TextBlock
            {
                Text = "[file: " + languageFile + "]",
                Foreground = Brushes.LightGray,
                FontSize = 12,
                FontStyle = FontStyles.Italic,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 15)
            };

            TextBlock labelDescription = new TextBlock
            {
                Text = set.Description,
                Foreground = Brushes.LightGray,
                FontSize = 12,
                FontWeight = FontWeights.SemiBold,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 15)
            };

            TextBlock labelDesc = new TextBlock
            {
                Text = "Wörter: " + wordCount,
                Foreground = Brushes.LightGray,
                FontSize = 12
            };

            stackPanel.Children.Add(labelName);
            stackPanel.Children.Add(labelFile);
            stackPanel.Children.Add(labelDescription);
            stackPanel.Children.Add(labelDesc);

            // Füge das StackPanel zum Border hinzu
            border.Child = stackPanel;

            // Füge den Border zum StackPanel des UserControls hinzu
            TileGrid.Children.Add(border);

            // Event, wenn Benutzer auf Tile klickt
            _vocSets.Add(border, set);
            border.MouseDown += Border_MouseDown;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Border? border = sender as Border;
            if (border != null)
            {
                VocabularySet set = _vocSets[border];

                TileClicked?.Invoke(this, set.Name as string);
            }
        }
    }
}
