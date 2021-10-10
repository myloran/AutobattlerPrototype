using System;
using System.Collections.Generic;

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

  public enum EAdditionalTargets {
    None,
    AlongLine,
  }

  public enum ETiming {
    Once,
    Period,
  }
  
  [Serializable]
  public class AbilityInfo {
    public string Name;
    public List<string> NestedAbilities = new List<string>();
    public ETarget Target;
    public EUnitTargetingRule UnitTargetingRule;
    public EAdditionalTargets AdditionalTargets;
    public ETiming Timing;
    public bool IsTimingOverridden;
    public float Damage;
    public float Range;
    public float AnimationHitTime;
    public float AnimationTotalTime;
    public float TimingPeriod;
    public int TimingCount;
    public float TimingInitialDelay;
    public float SilenceDuration;

    public AbilityInfo() { }

    public AbilityInfo(AbilityInfo info) {
      Name = info.Name;
      Target = info.Target;
      UnitTargetingRule = info.UnitTargetingRule;
      AdditionalTargets = info.AdditionalTargets;
      Damage = info.Damage;
      Range = info.Range;
      Timing = info.Timing;
      TimingPeriod = info.TimingPeriod;
      TimingCount = info.TimingCount;
      IsTimingOverridden = info.IsTimingOverridden;
      TimingInitialDelay = info.TimingInitialDelay;
    }
  }
}