using System.Collections.Generic;
using Shared;
using UnityEngine;
using static UnityEngine.Mathf;

namespace View.Presenters {
  public class TilePresenter {
    public TilePresenter(TileStartPoints startPoints, TileViewFactory tileFactory) {
      this.startPoints = startPoints;
      
      //TODO: extract logic from constructor
      for (int x = 0; x < 8; x++) { //TODO: extract tile creation and tiles getter logic
        for (int y = 0; y < 6; y++) {
          var coord = new Coord(x, y);
          var position = startPoints.Board.position + new Vector3(x, 0, y);
          tiles[coord] = tileFactory.Create(position);
        }
      }
      
      for (int x = 0; x < 10; x++) {
        var coord = new Coord(x, -1);
        tiles[coord] = tileFactory.Create(startPoints.Bench1.position + new Vector3(x, 0, 0));
      }
      
      for (int x = 0; x < 10; x++) {
        var coord = new Coord(x, -2);
        tiles[coord] = tileFactory.Create(startPoints.Bench2.position + new Vector3(x, 0, 0));
      }
    }
    
    public Coord FindClosestCoord(Vector3 position, EPlayer selectedPlayer) {
      var (didFind, coord) = SearchCoordAtBench(position, selectedPlayer);
      return didFind ? coord : FindCoordOnBoard(position);
    }

    public Coord FindClosestCoordLimitedByPlayerSide(Vector3 position, EPlayer selectedPlayer) {
      var (didFind, coord) = SearchCoordAtBench(position, selectedPlayer);
      return didFind ? coord : FindCoordOnBoard(position).LimitByPlayerSide(selectedPlayer);
    }

    (bool, Coord) SearchCoordAtBench(Vector3 position, EPlayer selectedPlayer) {
      if (position.z - 0.5f < startPoints.Bench1.position.z && selectedPlayer == EPlayer.First) {
        var coord = FindCoordOnBench(position, startPoints.Bench1.position, selectedPlayer);
        return (true, coord);
      }

      if (position.z + 0.5f > startPoints.Bench2.position.z && selectedPlayer == EPlayer.Second) {
        var coord = FindCoordOnBench(position, startPoints.Bench2.position, selectedPlayer);
        return (true, coord);
      }

      return (false, default);
    }

    public Vector3 PositionAt(Coord coord) {
      var startPoint = coord.Y == -1 ? startPoints.Bench1
        : coord.Y == -2 ? startPoints.Bench2
        : startPoints.Board;
      return startPoint.position + new Vector3(coord.X, 0, Max(coord.Y, 0));
    }

    public TileView TileAt(Coord coord) => tiles[coord];
        
    Coord FindCoordOnBoard(Vector3 position) {
      var indexPosition = position - startPoints.Board.position;
      var indexX = RoundToInt(indexPosition.x);
      var indexY = RoundToInt(indexPosition.z);
      var x = Clamp(indexX, 0, 7);
      var y = Clamp(indexY, 0, 5);

      return new Coord(x, y);
    }
    
    Coord FindCoordOnBench(Vector3 position, Vector3 startPosition, EPlayer selectedPlayer) {
      var indexPosition = position - startPosition;
      var index = RoundToInt(indexPosition.x);
      var indexClamped = Clamp(index, 0, 9); 
      
      return new Coord(indexClamped, selectedPlayer.BenchId());
    }

    readonly Dictionary<Coord, TileView> tiles = new Dictionary<Coord, TileView>(8 * 6 + 10 * 2);
    readonly TileStartPoints startPoints;
  }
}