using System.Globalization;
using Blackjack.Console.Models;

Console.WriteLine("Welcome to Blackjack");

var deck = new Deck();
deck.Shuffle();

Console.WriteLine($"Deck count: {deck.Count}");

for (int i = 0; i < 5; i++)
{
  var card = deck.Deal();
  Console.WriteLine(card);
}

Console.WriteLine($"Deck count after dealing: {deck.Count}");