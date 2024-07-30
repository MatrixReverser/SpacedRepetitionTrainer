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
    /// Interaktionslogik für SessionPanel.xaml
    /// </summary>
    public partial class SessionPanel : UserControl
    {
        public event EventHandler<string> CancelSessionEvent; 

        private VocabularySet _vocabularySet;
        private LearnConfig _learnConfig;

        public SessionPanel(VocabularySet vocabularySet, LearnConfig config)
        {
            InitializeComponent();

            _vocabularySet = vocabularySet;
            _learnConfig = config;
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

        private void LeftMouseButtonDown_Cancel(object sender, MouseButtonEventArgs args)
        {
            CancelSessionEvent.Invoke(this, string.Empty);
        }
    }
}
