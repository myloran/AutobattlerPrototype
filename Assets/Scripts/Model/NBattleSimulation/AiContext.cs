using System.Collections.Generic;
using System.Linq;
using FibonacciHeap;
using FixMath;
using Model.NBattleSimulation.Commands;
using Model.NUnit;
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

    
    public AiContext(Board board) => this.board = board;

    public void InsertCommand(F32 time, ICommand command) {
      var nextTime = CurrentTime + time;

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
      board.GetPlayerUnits(player.Opposite()).Where(u => u.IsAlive);
    
    public IEnumerable<Unit> GetSurroundUnits(Coord coord) => board.GetSurroundUnits(coord);
    public IEnumerable<Unit> GetAdjacentUnits(Coord coord) => board.GetAdjacentUnits(coord);
    public bool IsSurrounded(Coord coord) => board.IsSurrounded(coord);
    public bool IsTileEmpty(Coord coord) => !board.ContainsUnitAt(coord) && coord.IsInsideBoard();
    public void AddUnit(Coord coord, Unit unit) => board.AddUnit(coord, unit);
    public void RemoveUnit(Coord coord) => board.RemoveUnit(coord);

    public void CheckBattleIsOver() {
      if (!board.HasUnits(EPlayer.First) && !board.HasUnits(EPlayer.Second)) return;
      
      IsPlayerDead = true;
      PlayerDeathTime = CurrentTime;
    }
    
    public void Reset() {
      IsPlayerDead = false;
      CurrentTime = Zero;
      CheckBattleIsOver();
      aiHeap.Clear();
      nodes.Clear();
    }

    readonly FibonacciHeap<ICommand, F32> aiHeap = 
      new FibonacciHeap<ICommand, F32>(MinValue);
    
    readonly Dictionary<F32, FibonacciHeapNode<ICommand, F32>> nodes = 
      new Dictionary<F32, FibonacciHeapNode<ICommand, F32>>();
    
    readonly Board board;
    static readonly Logger log = MainLog.GetLogger(nameof(AiContext));
  }
}