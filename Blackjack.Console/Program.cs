using Blackjack.Console.Models;

Console.WriteLine("Welcome to TwentyOne");

while (true)
{
  var outcome = PlayRound();

  Console.WriteLine();
  Console.Write("Play again? (Y/N): ");
  var again = Console.ReadLine()?.Trim().ToUpperInvariant();

  if (again != "Y")
  {
    break;
  }

  Console.WriteLine();
}

static RoundOutcome PlayRound()
{
  var deck = new Deck();
  deck.Shuffle();

  var playerHand = new Hand();
  var dealerHand = new Hand();

  playerHand.AddCard(deck.Deal());
  dealerHand.AddCard(deck.Deal());
  playerHand.AddCard(deck.Deal());
  dealerHand.AddCard(deck.Deal());

  bool playerBJ = playerHand.IsBlackjack;
  bool dealerBJ = dealerHand.IsBlackjack;

  if (playerBJ || dealerBJ)
  {
    Console.WriteLine();
    Console.WriteLine("Dealer hand:");
    Console.WriteLine(dealerHand.ToAsciiString());
    Console.WriteLine($"Dealer total: {dealerHand.GetBestTotal()}");
    Console.WriteLine();
    Console.WriteLine("Player hand:");
    Console.WriteLine(playerHand.ToAsciiString());
    Console.WriteLine($"Player total: {playerHand.GetBestTotal()}");
    Console.WriteLine();
    Console.WriteLine("Result:");

    if (playerBJ && dealerBJ)
    {
      Console.WriteLine("Both have blackjack. Push.");
      return RoundOutcome.Push;
    }
    else if (playerBJ)
    {
      Console.WriteLine("Blackjack! Player wins.");
      return RoundOutcome.PlayerBlackjack;
    }
    else
    {
      Console.WriteLine("Dealer has blackjack. Dealer wins.");
      return RoundOutcome.DealerWin;
    }

  }

  Console.WriteLine();
  Console.WriteLine("Dealer shows:");
  Console.WriteLine(dealerHand.ToAsciiStringWithHiddenSecondCard());
  
  Console.WriteLine();
  Console.WriteLine("Player hand:");
  Console.WriteLine(playerHand.ToAsciiString());
  Console.WriteLine($"Total: {playerHand.GetBestTotal()}");

  while (true)
  {
    int playerTotal = playerHand.GetBestTotal();

    if (playerTotal > 21)
    {
      Console.WriteLine("Player busts!");
      return RoundOutcome.DealerWin;
    }
    
    if (playerTotal == 21)
    {
      Console.WriteLine("Player has 21. Standing.");
      break;
    }

    Console.WriteLine();
    Console.Write("Hit, or Stand? (H/S): ");

    var input = Console.ReadLine()?.Trim().ToUpperInvariant();

    if (input == null)
      continue;

    if (input == "H")
    {
      playerHand.AddCard(deck.Deal());
      Console.WriteLine();
      Console.WriteLine("Player draws:");
      Console.WriteLine(playerHand.Cards[^1]);
      
      int newTotal = playerHand.GetBestTotal();
      Console.WriteLine("Player hand:");
      Console.WriteLine(playerHand.ToAsciiString());
      Console.WriteLine($"Total: {newTotal}");

      if (newTotal == 21)
      {
        Console.WriteLine("Player has 21. Standing.");
        break;
      }
    }
    else if (input == "S")
    {
      Console.WriteLine("Player stands.");
      break;
    }
    else
    {
      Console.WriteLine("Please enter H or S.");
    }
  }

  Console.WriteLine();
  Console.WriteLine("Dealer hand (revealed):");
  Console.WriteLine(dealerHand.ToAsciiString());
  Console.WriteLine($"Dealer total: {dealerHand.GetBestTotal()}");

  while (dealerHand.GetBestTotal() < 17)
  {
    dealerHand.AddCard(deck.Deal());
    Console.WriteLine();
    Console.WriteLine("Dealer draws:");
    Console.WriteLine(dealerHand.Cards[^1]);
    Console.WriteLine("Dealer hand:");
    Console.WriteLine(dealerHand.ToAsciiString());
    Console.WriteLine($"Dealer total: {dealerHand.GetBestTotal()}");
  }

  int finalPlayerTotal = playerHand.GetBestTotal();
  int dealerTotal = dealerHand.GetBestTotal();

  Console.WriteLine();
  Console.WriteLine("Result:");

  if (dealerTotal > 21)
  {
    Console.WriteLine("Dealer busts. Player wins!");
    return RoundOutcome.PlayerWin;
  }
  else if (dealerTotal > finalPlayerTotal)
  {
    Console.WriteLine("Dealer wins.");
    return RoundOutcome.DealerWin;
  }
  else if (dealerTotal < finalPlayerTotal)
  {
    Console.WriteLine("Player wins!");
    return RoundOutcome.PlayerWin;
  }
  else
  {
    Console.WriteLine("Push. (tie)");
    return RoundOutcome.Push;
  }
}