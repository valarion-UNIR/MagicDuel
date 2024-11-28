using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicDuel
{
    [Flags]
    public enum Modifiers
    {
        None = 0,
        PlusOneWhenWin = 1 << 0,
        MinusOneWhenLose = 1 << 1,
        All = PlusOneWhenWin | MinusOneWhenLose,
    }
}
