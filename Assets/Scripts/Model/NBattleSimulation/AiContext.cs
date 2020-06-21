using System.Collections.Generic;
using System.Linq;
using Model.NAI.Commands;
using Model.NUnit;
using Model.NUnit.Abstraction;
using Shared;
using Shared.Addons.Examples.FixMath;
using Shared.Poco;
using Shared.Shared.Client;
using Shared.Shared.Client.Abstraction;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NBattleSimulation {
  public class AiContext : ITime { //TODO: make interface that won't expose reset/checkBattleIsOveer/isBattleOVer 
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

    public IEnumerable<IUnit> EnemyUnits(EPlayer player) => 
      board.GetPlayerUnits(player.Opposite()).Where(u => u.IsAlive);
    
    public IEnumerable<IUnit> GetSurroundUnits(Coord coord) => board.GetSurroundUnits(coord);
    public IEnumerable<IUnit> GetAdjacentUnits(Coord coord) => board.GetAdjacentUnits(coord);
    public bool IsSurrounded(Coord coord) => board.IsSurrounded(coord);
    public bool IsTileEmpty(Coord coord) => !board.ContainsUnit(coord) && coord.IsInsideBoard();
    public void AddUnit(Coord coord, IUnit unit) => board.AddUnit(coord, unit);
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
    }
    
    readonly Board board;
    readonly AiHeap heap;
    bool isPlayerDead;
    F32 playerDeathTime;
  }
}