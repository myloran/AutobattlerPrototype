using System;
using System.Collections.Generic;

namespace Shared.Primitives {
  public enum ETarget {
    Unit,
    Tile
  }

  public enum EUnitTargetingRule {
    Closest,
    Random,
    Farthest,
    MaxAbilityRange
  }

  public enum EAdditionalTargets {
    None,
    AlongLine,
  }

  public enum ETiming {
    Once,
    Period,
  }

  public enum ETargetPlayer {
    Enemy,
    Friend,
  }
  
  [Serializable]
  public class AbilityInfo {
    public string Name;
    public List<string> NestedAbilities = new List<string>();
    public ETargetPlayer TargetPlayer;
    public ETarget Target;
    public EUnitTargetingRule UnitTargetingRule;
    public EAdditionalTargets AdditionalTargets;
    public ETiming Timing;
    public bool IsTimingOverridden;
    public bool NeedRecalculateTarget;
    public float Damage;
    public float TargetingRange;
    public float AbilityRange; //TargetingRange, AbilityRange
    public float AnimationHitTime;
    public float AnimationTotalTime;
    public float TimingPeriod;
    public int TimingCount;
    public float TimingInitialDelay;
    public float SilenceDuration;

    public AbilityInfo() { }

    public AbilityInfo(AbilityInfo info) {
      Name = info.Name;
      TargetPlayer = info.TargetPlayer;
      Target = info.Target;
      UnitTargetingRule = info.UnitTargetingRule;
      AdditionalTargets = info.AdditionalTargets;
      Damage = info.Damage;
      AbilityRange = info.AbilityRange;
      Timing = info.Timing;
      TimingPeriod = info.TimingPeriod;
      TimingCount = info.TimingCount;
      IsTimingOverridden = info.IsTimingOverridden;
      NeedRecalculateTarget = info.NeedRecalculateTarget;
      TimingInitialDelay = info.TimingInitialDelay;
    }
  }

  public static class AbilityInfoExt {
    public static EPlayer GetPlayer(this ETargetPlayer targetPlayer, EPlayer self) {
      switch (targetPlayer) {
        case ETargetPlayer.Enemy:
          return self.Opposite();
          break;
        case ETargetPlayer.Friend:
          return self;
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(targetPlayer), targetPlayer, null);
      }
    }
  }
}