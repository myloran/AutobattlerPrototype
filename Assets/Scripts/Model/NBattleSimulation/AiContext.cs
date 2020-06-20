using System.Collections.Generic;
using System.Linq;
using FixMath;
using Model.NBattleSimulation.Commands;
using Model.NUnit;
using Shared;
using Shared.Shared.Client;
using static FixMath.F32;

namespace Model.NBattleSimulation {
  public class AiContext : ITime {
    public bool IsCyclicDecision;
    
    public AiContext(Board board, AiHeap heap) {
      this.board = board;
      this.heap = heap;
    }
    
    public bool IsBattleOver => isPlayerDead && CurrentTime > playerDeathTime;

    #region Heap

    public F32 CurrentTime {
      get => heap.CurrentTime;
      set => heap.CurrentTime = value;
    }

    public void InsertCommand(F32 time, ICommand command) => heap.InsertCommand(time, command);
    public (bool, ICommand) RemoveMin() => heap.RemoveMin();

    #endregion
    #region Board

    public IEnumerable<Unit> EnemyUnits(EPlayer player) => 
      board.GetPlayerUnits(player.Opposite()).Where(u => u.IsAlive);
    
    public IEnumerable<Unit> GetSurroundUnits(Coord coord) => board.GetSurroundUnits(coord);
    public IEnumerable<Unit> GetAdjacentUnits(Coord coord) => board.GetAdjacentUnits(coord);
    public bool IsSurrounded(Coord coord) => board.IsSurrounded(coord);
    public bool IsTileEmpty(Coord coord) => !board.ContainsUnitAt(coord) && coord.IsInsideBoard();
    public void AddUnit(Coord coord, Unit unit) => board.AddUnit(coord, unit);
    public void RemoveUnit(Coord coord) => board.RemoveUnit(coord);

    #endregion

    public void CheckBattleIsOver() {
      if (board.HasUnits(EPlayer.First) && board.HasUnits(EPlayer.Second)) return;
      
      isPlayerDead = true;
      playerDeathTime = CurrentTime;
    }
    
    public void Reset() {
      isPlayerDead = false;
      CurrentTime = Zero;
      playerDeathTime = MaxValue;
      CheckBattleIsOver();
      heap.Reset();
    }
    
    readonly Board board;
    readonly AiHeap heap;
    bool isPlayerDead;
    F32 playerDeathTime;
  }
}