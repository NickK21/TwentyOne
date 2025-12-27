using System.Linq;
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

  public string ToAsciiString()
  {
    if (_cards.Count == 0)
      return "(empty hand)";

    var cardLines = _cards.Select(c => c.ToAsciiLines()).ToList();
    int height = cardLines[0].Length;

    var lines = new List<string>(height);

    for (int row = 0; row < height; row++)
    {
      lines.Add(string.Join(" ", cardLines.Select(c => c[row])));
    }

    return string.Join(Environment.NewLine, lines);
  }

  public string ToAsciiStringWithHiddenSecondCard()
  {
    if (_cards.Count == 0)
      return "(empty hand)";

    var first = _cards[0].ToAsciiLines();

    var hidden = new[]
    {
      "┌─────────┐",
      "│░░░░░░░░░│",
      "│░░░░░░░░░│",
      "│░░░░░░░░░│",
      "│░░░░░░░░░│",
      "│░░░░░░░░░│",
      "└─────────┘"
    };

    var lines = new List<string>(first.Length);
    for (int i = 0; i < first.Length; i++)
      lines.Add($"{first[i]} {hidden[i]}");

    return string.Join(Environment.NewLine, lines);
  }
}