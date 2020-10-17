using Shared;
using Shared.Primitives;
using TMPro;
using View.NUnit;

namespace View.UIs {
  public class UnitTooltipUI : AutoReferencer<UnitTooltipUI> {
    public TMP_Text TName,
      THealth,
      TArmor,
      TDamage,
      TAttackSpeed,
      TAttackRange,
      TMoveSpeed;

    public void SetUnitData(UnitStats unit) {
      TName.text = "Name: " + unit.Name;
      THealth.text = "Health: " + unit.Health;
      TArmor.text = "Armor: " + unit.Armor;
      TDamage.text = "Damage: " + unit.Damage;
      TAttackSpeed.text = "AttackSpeed: " + unit.AttackSpeed;
      TAttackRange.text = "AttackAnimationSpeed: " + unit.AttackAnimationHitTime;
      TMoveSpeed.text = "MoveSpeed: " + unit.MoveSpeed;
    }

    public void SetHealth(float health) => THealth.text = "Health: " + health;
  }
}