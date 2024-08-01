using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaktionslogik für LearnMultipleChoicePanel.xaml
    /// </summary>
    public partial class LearnMultipleChoicePanel : UserControl
    {
        public event EventHandler<bool> QuestionDone;

        private List<Word> _learnSet;
        private List<Word> _failedSet;
        private Word[] _unmutableArray;
        private LearnDirection _learnDirection;
        private string _correctAnswer;

        public LearnMultipleChoicePanel(List<Word> learnSet, LearnDirection direction, List<Word> failedSet, ObservableCollection<Word>allWords)
        {
            InitializeComponent();

            _failedSet = failedSet;
            _learnSet = learnSet;

            _unmutableArray = new Word[allWords.Count];
            for (int i=0; i<allWords.Count; i++)
            {
                _unmutableArray[i] = allWords[i];
            }

            _learnDirection = direction;

            TermLabel.Text = GetQuestion();

            List<string> answers = GetAnswers();

            foreach (string answer in answers)
            {
                TextBlock uiAnswer = new TextBlock();
                uiAnswer.Text = answer;
                uiAnswer.FontSize = 18;
                uiAnswer.HorizontalAlignment = HorizontalAlignment.Center;
                uiAnswer.Foreground = new SolidColorBrush(Colors.Orange);
                uiAnswer.Margin = new Thickness(0, 0, 0, 25);
                uiAnswer.MouseLeftButtonDown += OnClickAnswer;

                ChoicePanel.Children.Add(uiAnswer);
            }
        }

        /**
         * Is called if the user click on an answer
         */
        private void OnClickAnswer(object sender, MouseButtonEventArgs e)
        {
            string answer = ((TextBlock)sender).Text;
            
            if (answer == _correctAnswer)
            {
                MarkSucceeded();
                MessageBox.Show("Success");
            } else
            {
                MarkFailed();
                MessageBox.Show("Fail");
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

        private List<string> GetAnswers()
        {
            List<Word> result = new List<Word>();
            Random rand = new Random();

            result.Add(_learnSet[0]);
       
            for (int i = 0; i < 8; i++)
            {
                Word randomWord = _unmutableArray[rand.Next(0, _unmutableArray.Length)];
                if (randomWord != _learnSet[0])
                {
                    result.Add(randomWord);
                }
            }

            for (int i=0; i<result.Count; i++)
            {
                int k = rand.Next(0, result.Count);
                Word w = result[k];
                result[k] = result[i];
                result[i] = w;
            }

            List<string> stringResult = new List<string>();

            if (_learnDirection == LearnDirection.TO_TRANSLATION)
            {
                foreach (Word word in result)
                {
                    stringResult.Add(word.Term);

                    if (word == _learnSet[0])
                    {
                        _correctAnswer = word.Term;
                    }
                }
            }
            else
            {
                foreach (Word word in result)
                {
                    string completeAnswer = string.Empty;
                    
                    foreach (string str in word.Translation)
                    {
                        if (completeAnswer != string.Empty)
                        {
                            completeAnswer += ", ";
                        }
                        completeAnswer += str;
                    }

                    stringResult.Add(completeAnswer);

                    if (word == _learnSet[0])
                    {
                        _correctAnswer = completeAnswer;
                    }
                }
            }

            return stringResult;
        }
    }
}
