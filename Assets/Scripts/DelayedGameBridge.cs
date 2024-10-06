using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Events;

public class DelayedAction
{
  public Action action;
  public int ms;
}

public class GameOnAll : UnityEvent { }

public class DelayedGameBridge
{
  public static DelayedGameBridge instance;
  public GameOnPlaced onPlaced = new();
  public GameOnKilled onKilled = new();
  public GameOnTurnBegin onTurnBegin = new();
  public GameOnTurnEnd onTurnEnd = new();
  public GameOnCardStatChange onCardStatChange = new();
  public GameOnAttack onAttack = new();
  public GameOnPlayerDrawCard onPlayerDrawCard = new();
  public GameOnHeroAttack onHeroAttack = new();
  public GameOnHeroStatChange onHeroStatChange = new();
  public GameOnAll onAll = new();


  private readonly List<DelayedAction> queue = new();

  private async void Dequeue()
  {
    while (true)
    {
      var temp = queue.ToList();
      queue.Clear();
      foreach (var action in temp)
      {
        action.action();
        await Task.Delay(action.ms);
      }
      await Task.Delay(50);
    }
  }

  private void AddToQueue(Action action, int ms)
  {
    queue.Add(new DelayedAction
    {
      action = action,
      ms = ms,
    });
  }

  private void Bind<T>(UnityEvent<T> source, UnityEvent<T> to, int ms)
  {
    source.AddListener((T arg) => AddToQueue(() =>
    {
      to.Invoke(arg);
      onAll.Invoke();
    }, ms));
  }

  private void Bind2<T, U>(UnityEvent<T, U> source, UnityEvent<T, U> to, int ms)
  {
    source.AddListener((T arg, U arg1) => AddToQueue(() =>
    {
      to.Invoke(arg, arg1);
      onAll.Invoke();
    }, ms));
  }

  public DelayedGameBridge()
  {
    Bind(GameBridge.instance.onPlaced, onPlaced, 100);
    Bind2(GameBridge.instance.onKilled, onKilled, 100);
    Bind(GameBridge.instance.onTurnBegin, onTurnBegin, 200);
    Bind(GameBridge.instance.onTurnEnd, onTurnEnd, 100);
    Bind2(GameBridge.instance.onCardStatChange, onCardStatChange, 100);
    Bind2(GameBridge.instance.onAttack, onAttack, 500);
    Bind(GameBridge.instance.onPlayerDrawCard, onPlayerDrawCard, 100);
    Bind2(GameBridge.instance.onHeroAttack, onHeroAttack, 500);
    Bind(GameBridge.instance.onHeroStatChange, onHeroStatChange, 100);
    Dequeue();
  }

  public static void Initialize()
  {
    instance = new();
  }

  public void Clear()
  {
    queue.Clear();
  }
}