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
    /// Interaktionslogik für LearnWritePanel.xaml
    /// </summary>
    public partial class LearnWritePanel : UserControl
    {
        public event EventHandler<bool> QuestionDone;

        private List<Word> _learnSet;
        private List<Word> _failedSet;
        private LearnDirection _learnDirection;
        private List<string> _correctAnswers;

        public LearnWritePanel(List<Word> learnSet, LearnDirection direction, List<Word> failedSet)
        {
            InitializeComponent();

            _failedSet = failedSet;
            _learnSet = learnSet;
            _learnDirection = direction;
            _correctAnswers = new List<string>();

            TermLabel.Text = GetQuestion();
            CreateCorrectAnswers();

            TextAnswer.Focus();            
        }

        /**
         * Is called if the user hits ENTER inside the textbox for the answer
         */
        private void AnswerEntered(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string answer = TextAnswer.Text.ToLower();
                bool correct = false;

                foreach (string cmpAnswer in _correctAnswers)
                {
                    if (cmpAnswer == answer)
                    {
                        correct = true;
                        break;
                    }
                }

                if (correct)
                {
                    MarkSucceeded();
                } else
                {
                    MarkFailed();
                }
            }
        }

        /**
         * Marks the current vocabulary as failed
         */
        private void MarkFailed()
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
        private void MarkSucceeded()
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

        private void CreateCorrectAnswers()
        {
            if (_learnDirection == LearnDirection.TO_TRANSLATION)
            {
                _correctAnswers.Add(_learnSet[0].Term.ToLower());
            }
            else
            {                
                string[] translation = _learnSet[0].Translation;
                foreach (string part in translation)
                {
                    _correctAnswers.Add(part.ToLower());      
                }
            }
        }
    }
}
