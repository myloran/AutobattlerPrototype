using System.Collections.Generic;
using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NUnit.Components {
  public class HealthComponent : IHealth {
    public F32 Health { get; private set; }

    public HealthComponent(F32 startingHealth, F32 armor) {
      this.startingHealth = startingHealth;
      this.armor = armor;
    }

    public bool IsAlive => Health > 0;

    public void Reset() {
      Health = startingHealth;
      observers.Clear();
    }

    public void SubToDeath(ITargeting targeting) => observers.Add(targeting);
    public void UnsubFromDeath(ITargeting targeting) => observers.Remove(targeting);

    public void ApplyHeal(F32 heal) {
      Health += heal;
      Health = Clamp(Health, Zero, startingHealth);
    }

    public void TakeDamage(F32 damage) {
      var damageDealt = damage - damage * armor / (armor + 10); //TODO: take magic resist into account when taking ability damage
      Health -= damageDealt;
      Health = Clamp(Health, Zero, startingHealth);
      
      if (!IsAlive) NotifyDied();
    }

    void NotifyDied() {
      for (var i = observers.Count - 1; i >= 0; i--) {
        observers[i].ClearTarget();
      }
    }

    public override string ToString() => $"{nameof(startingHealth)}: {startingHealth}, {nameof(Health)}: {Health}, {nameof(armor)}: {armor}";

    readonly List<ITargeting> observers = new List<ITargeting>();
    F32 startingHealth;
    F32 armor;
  }
}