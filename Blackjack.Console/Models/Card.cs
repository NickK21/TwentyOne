namespace Blackjack.Console.Models;

public class Card
{
  public Rank Rank { get; }  
  public Suit Suit { get; }  
  public Card( Rank rank, Suit suit)
  {
    Rank = rank;
    Suit = suit;
  }

  public override string ToString()
  {
    return $"{Rank} of {Suit}";
  }

  public string[] ToAsciiLines()
  {
    string rank = RankToLabel(Rank);
    string suit = SuitToSymbol(Suit);

    string left = rank.PadRight(2);
    string right = rank.PadLeft(2);

    return new[]
    {
      "┌─────────┐",
      $"│{left}       │",
      "│         │",
      $"│    {suit}    │",
      "│         │",
      $"│       {right}│",
      "└─────────┘"
    };
  }

  private static string RankToLabel(Rank rank) =>
    rank switch
    {
      Rank.Ace => "A",
      Rank.King => "K",
      Rank.Queen => "Q",
      Rank.Jack => "J",
      Rank.Ten => "10",
      _ => ((int)rank).ToString()
    };

  private static string SuitToSymbol(Suit suit) =>
    suit switch
    {
      Suit.Spades => "♠",
      Suit.Hearts => "♥",
      Suit.Diamonds => "♦",
      Suit.Clubs => "♣",
      _ => "?"
    };
}