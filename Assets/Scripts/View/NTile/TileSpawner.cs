using System.Collections.Generic;
using Shared;
using Shared.Poco;
using UnityEngine;
using View.Views;
using static Shared.Const;

namespace View.NTile {
  public class TileSpawner {
    public TileSpawner(TileStartPoints startPoints, TileViewFactory tileFactory) {
      this.startPoints = startPoints;
      this.tileFactory = tileFactory;
    }

    public void Init() {
      for (int x = 0; x < BoardSizeX; x++) {
        for (int y = 0; y < BoardSizeY; y++) {
          var coord = new Coord(x, y);
          var position = startPoints.Board.position + new Vector3(x, 0, y);
          tiles[coord] = tileFactory.Create(position);
        }
      }
      
      for (int x = 0; x < BenchSizeX; x++) {
        var coord = new Coord(x, Player1BenchId);
        tiles[coord] = tileFactory.Create(startPoints.Bench1.position + new Vector3(x, 0, 0));
      }
      
      for (int x = 0; x < BenchSizeX; x++) {
        var coord = new Coord(x, Player2BenchId);
        tiles[coord] = tileFactory.Create(startPoints.Bench2.position + new Vector3(x, 0, 0));
      }
    }
    
    public TileView TileAt(Coord coord) => tiles[coord];
    public IEnumerable<TileView> Values => tiles.Values;

    readonly TileViewFactory tileFactory;
    readonly TileStartPoints startPoints;
    readonly Dictionary<Coord, TileView> tiles = new Dictionary<Coord, TileView>(
      BoardSizeX * BoardSizeY + BenchSizeX * 2);
  }
}