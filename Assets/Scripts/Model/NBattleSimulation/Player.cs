using Model.NUnit;
using Shared;
using Shared.Abstraction;

namespace Model.NBattleSimulation {
  public class Player : BasePlayer<Unit> {
    public Player(EPlayer player, IUnitDict<Unit> boardUnitDict, IUnitDict<Unit> benchUnitDict)
      : base(player, boardUnitDict, benchUnitDict) { }

    protected override void OnChangeCoord(Coord coord, Unit unit) => 
      unit.StartingCoord = coord;

    public (bool, Unit) GetUnit(Coord coord) {
      var dict = coord.BelongsToPlayer(Player) ? BenchUnits : BoardUnits;
      
      return dict.Has(coord) 
        ? (true, dict[coord]) 
        : (false, default);
    }
  }
}