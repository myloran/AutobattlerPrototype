using UnityEngine;

namespace Model.NUnit {
  public class CHealth {
    public float StartingHealth;
    public float Health;
    public float Armor;
    
    public CHealth(float startingHealth, float armor) {
      StartingHealth = startingHealth;
      Armor = armor;
    }

    public bool IsAlive => Health > 0;

    public void Reset() {
      Health = StartingHealth;
    }

    public void CalculateDamage(float damage) {
      var damageDealt = damage - damage * Armor / (Armor + 10);
      Health -= damageDealt;
      Mathf.Clamp(Health, 0f, StartingHealth);
    }
  }
}