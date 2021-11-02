using System;

namespace Shared.Primitives {
  [Serializable]
  public class EffectInfo {
    public string Name;
    public float Damage;
    public float Heal;
    public float ModifyCritChance;
    public float ModifyStunChance;
    public float ModifyStunChanceDuration;
    public float SilenceDuration;
    public float TauntDuration;
    public float StunDuration;
  }
}