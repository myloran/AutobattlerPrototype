using System.Collections.Generic;
using FixMath;
using static FixMath.F32;

namespace Model.NUnit {
  public class CHealth  {
    public F32 StartingHealth;
    public F32 Health;
    public F32 Armor;
    
    public CHealth(F32 startingHealth, F32 armor) {
      StartingHealth = startingHealth;
      Armor = armor;
    }

    public bool IsAlive => Health > 0;

    public void Reset() => Health = StartingHealth;
    public void SubToDeath(CTarget target) => observers.Add(target);
    public void UnsubFromDeath(CTarget target) => observers.Remove(target);

    public void TakeDamage(F32 damage) {
      var damageDealt = damage - damage * Armor / (Armor + 10);
      Health -= damageDealt;
      Health = Clamp(Health, Zero, StartingHealth);
      
      if (!IsAlive) NotifyDied();
    }

    void NotifyDied() {
      for (var i = observers.Count - 1; i >= 0; i--) {
        observers[i].Clear();
      }
    }

    public override string ToString() => $"{nameof(StartingHealth)}: {StartingHealth}, {nameof(Health)}: {Health}, {nameof(Armor)}: {Armor}";

    readonly List<CTarget> observers = new List<CTarget>();
  }
}