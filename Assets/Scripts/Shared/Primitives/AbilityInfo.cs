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
    Self
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
  public class AbilityInfo : IInfo {
    //id
    public string Name { get; set; }
    
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
    public float Heal;
    public float ModifyCritChance;
    public float SilenceDuration;
    public float TauntDuration;
    public float StunDuration;
    
    //visual stuff
    public float AnimationHitTime;
    public float AnimationTotalTime;

    public AbilityInfo() { }
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