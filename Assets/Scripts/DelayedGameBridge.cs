using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class DelayedGameBridge
{
  public static DelayedGameBridge instance = new();
  public GameOnPlaced onPlaced = new();
  public GameOnKilled onKilled = new();
  public GameOnTurnBegin onTurnBegin = new();
  public GameOnTurnEnd onTurnEnd = new();
  public GameOnDamageTaken onDamageTaken = new();
  public GameOnAttack onAttack = new();
  public GameOnCardUpdate onCardUpdate = new();
  public GameOnPlayerDrawCard onPlayerDrawCard = new();


  private readonly List<Action> queue = new();

  private async void Dequeue()
  {
    while (true)
    {
      var temp = queue.ToList();
      queue.Clear();
      foreach (var action in temp)
      {
        action();
        await Task.Delay(100);
      }
      await Task.Delay(1000);
    }
  }

  private void AddToQueue(Action action)
  {
    queue.Add(action);
  }

  public DelayedGameBridge()
  {
    GameBridge.instance.onPlaced.AddListener((GameCard card) =>
    {
      AddToQueue(() => onPlaced.Invoke(card));
    });
    GameBridge.instance.onKilled.AddListener((GameCard card) =>
    {
      AddToQueue(() => onKilled.Invoke(card));
    });
    GameBridge.instance.onTurnBegin.AddListener((int playerId) =>
    {
      AddToQueue(() => onTurnBegin.Invoke(playerId));
    });
    GameBridge.instance.onTurnEnd.AddListener((int playerId) =>
    {
      AddToQueue(() => onTurnEnd.Invoke(playerId));
    });
    GameBridge.instance.onDamageTaken.AddListener((GameCard card) =>
    {
      AddToQueue(() => onDamageTaken.Invoke(card));
    });
    GameBridge.instance.onAttack.AddListener((GameCard card) =>
    {
      AddToQueue(() => onAttack.Invoke(card));
    });
    GameBridge.instance.onCardUpdate.AddListener((GameCard card) =>
    {
      AddToQueue(() => onCardUpdate.Invoke(card));
    });
    GameBridge.instance.onPlayerDrawCard.AddListener((GameCard card) =>
    {
      AddToQueue(() => onPlayerDrawCard.Invoke(card));
    });
    Dequeue();
  }
}