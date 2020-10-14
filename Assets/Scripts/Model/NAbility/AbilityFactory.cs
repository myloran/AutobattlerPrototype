using System.Collections.Generic;
using Shared.Primitives;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NAbility {
  public class AbilityFactory {
    public AbilityFactory(Dictionary<string, AbilityInfo> abilities) {
      this.abilities = abilities;
    }

    public AbilityComponent Create(string name) {
      var info = abilities[name];
      var ability = new AbilityComponent()
        .SelectTarget(info.Target)
        .SelectUnitTargetingRule(info.UnitTargetingRule)
        .SetDamage(ToF32(info.Damage))
        .SetRange(ToF32(info.Range));
      return ability;
    }
    
    readonly Dictionary<string, AbilityInfo> abilities;
  }
}