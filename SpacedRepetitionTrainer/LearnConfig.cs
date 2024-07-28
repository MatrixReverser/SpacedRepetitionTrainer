using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpacedRepetitionTrainer
{
    public enum LearnMode
    {
        MIXED,
        CARD,
        MULTIPLE_CHOICE,
        WRITE
    }

    public enum LearnDirection
    {
        MIXED,
        TO_TRANSLATION,
        FROM_TRANSLATION
    }

    internal class LearnConfig
    {
        public int Count { get; set; }
        public LearnMode Mode { get; set; }
        public LearnDirection Direction { get; set; }

        public LearnConfig()
        {
            Count = 0;
            Mode = LearnMode.MIXED;
            Direction = LearnDirection.MIXED;
        }
    }
}
