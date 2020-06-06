using Model.NUnit;
using Shared;

namespace Model.NBattleSimulation {
  public class Player : BasePlayer<Unit> {
    public Player(IUnitDict<Unit> boardUnitDict, IUnitDict<Unit> benchUnitDict) : 
      base(boardUnitDict, benchUnitDict) { }

    protected override void OnChangeCoord(Coord coord, Unit unit) => 
      unit.Movement.StartingCoord = coord;
  }
}