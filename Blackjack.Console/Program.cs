using Blackjack.Console.Models;

Console.WriteLine("Welcome to TwentyOne");

decimal chips = 500m;
const decimal minBet = 5m;
int hands = 0;
int wins = 0;
int losses = 0;
int pushes = 0;
int blackjacks = 0;

while (true)
{
  Console.WriteLine();
  Console.WriteLine($"Chips: {chips}");

  var bet = PromptForBet(chips, minBet);
  var outcome = PlayRound(bet);
  ApplyOutcomeStats(outcome, ref hands, ref wins, ref losses, ref pushes, ref blackjacks);
  
  var delta = PayoutFor(outcome, bet);
  chips += delta;

  Console.WriteLine();
  Console.WriteLine($"Outcome: {outcome}");
  Console.WriteLine(delta >= 0 ? $"+{delta} chips" : $"{delta} chips");
  Console.WriteLine($"Chips now: {chips}");

  if (chips < minBet)
  {
    Console.WriteLine("You're out of chips (or below minimum bet). Game over.");
    break;
  }

  Console.WriteLine();
  if (!PromptPlayAgain())
    break;
}

Console.WriteLine();
Console.WriteLine("Session stats:");
Console.WriteLine($"Hands: {hands}");
Console.WriteLine($"Wins: {wins}");
Console.WriteLine($"Losses: {losses}");
Console.WriteLine($"Pushes: {pushes}");
Console.WriteLine($"Blackjacks: {blackjacks}");

static bool PromptPlayAgain()
{
  while (true)
  {
    Console.Write("Play again? (Y/N): ");
    var s = Console.ReadLine()?.Trim().ToUpperInvariant();

    if (s == "Y") return true;
    if (s == "N") return false;

    Console.WriteLine("Please enter Y or N.");
  }
}

static void RenderTable(Hand player, Hand dealer, decimal bet, bool hideDealerHoleCard)
{
  Console.Clear();
  Console.WriteLine($"Bet: {bet}");
  Console.WriteLine();

  Console.WriteLine("Dealer:");
  Console.WriteLine(hideDealerHoleCard
    ? dealer.ToAsciiStringWithHiddenSecondCard()
    : dealer.ToAsciiString());

  Console.WriteLine($"Dealer total: {(hideDealerHoleCard ? "??" : dealer.GetBestTotal().ToString())}");

  Console.WriteLine();
  Console.WriteLine("Player:");
  Console.WriteLine(player.ToAsciiString());
  Console.WriteLine($"Player total: {player.GetBestTotal()}");
}

static decimal PromptForBet(decimal chips, decimal minBet)
{
  while (true)
  {
    Console.Write($"Bet amount (min {minBet}, max {chips}): ");
    var raw = Console.ReadLine()?.Trim();

    if (string.IsNullOrWhiteSpace(raw))
    {
      continue;
    }

    if (!decimal.TryParse(raw, out var bet))
    {
      Console.WriteLine("Please enter a number.");
      continue;
    }

    if (bet < minBet)
    {
      Console.WriteLine($"Minimum bet is {minBet}.");
      continue;
    }

    if (bet > chips)
    {
      Console.WriteLine("You don't have enough chips for that bet.");
      continue;
    }
    return bet;
  }
}

static decimal PayoutFor(RoundOutcome outcome, decimal bet)
{
  return outcome switch
  {
    RoundOutcome.PlayerWin => bet,
    RoundOutcome.DealerWin => -bet,
    RoundOutcome.Push => 0m,
    RoundOutcome.PlayerBlackjack => bet * 1.5m,
    _ => 0m
  };
}

static void ApplyOutcomeStats(RoundOutcome outcome, ref int hands, 
  ref int wins, ref int losses, ref int pushes, ref int blackjacks)
{
  hands++;

  switch (outcome)
  {
    case RoundOutcome.PlayerWin:
      wins++;
      break;
    case RoundOutcome.DealerWin:
      losses++;
      break;
    case RoundOutcome.Push:
      pushes++;
      break;
    case RoundOutcome.PlayerBlackjack:
      wins++;
      blackjacks++;
      break;
  }
}

static RoundOutcome PlayRound(decimal bet)
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
    RenderTable(playerHand, dealerHand, bet, hideDealerHoleCard: false);
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

  RenderTable(playerHand, dealerHand, bet, hideDealerHoleCard: true);

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
      RenderTable(playerHand, dealerHand, bet, hideDealerHoleCard: true);
      int newTotal = playerHand.GetBestTotal();

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

  RenderTable(playerHand, dealerHand, bet, hideDealerHoleCard: false);
  Console.WriteLine();

  while (dealerHand.GetBestTotal() < 17)
  {
    dealerHand.AddCard(deck.Deal());
    RenderTable(playerHand, dealerHand, bet, hideDealerHoleCard: false);
  }

  int finalPlayerTotal = playerHand.GetBestTotal();
  int dealerTotal = dealerHand.GetBestTotal();

  RenderTable(playerHand, dealerHand, bet, hideDealerHoleCard: false);
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