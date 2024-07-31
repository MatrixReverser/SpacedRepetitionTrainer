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
    /// Interaktionslogik für LearnCardPanel.xaml
    /// </summary>
    public partial class LearnCardPanel : UserControl
    {
        public event EventHandler<bool> QuestionDone;

        private List<Word> _learnSet;
        private List<Word> _failedSet;
        private LearnDirection _learnDirection;

        public LearnCardPanel(List<Word> learnSet, LearnDirection direction, List<Word> failedSet)
        {
            InitializeComponent();

            _failedSet = failedSet;
            _learnSet = learnSet;
            _learnDirection = direction;

            TermLabel.Text = GetQuestion();

            Button button = new Button();
            button.Content = "Karte umdrehen";
            button.FontSize = 14;
            button.Click += SwapCards;
            ButtonPanel.Children.Add(button);
        }

        /**
         * Shows the correct answer
         */
        private void SwapCards(object sender, RoutedEventArgs e)
        {
            ButtonPanel.Children.Clear();
            TermLabel.Text = GetAnswer();

            Button buttonYes = new Button();
            buttonYes.Content = "Ich lag richtig";
            buttonYes.FontSize = 14;
            buttonYes.Click += MarkSucceeded;
            ButtonPanel.Children.Add(buttonYes);

            Button buttonNo = new Button();
            buttonNo.Content = "Ich lag falsch";
            buttonNo.FontSize = 14;
            buttonNo.Margin = new Thickness(0, 10, 0, 0);
            buttonNo.Click += MarkFailed;
            ButtonPanel.Children.Add(buttonNo);
        }

        /**
         * Marks the current vocabulary as failed
         */
        private void MarkFailed(object sender, RoutedEventArgs e)
        {
            Word word = _learnSet[0];
            if (word.Level > 0)
            {
                word.Level--;
            }
            _failedSet.Add(word);
            QuitPanel(false);
        }

        /**
         * Marks the current vocabulary as succeeded
         */
        private void MarkSucceeded(object sender, RoutedEventArgs e)
        {
            Word word = _learnSet[0];
            if (word.Level < 9)
            {
                word.Level++;
            }
            QuitPanel(true);
        }

        /**
         * Sends an event that this panel can be quit
         */
        private void QuitPanel(Boolean succeeded)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            long unixTimestamp = now.ToUnixTimeSeconds();
            _learnSet[0].Timestamp = unixTimestamp;

            _learnSet.Remove(_learnSet[0]);
            QuestionDone?.Invoke(this, succeeded);
        }

        private string GetQuestion()
        {
            if (_learnDirection == LearnDirection.FROM_TRANSLATION)
            {
                return _learnSet[0].Term;
            }
            else
            {
                string[] translation = _learnSet[0].Translation;
                Random rand = new Random();
                return translation[rand.Next(0, translation.Count())];
            }
        }

        private string GetAnswer()
        {
            if (_learnDirection == LearnDirection.TO_TRANSLATION)
            {
                return _learnSet[0].Term;
            }
            else
            {       
                string result = string.Empty;
                string[] translation = _learnSet[0].Translation;
                foreach (string part in translation)
                {
                    if (result != string.Empty)
                    {
                        result += ", ";
                    }
                    result += part;
                }
                return result;
            }
        }


    }
}
