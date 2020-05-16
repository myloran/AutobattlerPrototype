using System;
using UnityEngine;

namespace View {
  public class BoardView : MonoBehaviour {
    public TileView TilePrebab;
    public Transform StartPoint;

    void Start() {
      for (int x = 0; x < 8; x++) {
        for (int y = 0; y < 6; y++) {
          tiles[x, y] = Instantiate(TilePrebab, StartPoint.position + new Vector3(x, 0, y), 
            Quaternion.Euler(90, 0, 0));
          tiles[x, y].X = x;
          tiles[x, y].Y = y;
        }
      }
    }

    readonly TileView[,] tiles = new TileView[8, 6];
  }
}