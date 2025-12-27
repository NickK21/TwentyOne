using Blackjack.Console.Models;

Console.WriteLine("Welcome to Blackjack");

var deck = new Deck();
deck.Shuffle();

var hand = new Hand();

hand.AddCard(deck.Deal());
hand.AddCard(deck.Deal());

Console.WriteLine("Player hand:");
Console.WriteLine(hand);

Console.WriteLine($"Total: {hand.GetBestTotal()}");

Console.WriteLine($"Deck count after dealing: {deck.Count}");