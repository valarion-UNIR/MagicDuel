// See https://aka.ms/new-console-template for more information
using MagicDuel;

int player1Wins = 0, player2Wins = 0;

var tries = 100000;
List<int> turncount = new(tries);
for(var i= 0; i < tries; i++)
{
    var random = new Random(i * tries);

    var game = new Game(
         5,
         6,
         new Player(random, Element.Fire),
         new Player(random, Element.Earth),
         random,
         1,
         2,
         1
        );

    while (!game.PlayTurn()) ;

    turncount.Add(game.TurnsPlayed);
    if (game.BoardPosition > 0)
        player1Wins++;
    else
        player2Wins++;

    Console.WriteLine("------------------------------------------------------");
    Console.WriteLine($"Player1 ({player1Wins}) - ({player2Wins}) Player2");
    Console.WriteLine("------------------------------------------------------");
}

var min = turncount.Min();
var max = turncount.Max();
var mean = turncount.Average();
var median = turncount[tries / 2];
double timesOver(double threshold) => turncount.Count(t => t > threshold) / (double)tries * 100;
Console.WriteLine("------------------------------------------------------");
Console.WriteLine("Final result:");
Console.WriteLine($"Player1 ({player1Wins}) - ({player2Wins}) Player2 | ElementAdvantage: {Math.Abs((player1Wins/(double)tries-0.5)*100) : 0.00}%");
Console.WriteLine("Turns analysis:");
Console.Write($"Min: {min}, Max: {max}, Mean: {mean: 0.00}, Median: {median}, >Mean: {timesOver(mean): 0.00}%");
foreach(var value in (int[])[15, 20, 25, 30, 35])
    Console.Write($", >{value}: {timesOver(value): 0.00}%");
Console.WriteLine();
Console.WriteLine("------------------------------------------------------");
