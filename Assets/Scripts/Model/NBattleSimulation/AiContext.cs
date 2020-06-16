using System.Collections.Generic;
using System.Linq;
using FibonacciHeap;
using Model.NBattleSimulation.Commands;
using Model.NUnit;
using PlasticFloor.EventBus;
using Shared;
using Shared.OkwyLogging;
using Shared.Shared.Client;

namespace Model.NBattleSimulation {
  //TODO: extract responsibility related to battle simulation
  public class AiContext : ITime {
    public bool IsPlayerDead { get; private set; }
    public float PlayerDeathTime { get; private set; }
    public TimePoint CurrentTime { get; set; }
    public bool IsCyclicDecision;

    public readonly Board Board;
    
    public AiContext(Board board) => Board = board;

    public void InsertCommand(float time, ICommand command) {
      var nextTime = CurrentTime + time;
      
      if (nodes.ContainsKey(nextTime)) {
        var existingNode = nodes[nextTime];
        var existingCommand = existingNode.Data;
        existingNode.Data = new CompositeCommand(existingCommand, command);
        return;
      }

      var node = new FibonacciHeapNode<ICommand, TimePoint>(command, nextTime);
      aiHeap.Insert(node);
      nodes[nextTime] = node;
    }

    public (bool, ICommand) RemoveMin() {
      var node = aiHeap.RemoveMin();
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
    
    public Unit FindNearestTarget(EPlayer player, Coord coord) {
      var units = EnemyUnits(player); //TODO: move to target component?
      
      return !units.Any() 
        ? default 
        : units.MinBy(u => CoordExt.SqrDistance(coord, u.Movement.Coord));
    } 

    IEnumerable<Unit> EnemyUnits(EPlayer player) => 
      Board.GetPlayerUnits(player.Opposite());
    
    public IEnumerable<Unit> GetSurroundUnits(Coord coord) => Board.GetSurroundUnits(coord);
    public bool IsSurrounded(Coord coord) => Board.IsSurrounded(coord);
    public bool IsTileEmpty(Coord coord) => !Board.ContainsUnitAt(coord) && coord.IsInsideBoard();

    public void CheckBattleIsOver() {
      if (!Board.HasUnits(EPlayer.First) && !Board.HasUnits(EPlayer.Second)) return;
      
      IsPlayerDead = true;
      PlayerDeathTime = CurrentTime;
    }
    
    public void Reset(Player player1, Player player2) {
      Board.Reset(player1, player2);
      IsPlayerDead = false;
      CurrentTime = 0;
      CheckBattleIsOver();
      
      foreach (var unit in Board.Values) {
        unit.Reset();
        var decisionCommand = new MakeDecisionCommand(unit.Ai, this, 0);
        InsertCommand(0, decisionCommand);
      }
    }

    readonly FibonacciHeap<ICommand, TimePoint> aiHeap = 
      new FibonacciHeap<ICommand, TimePoint>(float.MinValue);
    
    readonly Dictionary<TimePoint, FibonacciHeapNode<ICommand, TimePoint>> nodes = 
      new Dictionary<TimePoint, FibonacciHeapNode<ICommand, TimePoint>>();

    static readonly Logger log = MainLog.GetLogger(nameof(AiContext));

    public IEnumerable<Unit> GetAdjacentUnits(Coord coord) => Board.GetAdjacentUnits(coord);
  }
}