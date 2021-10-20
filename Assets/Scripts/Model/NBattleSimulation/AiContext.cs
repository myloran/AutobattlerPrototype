using System.Collections.Generic;
using System.Linq;
using Model.NAI.Commands;
using Model.NUnit.Abstraction;
using Newtonsoft.Json;
using Shared.Addons.Examples.FixMath;
using Shared.Primitives;
using Shared.Shared.Client.Abstraction;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NBattleSimulation {
  public class AiContext : ITime { //TODO: make interface that won't expose reset/checkBattleIsOveer/isBattleOVer 
    public AiContext(Board board, AiHeap heap) {
      this.board = board;
      this.heap = heap;
    }
    
    public bool IsPlayerDead;
    public bool IsBattleOver => IsPlayerDead && CurrentTime > playerDeathTime;

    #region Heap

    public F32 CurrentTime {
      get => heap.CurrentTime;
      set => heap.CurrentTime = value;
    }

    public void InsertCommand(F32 time, ICommand command) => heap.InsertCommand(time, command);
    public (bool IsEmpty, PriorityCommand PriorityCommand) RemoveMin() => heap.RemoveMin();

    #endregion
    #region Board

    public IEnumerable<IUnit> EnemyUnits(EPlayer player) => 
      board.GetPlayerUnits(player.Opposite()).Where(u => u.IsAlive);
    
    public IEnumerable<IUnit> GetSurroundUnits(Coord coord) => board.GetSurroundUnits(coord);
    public IEnumerable<IUnit> GetAdjacentUnits(Coord coord) => board.GetAdjacentUnits(coord);
    public bool IsSurrounded(Coord coord) => board.IsSurrounded(coord);
    public bool IsTileEmpty(Coord coord) => !board.ContainsUnit(coord) && coord.IsInsideBoard();
    public IUnit TryGetUnit(Coord coord) => board.TryGetUnit(coord);
    public void AddUnit(Coord coord, IUnit unit) => board.AddUnit(coord, unit);
    public void RemoveUnit(Coord coord) => board.RemoveUnit(coord);
    public IUnit FindClosestUnitTo(Coord coord, EPlayer player) => board.FindClosestUnitTo(coord, player);
    public IUnit TryFindUnitOnMaxSqrRange(Coord coord, F32 maxRange, EPlayer player) => board.TryFindUnitOnMaxSqrRange(coord, maxRange, player);
    public IEnumerable<IUnit> FindUnitsWithinSqrRange(Coord coord, EPlayer player, F32 sqrRadius) => board.FindUnitsWithinSqrRange(coord, player, sqrRadius);

    #endregion

    public void CheckBattleIsOver() {
      if (board.HasAliveUnits(EPlayer.First) && board.HasAliveUnits(EPlayer.Second)) return;
      
      IsPlayerDead = true;
      playerDeathTime = CurrentTime;
    }

    
    public void Reset() {
      IsPlayerDead = false;
      CurrentTime = Zero;
      playerDeathTime = MaxValue;
      CheckBattleIsOver();
    }
    
    [JsonProperty] readonly Board board;
    [JsonProperty] readonly AiHeap heap;
    F32 playerDeathTime;

    public IEnumerable<IUnit> GetUnits(IEnumerable<Coord> coords, EPlayer player) => board.GetUnits(coords, player);
  }
}