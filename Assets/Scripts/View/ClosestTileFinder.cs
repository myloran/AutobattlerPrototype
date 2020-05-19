using UnityEngine;

namespace View {
  public class ClosestTileFinder {
    public ClosestTileFinder(BoardView board, BenchView bench1, BenchView bench2) {
      this.board = board;
      this.bench1 = bench1;
      this.bench2 = bench2;
    }

    public TileView Find(Vector3 position) {
      if (position.z - 0.5f < bench1.StartZPosition)
        return bench1.FindClosestTile(position);
      
      if (position.z + 0.5f > bench2.StartZPosition)
        return bench2.FindClosestTile(position);
      
      return board.FindClosestTile(position);
    }

    public bool IsBench(Vector3 position) => 
      position.z - 0.5f < bench1.StartZPosition 
      || position.z + 0.5f > bench2.StartZPosition;

    readonly BoardView board;
    readonly BenchView bench1, bench2;
  }
}