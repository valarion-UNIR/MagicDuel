using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicDuel
{
    public static class DeckExtensions
    {
        private static readonly Card[] _deck;

        static DeckExtensions()
            => _deck = Enum.GetValues<Element>().SelectMany(element => Enumerable.Range(1, 10).Select(value => new Card(value, element))).ToArray();

        public static List<Card> GetDeck()
            => [.. _deck];

        public static List<T> Randomize<T>(this IEnumerable<T> list, Random? random = null)
        {
            random ??= new Random();

            var left = list.ToList();
            var result = new List<T>();

            while (left.Count > 0)
            {
                var index = random.Next(left.Count);
                result.Add(left[index]);
                left.RemoveAt(index);
            }

            return result;
        }
    }
}
