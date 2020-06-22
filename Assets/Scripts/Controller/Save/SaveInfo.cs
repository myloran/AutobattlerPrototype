using System.Collections.Generic;
using MessagePack;
using Shared;
using Shared.Primitives;

namespace Controller.Save {
  [MessagePackObject]
  public class SaveInfo {
    [Key(0)] public string Name;
    [Key(1)] public Dictionary<Coord, string> Player1BenchUnits;
    [Key(2)] public Dictionary<Coord, string> Player2BenchUnits;
    [Key(3)] public Dictionary<Coord, string> Player1BoardUnits;
    [Key(4)] public Dictionary<Coord, string> Player2BoardUnits;
  }
}