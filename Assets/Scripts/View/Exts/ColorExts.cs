using Shared;
using Shared.Primitives;
using UnityEngine;
using static UnityEngine.Color;

namespace View.Exts {
  public static class ColorExts {
    public static Color ToColor(this EPlayer player) => player == EPlayer.First ? blue : red;
  }
}