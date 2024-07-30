using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaktionslogik für LearnConfigPanel.xaml
    /// </summary>
    public partial class LearnConfigPanel : UserControl
    {
        public event EventHandler<bool> ConfigConfirmed;

        public LearnConfigPanel()
        {
            InitializeComponent();
        }

        private void TextBox_NumberFilter(object sender, TextCompositionEventArgs e)
        {
            // Regex zur Überprüfung, ob die Eingabe eine Ziffer ist
            Regex regex = new Regex("[^0-9]+"); // Erlaubt nur Ziffern
            e.Handled = regex.IsMatch(e.Text); // Setzt Handled auf true, wenn es keine Ziffer ist
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

        private void LeftMouseButtonDown_Learn(object sender, MouseButtonEventArgs args)
        {
            ConfigConfirmed.Invoke(this, true);
        }

        private void LeftMouseButtonDown_Cancel(object sender, MouseButtonEventArgs args)
        {
            ConfigConfirmed?.Invoke(this, false);
        }

        /**
         * returns a configuration object of the data that has been entered in this panel
         */
        public LearnConfig GetConfiguration()
        {
            LearnConfig config = new LearnConfig();

            config.Count = int.Parse(TextWordCount.Text);

            if (((RadioButton)RadioModePanel.Children[0]).IsChecked == true) { config.Mode = LearnMode.MIXED; }
            else if (((RadioButton)RadioModePanel.Children[1]).IsChecked == true) { config.Mode = LearnMode.CARD; }
            else if (((RadioButton)RadioModePanel.Children[2]).IsChecked == true) { config.Mode = LearnMode.MULTIPLE_CHOICE; }
            else { config.Mode = LearnMode.WRITE; }

            if (((RadioButton)RadioDirectionPanel.Children[0]).IsChecked == true) { config.Direction = LearnDirection.MIXED; }
            else if (((RadioButton)RadioDirectionPanel.Children[1]).IsChecked == true) { config.Direction = LearnDirection.TO_TRANSLATION; }
            else {  config.Direction = LearnDirection.FROM_TRANSLATION; }

            return config;
        }
    }
}
