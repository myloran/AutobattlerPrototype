using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using Shared.Primitives;
using static Shared.Primitives.CoordExt;

namespace Model.NAI {
  public class MoveFinder {
    public MoveInfo MoveInfo;

    public bool Find(IUnit unit, AiContext context) {
      this.context = context;
      this.unit = unit;
      targetCoord = unit.Target.Coord;
      
      var direction = (targetCoord - unit.Coord).Normalized;
      if (CheckMove(direction)) return true;

      var (direction1, direction2) = direction.GetClosestDirections();
      if (CheckMove(direction1, direction2)) return true;

      var direction3 = direction1.GetClosestDirection(direction);
      var direction4 = direction2.GetClosestDirection(direction);
      if (CheckMove(direction3, direction4)) return true;
      
      var direction5 = direction3.GetClosestDirection(direction1);
      var direction6 = direction4.GetClosestDirection(direction2);
      if (CheckMove(direction5, direction6)) return true;

      return false;
    }

    bool CheckMove(Coord direction1, Coord direction2) {
      var coord1 = unit.Coord + direction1;
      var coord2 = unit.Coord + direction2;
      var canMove1 = context.IsTileEmpty(coord1);
      var canMove2 = context.IsTileEmpty(coord2);

      if (canMove1 && canMove2) return CheckSimulatedMove();
      if (canMove1) return SelectMove(coord1, direction1);
      if (canMove2) return SelectMove(coord2, direction2);

      return false;
      
      //Checks which coord will be closer to the target after simulating second move
      bool CheckSimulatedMove() {
        var (canMoveNext1, nextCoord1) = SimulateMove(coord1);
        var (canMoveNext2, nextCoord2) = SimulateMove(coord2);
        var distance1 = SqrDistance(targetCoord, nextCoord1);
        var distance2 = SqrDistance(targetCoord, nextCoord2);

        if (distance1 == distance2) return SelectShorterMove();

        return distance1 < distance2
          ? SelectMove(coord1, direction1)
          : SelectMove(coord2, direction2);

        bool SelectShorterMove() {
          if (canMoveNext1) return SelectMove(coord2, direction2);
          if (canMoveNext2) return SelectMove(coord1, direction1);

          return SelectMove(coord1, direction1);
        }
      }
    }

    bool SelectMove(Coord coord, Coord direction) {
      var time = unit.TimeToMove(direction.IsDiagonal);
      MoveInfo = new MoveInfo(coord, time);
      return true;
    }

    (bool, Coord) SimulateMove(Coord coord) {
      (bool, Coord) res = (false, coord);
      
      var direction = (unit.Target.Coord - coord).Normalized;
      if (CheckMove(direction)) return res;

      var (direction1, direction2) = direction.GetClosestDirections();
      if (CheckMove(direction1)) return res;
      if (CheckMove(direction2)) return res;

      return res;
      
      bool CheckMove(Coord moveDirection) {
        var pos = coord + moveDirection;
        if (!context.IsTileEmpty(pos)) return false;
        
        res = (true, pos);
        return true;
      }
    }

    bool CheckMove(Coord direction) {
      var newCoord = unit.Coord + direction;
      if (!context.IsTileEmpty(newCoord)) return false;

      return SelectMove(newCoord, direction);
    }

    IUnit unit;
    AiContext context;
    Coord targetCoord; }
}