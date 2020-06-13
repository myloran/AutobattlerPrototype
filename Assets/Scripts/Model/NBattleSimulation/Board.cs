using Model.NUnit;
using Shared;

namespace Model.NBattleSimulation {
  public class Board : BaseBoard<Unit, Player> {
    public Board(IUnitDict<Unit> units, IUnitDict<Unit> player1Units, 
      IUnitDict<Unit> player2Units) : base(units, player1Units, player2Units) { }

    protected override void OnChangeCoord(Coord coord, Unit unit) { }
  }
}