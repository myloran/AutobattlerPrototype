using System;

namespace Shared.Primitives {
  public enum ETarget {
    Unit,
    Tile
  }

  public enum EUnitTargetingRule {
    Random,
    Closest,
    Farest,
  }
  
  [Serializable]
  public class AbilityInfo {
    public string Name;
    public ETarget Target;
    public EUnitTargetingRule UnitTargetingRule;
    public float Damage;
    public float Range;

    public AbilityInfo() { }

    public AbilityInfo(AbilityInfo info) {
      Name = info.Name;
      Target = info.Target;
      UnitTargetingRule = info.UnitTargetingRule;
      Damage = info.Damage;
      Range = info.Range;
    }
  }
}