namespace Blackjack.Console.Models;

public class Hand
{
  private readonly List<Card> _cards = new();

  public IReadOnlyList<Card> Cards => _cards;

public bool IsBlackjack =>
  _cards.Count == 2 && GetBestTotal() == 21;

  public void AddCard(Card card)
  {
    _cards.Add(card);
  }

  public int GetBestTotal()
  {
    int total = 0;
    int aceCount = 0;

    foreach (var card in _cards)
    {
      switch (card.Rank)
      {
        case Rank.Ace:
          aceCount++;
          total += 11;
          break;
        case Rank.King:
        case Rank.Queen:
        case Rank.Jack:
          total += 10;
          break;
        default:
          total += (int)card.Rank;
          break;
      }
    }

    while (total > 21 && aceCount > 0)
    {
      total -= 10;
      aceCount--;
    }

    return total;
  }

  public override string ToString()
  {
    return string.Join(", ", _cards);
  }
}