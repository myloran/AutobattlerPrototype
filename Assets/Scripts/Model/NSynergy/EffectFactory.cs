using System.Collections.Generic;
using Model.NAbility.Abstraction;
using Model.NAbility.Effects;
using PlasticFloor.EventBus;
using Shared.Primitives;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NAbility {
  public class EffectFactory {
    public EffectFactory(IEventBus bus) {
      this.bus = bus;
    }

    public IEffect Create(EffectInfo info) {
      var effects = new List<IEffect>();
      
      if (info.Damage > 0) effects.Add(new DamageEffect(bus, ToF32(info.Damage)));
      if (info.SilenceDuration > 0) effects.Add(new SilenceEffect(bus, ToF32(info.SilenceDuration)));
      // if (info.TauntDuration > 0) effects.Add(new TauntEffect(bus, unit, ToF32(info.TauntDuration)));
      if (info.StunDuration > 0) effects.Add(new StunEffect(bus, ToF32(info.StunDuration)));
      if (info.Heal > 0) effects.Add(new HealEffect(bus, ToF32(info.Heal)));
      if (info.ModifyCritChance > 0) effects.Add(new ModifyCritChanceEffect(bus, ToF32(info.ModifyCritChance)));
      if (info.ModifyStunChance > 0) effects.Add(new ModifyStunChanceEffect(bus, ToF32(info.ModifyStunChance)));
      if (info.ModifyStunChanceDuration > 0) effects.Add(new ModifyStunChanceDurationEffect(bus, ToF32(info.ModifyStunChanceDuration)));
      if (info.ModifySilenceChance > 0) effects.Add(new ModifySilenceChanceEffect(bus, ToF32(info.ModifySilenceChance)));
      if (info.ModifySilenceChanceDuration > 0) effects.Add(new ModifySilenceChanceDurationEffect(bus, ToF32(info.ModifySilenceChanceDuration)));

      return new CompositeEffect(effects.ToArray());
    }

    readonly IEventBus bus;
  }
}                      