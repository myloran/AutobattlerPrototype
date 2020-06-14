using System.Collections.Generic;

namespace Model.NUnit {
  public class CHealth  {
    public float StartingHealth;
    public float Health;
    public float Armor;
    
    public CHealth(float startingHealth, float armor) {
      StartingHealth = startingHealth;
      Armor = armor;
    }

    public bool IsAlive => Health > 0;

    public void Reset() => Health = StartingHealth;
    public void SubToDeath(CTarget target) => observers.Add(target);
    public void UnsubFromDeath(CTarget target) => observers.Remove(target);

    public void TakeDamage(float damage) {
      var damageDealt = damage - damage * Armor / (Armor + 10);
      Health -= damageDealt;
      Health = MathExt.Clamp(Health, 0f, StartingHealth);
      
      if (!IsAlive) NotifyDied();
    }

    void NotifyDied() {
      observers.ForEach(o => o.OnDeath());
      observers.Clear();
    }

    public override string ToString() => $"{nameof(StartingHealth)}: {StartingHealth}, {nameof(Health)}: {Health}, {nameof(Armor)}: {Armor}";

    readonly List<CTarget> observers = new List<CTarget>();
  }
}