using Blackjack.Console.Models;

Console.WriteLine("Welcome to Blackjack");

var card1 = new Card(Rank.Ace, Suit.Spades);
var card2 = new Card(Rank.Ten, Suit.Hearts);

Console.WriteLine(card1);
Console.WriteLine(card2);