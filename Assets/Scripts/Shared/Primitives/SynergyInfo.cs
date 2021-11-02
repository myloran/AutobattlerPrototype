using System.Collections.Generic;

namespace Shared.Primitives {
  public class SynergyInfo : IInfo {
    public string Name { get; set; }
    public List<SynergyLevel> SynergyLevels = new List<SynergyLevel>();

  }

  public class SynergyLevel {
    public int UnitCount;
    public string EffectName;
  }
}