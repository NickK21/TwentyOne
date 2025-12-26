namespace Blackjack.Console.Models;

public class Deck
{
  private readonly List<Card> _cards = new();
  public int Count => _cards.Count;

  public Deck()
  {
    foreach (Suit suit in Enum.GetValues<Suit>())
    {
      foreach (Rank rank in Enum.GetValues<Rank>())
      {
        _cards.Add(new Card(rank, suit));
      } 
    }
  }

  public void Shuffle()
  {
    var rng = Random.Shared;

    for (int i = _cards.Count - 1; i > 0; i--)
    {
      int j = rng.Next(i + 1);
      (_cards[i], _cards[j]) = (_cards[j], _cards[i]);
    }
  }

  public Card Deal()
  {
    if (_cards.Count == 0)
    {
      throw new InvalidOperationException("Cannot deal from an empty deck.");
    }

    var topIndex = _cards.Count - 1;
    var card = _cards[topIndex];
    _cards.RemoveAt(topIndex);
    return card;
  }
}