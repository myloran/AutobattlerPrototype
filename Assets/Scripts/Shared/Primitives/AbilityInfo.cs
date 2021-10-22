using System;
using System.Collections.Generic;

namespace Shared.Primitives {
  public enum ETarget {
    Unit,
    Tile,
  }

  public enum EUnitTargetingRule {
    Closest,
    Random,
    Farthest,
    MaxAbilityRange,
  }

  public enum EAdditionalTargets {
    None,
    AlongLine,
    WithinRadius,
  }

  public enum ETiming {
    Once,
    Period,
  }

  public enum ETargetPlayer {
    Enemy,
    Friend,
  }
  
  public enum EStartingPosition {
    Target,
    Self,
    Custom,
  }

  public enum EAbilityOrigin {
    Target,
    Self,
    Custom
  }
  
  [Serializable]
  public class AbilityInfo {
    //id
    public string Name;
    
    //nested abilities
    public List<string> NestedAbilities = new List<string>();
    public bool IsTimingOverridden;
    
    //targeting
    public ETargetPlayer TargetPlayer;
    public ETarget Target;
    public EUnitTargetingRule UnitTargetingRule;
    public EAdditionalTargets AdditionalTargets;
    public EAbilityOrigin AbilityOrigin;
    public float TargetingRange;
    public bool NeedRecalculateTarget;
    public bool IsReusingAttackTarget;
    
    //?
    public float AbilityRadius;
    
    //timing
    public ETiming Timing;
    public float TimingPeriod;
    public int TimingCount;
    public float TimingInitialDelay;
    
    //properties
    public float Damage;
    public float SilenceDuration;
    public float TauntDuration;
    
    //visual stuff
    public float AnimationHitTime;
    public float AnimationTotalTime;

    public AbilityInfo() { }

    public AbilityInfo(AbilityInfo info) {
      Name = info.Name;
      TargetPlayer = info.TargetPlayer;
      Target = info.Target;
      UnitTargetingRule = info.UnitTargetingRule;
      AdditionalTargets = info.AdditionalTargets;
      Damage = info.Damage;
      AbilityRadius = info.AbilityRadius;
      Timing = info.Timing;
      TimingPeriod = info.TimingPeriod;
      TimingCount = info.TimingCount;
      IsTimingOverridden = info.IsTimingOverridden;
      NeedRecalculateTarget = info.NeedRecalculateTarget;
      TimingInitialDelay = info.TimingInitialDelay;
      IsReusingAttackTarget = info.IsReusingAttackTarget;
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