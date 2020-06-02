using System.Collections.Generic;
using FibonacciHeap;
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

    public void InsertCommand(ICommand command, float time = 0) => 
      AiHeap[CurrentTime + time] = command;

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
      CheckBattleIsOver();
      
      foreach (var unit in Board.GetUnits()) {
        unit.Reset();
        var decisionCommand = new MakeDecisionCommand(unit.Ai, this, 0);
        AiHeap[0] = decisionCommand;
      }
    }
  }
}