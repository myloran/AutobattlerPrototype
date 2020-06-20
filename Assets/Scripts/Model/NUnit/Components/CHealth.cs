using System.Collections.Generic;
using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;
using static Shared.Addons.Examples.FixMath.F32;

namespace Model.NUnit.Components {
  public class CHealth : IHealth {
    public F32 Health { get; private set; }

    public CHealth(F32 startingHealth, F32 armor) {
      this.startingHealth = startingHealth;
      this.armor = armor;
    }

    public bool IsAlive => Health > 0;

    public void Reset() {
      Health = startingHealth;
      observers.Clear();
    }

    public void SubToDeath(ITarget target) => observers.Add(target);
    public void UnsubFromDeath(ITarget target) => observers.Remove(target);

    public void TakeDamage(F32 damage) {
      var damageDealt = damage - damage * armor / (armor + 10);
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

    readonly List<ITarget> observers = new List<ITarget>();
    F32 startingHealth;
    F32 armor;
  }
}