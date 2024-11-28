using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicDuel
{
    public record Card(int Value, Element Element)
    {
        public bool Wins(Card card)
            => card.Element == Element ? Value > card.Value :
               (Element == Element.Fire && card.Element == Element.Earth)
            || (Element == Element.Earth && card.Element == Element.Water)
            || (Element == Element.Water && card.Element == Element.Fire);
    }
}
