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
    /// Interaktionslogik für StatisticPanel.xaml
    /// </summary>
    public partial class StatisticPanel : UserControl
    {
        public event EventHandler<string> BackEvent;

        public StatisticPanel(VocabularySet vocabularySet)
        {
            InitializeComponent();

            int wordCount = vocabularySet.Words.Count;
            int[] levels = CalculateLevels(vocabularySet);

            AdjustBars(wordCount, levels);
        }

        /**
         * Adjust the bars to show correct values
         */
        private void AdjustBars(int wordCount, int[] levels)
        {
            BarLevel0.Maximum = wordCount;      BarLevel0.Value = levels[0];
            BarLevel1.Maximum = wordCount;      BarLevel1.Value = levels[1];
            BarLevel2.Maximum = wordCount;      BarLevel2.Value = levels[2];
            BarLevel3.Maximum = wordCount;      BarLevel3.Value = levels[3];
            BarLevel4.Maximum = wordCount;      BarLevel4.Value = levels[4];
            BarLevel5.Maximum = wordCount;      BarLevel5.Value = levels[5];
            BarLevel6.Maximum = wordCount;      BarLevel6.Value = levels[6];
            BarLevel7.Maximum = wordCount;      BarLevel7.Value = levels[7];
            BarLevel8.Maximum = wordCount;      BarLevel8.Value = levels[8];
            BarLevel9.Maximum = wordCount;      BarLevel9.Value = levels[9];
        }

        /**
         * Caclulates how many words belong to each level (0-9)
         */
        private int[] CalculateLevels(VocabularySet vocabularySet)
        {
            int[] levels = new int[10];

            foreach (Word word in vocabularySet.Words)
            {
                levels[word.Level]++;
            }

            return levels;
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
         * Is called if the user wants to go back to the languaGE OVERVIEW SCREE
         */
        public void LeftMouseButtonDown_Cancel(object sender, MouseButtonEventArgs args)
        {
            BackEvent?.Invoke(this, string.Empty);
        }
    }
}
