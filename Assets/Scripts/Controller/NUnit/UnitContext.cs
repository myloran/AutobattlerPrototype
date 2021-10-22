using Model.NUnit.Abstraction;
using Shared.Addons.Examples.FixMath;
using View.NUnit;

namespace Controller.NUnit {
  public class UnitContext {
    public UnitContext(IUnit model, UnitView view) {
      this.model = model;
      this.view = view;
    }

    public void SetMana(float amount) {
      model.Mana = F32.ToF32(amount);
      view.SetMana(amount);
    }

    readonly IUnit model;
    readonly UnitView view;
  }
}