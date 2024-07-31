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
        private List<Word> _learnSet;
        private List<Word> _failedSet;
        private int _learnCount;

        public SessionPanel(VocabularySet vocabularySet, LearnConfig config)
        {
            InitializeComponent();

            _learnCount = 0;
            _vocabularySet = vocabularySet;
            _learnConfig = config;
            _learnSet = BuildLearnSet();
            _failedSet = new List<Word>();
            ShuffleLearnSet();

            int maxWordsToLearn = _learnSet.Count - 1;
            if (_learnConfig.Count < _learnSet.Count)
            {
                maxWordsToLearn = _learnConfig.Count - 1;
            }

            barProgress.Maximum = maxWordsToLearn;
            barProgress.Value = 0;

            barSucceded.Maximum = maxWordsToLearn + 1;
            barSucceded.Value = 0;

            barFailed.Maximum = maxWordsToLearn + 1;
            barFailed.Value = 0;

            ShowNextWord();
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

        /**
         * Creates a list of vocabularies that should be learned in this session
         */
        private List<Word> BuildLearnSet()
        {
            List<Word> result = new List<Word>();

            foreach (Word word in _vocabularySet.Words)
            {
                DateTime wordTime = DateTimeOffset.FromUnixTimeSeconds(word.Timestamp).DateTime;
                DateTime currentDateTime = DateTime.UtcNow;
                TimeSpan difference = currentDateTime - wordTime;
                int timeDiff = (int)difference.TotalDays;

                int repetitionInterval = word.GetRepetitionIntervall();

                if (repetitionInterval <= timeDiff)
                {
                    result.Add(word);
                }
            }

            return result;
        }

        /**
         * Shuffles all words in the learn set
         */
        private void ShuffleLearnSet()
        {
            Random rng = new Random();
            int n = _learnSet.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Word value = _learnSet[k];
                _learnSet[k] = _learnSet[n];
                _learnSet[n] = value;
            }
        }

        /**
         * Displays the next word or the end screen if no more words are in the learn set or if the maximum number
         * of words to learn (for this session) has been reached
         */
        private void ShowNextWord()
        {
            barProgress.Value = _learnCount;
            _learnCount++;

            if (_learnSet.Count == 0 || _learnCount > _learnConfig.Count)
            {
                ShowEndScreen();
                return;
            } 

            // get the learn direction
            LearnDirection direction = _learnConfig.Direction;
            if (direction == LearnDirection.MIXED)
            {
                Random rand = new Random();
                if (rand.Next(0, 2) == 0)
                {
                    direction = LearnDirection.TO_TRANSLATION;
                }
                else
                {
                    direction = LearnDirection.FROM_TRANSLATION;
                }
            }

            // get the learn mode
            LearnMode mode = _learnConfig.Mode;
            if (mode == LearnMode.MIXED)
            {
                Random rand = new Random();
                int randMode = rand.Next(0, 3);

                if (randMode == 0)
                {
                    mode = LearnMode.CARD;
                } else if (randMode == 1)
                {
                    mode = LearnMode.MULTIPLE_CHOICE;
                }else
                {
                    mode = LearnMode.WRITE;
                }
            }

            UserControl? subPanel = null;

            switch (mode)
            {
                case LearnMode.CARD:
                    subPanel = new LearnCardPanel(_learnSet, direction, _failedSet);
                    ((LearnCardPanel)subPanel).QuestionDone += QuestionDone;
                    break;
                case LearnMode.MULTIPLE_CHOICE:
                    break;
                case LearnMode.WRITE:
                    break;
            }

            if (subPanel != null)
            {
                SubContent.Child = subPanel;
            } else
            {
                ShowEndScreen();
            }
        }

        /**
         * Is called if a question has been answered
         */
        private void QuestionDone(object? sender, bool e)
        {
            // correct or worng answer?
            if (e)
            {
                barSucceded.Value++;
            } else
            {
                barFailed.Value++;
            }

            ShowNextWord();
        }

        /**
         * Shows the end screen
         */
        private void ShowEndScreen()
        {
            SubTitle.Text = "Du bist fertig für heute! :-)";
            CancelButton.Text = "Zurück";

            _vocabularySet.Save();
        }
    }
}
