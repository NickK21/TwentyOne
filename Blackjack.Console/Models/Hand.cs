namespace Blackjack.Console.Models;

public class Hand
{
  private readonly List<Card> _cards = new();

  public IReadOnlyList<Card> Cards => _cards;

  public void AddCard(Card card)
  {
    _cards.Add(card);
  }

  public override string ToString()
  {
    return string.Join(", ", _cards);
  }
}