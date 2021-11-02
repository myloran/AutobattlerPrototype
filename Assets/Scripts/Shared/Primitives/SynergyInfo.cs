using System.Collections.Generic;

namespace Shared.Primitives {
  public class SynergyInfo {
    public string Name;
    public List<SynergyLevel> SynergyLevels = new List<SynergyLevel>();

  }

  public class SynergyLevel {
    public int UnitCount;
    public string EffectName;
  }
}