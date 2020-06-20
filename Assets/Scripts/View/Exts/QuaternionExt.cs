using Shared;
using UnityEngine;

namespace View.Exts {
  public static class QuaternionExt {
    public static Quaternion ToQuaternion(this Coord coord) {
      var direction = coord.Normalized;
      // int rotation = 0;
      // if (direction.X == 1) rotation += 45;
      // if (direction.X == -1) rotation += 225;
      // if (direction.Y == 0) rotation += 45;
      // if (direction.Y == -1) rotation += 90;
      // var x = direction.X == 1 ? 45 : 0;
      // var y = direction.Y == 0 ? 45 : 0;
      if (direction == (0, 1)) return Quaternion.Euler(0, 0, 0);
      if (direction == (1, 1)) return Quaternion.Euler(0, 45, 0);
      if (direction == (1, 0)) return Quaternion.Euler(0, 90, 0);
      if (direction == (1, -1)) return Quaternion.Euler(0, 135, 0);
      if (direction == (0, -1)) return Quaternion.Euler(0, 180, 0);
      if (direction == (-1, -1)) return Quaternion.Euler(0, 225, 0);
      if (direction == (-1, 0)) return Quaternion.Euler(0, 270, 0);
      if (direction == (-1, 1)) return Quaternion.Euler(0, 315, 0);
      return Quaternion.identity;
    }

    public static Quaternion ToQuaternion(this EPlayer player) =>
      player == EPlayer.First
        ? Quaternion.identity
        : Quaternion.Euler(new Vector3(0, 180, 0));
  }
}