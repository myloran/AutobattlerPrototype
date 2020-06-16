using Controller.Update;
using Model;
using Model.NUnit;
using Shared;
using View.UIs;

namespace Controller.NDebug {
  public class UnitModelDebugController : ITick {
    public UnitModelDebugController(ModelContext context, ModelUI ui) {
      this.context = context;
      this.ui = ui;
    }
    
    public void Tick() {
      if (!isOn || unit == null) return;
      
      ui.UpdateText(unit.ToString());
    }
                                                       
    public void SelectUnitModel(Coord coord) => unit = context.GetUnit(coord);

    public void SetActive(bool isOn) {
      this.isOn = isOn;
      ui.gameObject.SetActive(isOn);
    }

    readonly ModelContext context;
    readonly ModelUI ui;
    Unit unit;
    bool isOn;
  }
}