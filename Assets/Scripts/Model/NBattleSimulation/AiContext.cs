using System.Collections.Generic;
using FibonacciHeap;
using MinMaxHeap;
using Model.NBattleSimulation.Commands;
using Model.NUnit;
using Shared;

namespace Model.NBattleSimulation {
  public class AiContext {
    public bool IsPlayerDead { get; private set; }
    public float PlayerDeathTime { get; private set; }
    public TimePoint CurrentTime;

    public Board Board;
    public readonly FibonacciHeap<ICommand, TimePoint> AiHeap;
    
    public AiContext(Board board, FibonacciHeap<ICommand, TimePoint> aiHeap) {
      Board = board;
      AiHeap = aiHeap;
    }

    public void InsertCommand(ICommand command, float time = 0) {
      var nextTime = CurrentTime + time;
      
      if (nodes.ContainsKey(nextTime)) {
        var existingNode = nodes[nextTime];
        var existingCommand = existingNode.Data;
        existingNode.Data = new CompositeCommand(existingCommand, command);
        return;
      }

      var node = new FibonacciHeapNode<ICommand, TimePoint>(command, nextTime);
      AiHeap.Insert(node);
      nodes[nextTime] = node;
    }

    public (bool, ICommand) RemoveMin() {
      var node = AiHeap.RemoveMin();
      if (node == null) {
        log.Info("The battle is over");
        return (true, default);
      }
      var time = node.Key;
      
      if (IsPlayerDead && time > PlayerDeathTime) {
        return (true, default);
      }
      
      var command = node.Data;
      CurrentTime = time;
      nodes.Remove(CurrentTime);
      return (false, command);
    }

    public IEnumerable<Unit> EnemyUnits(EPlayer player) =>
      Board.GetPlayerUnits(player.Opposite());

    public bool IsTileEmpty(Coord coord) => !Board.ContainsUnitAt(coord);

    public void CheckBattleIsOver() {
      if (Board.HasUnits(EPlayer.First) || Board.HasUnits(EPlayer.Second)) {
        IsPlayerDead = true;
        PlayerDeathTime = CurrentTime;
      }
    }
    
    public void Reset(Player player1, Player player2) {
      Board.Reset(player1, player2);
      IsPlayerDead = false;
      CurrentTime = 0;
      CheckBattleIsOver();
      
      foreach (var unit in Board.Units) {
        unit.Reset();
        var decisionCommand = new MakeDecisionCommand(unit.Ai, this, 0);
        InsertCommand(decisionCommand);
      }
    }
        
    Dictionary<TimePoint, FibonacciHeapNode<ICommand, TimePoint>> nodes = 
      new Dictionary<TimePoint, FibonacciHeapNode<ICommand, TimePoint>>();

    static readonly Okwy.Logging.Logger log = Okwy.Logging.MainLog.GetLogger(nameof(AiContext));
  }
}