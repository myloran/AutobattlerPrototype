using Shared;
using Shared.Poco;
using UnityEngine;
using View.Views;
using static Shared.Const;
using static UnityEngine.Mathf;

namespace View.NTile {
  public class TilePresenter { //TODO: rename to CoordFinder?
    public TilePresenter(TileStartPoints startPoints) => this.startPoints = startPoints;

    public Coord FindClosestCoord(Vector3 position, EPlayer selectedPlayer) {
      var (didFind, coord) = FindCoordOnBenches(position, selectedPlayer);
      return didFind ? coord : FindCoordOnBoard(position);
    }

    public Coord FindClosestCoordLimitedByPlayerSide(Vector3 position, EPlayer selectedPlayer) {
      var (didFind, coord) = FindCoordOnBenches(position, selectedPlayer);
      return didFind ? coord : FindCoordOnBoard(position).LimitByPlayerSide(selectedPlayer);
    }

    (bool, Coord) FindCoordOnBenches(Vector3 position, EPlayer selectedPlayer) {
      if (position.z - TileHalfLength < startPoints.Bench1.position.z && selectedPlayer == EPlayer.First) {
        var coord = FindCoordOnBench(position, startPoints.Bench1.position, selectedPlayer);
        return (true, coord);
      }

      if (position.z + TileHalfLength > startPoints.Bench2.position.z && selectedPlayer == EPlayer.Second) {
        var coord = FindCoordOnBench(position, startPoints.Bench2.position, selectedPlayer);
        return (true, coord);
      }

      return (false, default);
    }

    public Vector3 PositionAt(Coord coord) {
      var startPoint = coord.IsPlayer1Bench() ? startPoints.Bench1
        : coord.IsPlayer2Bench() ? startPoints.Bench2
        : startPoints.Board;
      return startPoint.position + new Vector3(coord.X, 0, Max(coord.Y, 0));
    }

    Coord FindCoordOnBoard(Vector3 position) {
      var indexPosition = position - startPoints.Board.position;
      var indexX = RoundToInt(indexPosition.x);
      var indexY = RoundToInt(indexPosition.z);
      var x = Clamp(indexX, 0, BoardSizeX - 1);
      var y = Clamp(indexY, 0, BoardSizeY - 1);

      return new Coord(x, y);
    }
    
    Coord FindCoordOnBench(Vector3 position, Vector3 startPosition, EPlayer selectedPlayer) {
      var indexPosition = position - startPosition;
      var index = RoundToInt(indexPosition.x);
      var indexClamped = Clamp(index, 0, BenchSizeX - 1); 
      
      return new Coord(indexClamped, selectedPlayer.BenchId());
    }

    readonly TileStartPoints startPoints;
  }
}