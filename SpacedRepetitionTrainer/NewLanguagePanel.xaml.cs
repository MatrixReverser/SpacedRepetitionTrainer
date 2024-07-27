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
    /// Interaktionslogik für NewLanguagePanel.xaml
    /// </summary>
    public partial class NewLanguagePanel : UserControl
    {
        public event EventHandler<bool>? DialogClosed;

        public NewLanguagePanel()
        {
            InitializeComponent();
        }

        /**
         * Is called if the mouse enters ANY of the panels that represets buttons
         */
        private void MouseEnter_General(object sender, MouseEventArgs args)
        {
            this.Cursor = Cursors.Hand;
        }

        public string GetLanguageName()
        {
            return TextFieldName.Text;
        }

        public string GetLanguageDescription()
        {
            return TextFieldDescription.Text;
        }

        /**
         * Is called if the mouse leaves ANY of the panels that represent buttons
         */
        private void MouseLeave_General(object sender, MouseEventArgs args)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void CancelClicked(object sender, MouseButtonEventArgs args)
        {
            DialogClosed?.Invoke(this, false);
        }

        private void SaveClicked(object sender, MouseButtonEventArgs args)
        {
            DialogClosed?.Invoke(this, true);
        }
    }
}
