using System.Collections.Generic;
using System.Linq;
using FibonacciHeap;
using FixMath;
using Model.NBattleSimulation.Commands;
using Model.NUnit;
using PlasticFloor.EventBus;
using Shared;
using Shared.OkwyLogging;
using Shared.Shared.Client;
using static FixMath.F32;

namespace Model.NBattleSimulation {
  //TODO: extract responsibility related to battle simulation
  public class AiContext : ITime {
    public bool IsPlayerDead { get; private set; }
    public F32 PlayerDeathTime { get; private set; }
    public F32 CurrentTime { get; set; }
    public bool IsCyclicDecision;

    public readonly Board Board;
    
    public AiContext(Board board) => Board = board;

    public void InsertCommand(F32 time, params ICommand[] commands) {
      var nextTime = CurrentTime + time;

      foreach (var command in commands) {
        InsertCommand(command);
      }

      void InsertCommand(ICommand command) {
        if (nodes.ContainsKey(nextTime)) {
          var existingNode = nodes[nextTime];
          var existingCommand = existingNode.Data;
          existingNode.Data = new CompositeCommand(existingCommand, command);
          return;
        }

        var node = new FibonacciHeapNode<ICommand, F32>(command, nextTime);
        aiHeap.Insert(node);
        nodes[nextTime] = node;
      }
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
    
    public (bool, Unit) FindNearestTarget(EPlayer player, Coord coord) {
      var units = EnemyUnits(player); //TODO: move to target component?
      
      return !units.Any() 
        ? (false, default) 
        : (true, units.MinBy(u => CoordExt.SqrDistance(coord, u.Coord)));
    } 

    IEnumerable<Unit> EnemyUnits(EPlayer player) => 
      Board.GetPlayerUnits(player.Opposite()).Where(u => u.IsAlive);
    
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
      CurrentTime = Zero;
      CheckBattleIsOver();
      aiHeap.Clear();
      nodes.Clear();
      
      foreach (var unit in Board.Values) {
        unit.Reset();
        InsertCommand(Zero, new MakeDecisionCommand(unit.Ai, this, Zero));
      }
    }

    readonly FibonacciHeap<ICommand, F32> aiHeap = 
      new FibonacciHeap<ICommand, F32>(MinValue);
    
    readonly Dictionary<F32, FibonacciHeapNode<ICommand, F32>> nodes = 
      new Dictionary<F32, FibonacciHeapNode<ICommand, F32>>();

    static readonly Logger log = MainLog.GetLogger(nameof(AiContext));

    public IEnumerable<Unit> GetAdjacentUnits(Coord coord) => Board.GetAdjacentUnits(coord);
  }
}