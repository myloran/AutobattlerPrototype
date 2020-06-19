using Controller.Update;
using Model;
using Model.NUnit;
using Shared;
using View.UIs;

namespace Controller.NDebug {
  public class UnitModelDebugController : ITick {
    public UnitModelDebugController(ModelContext context, ModelUI ui, DebugInfo debugInfo) {
      this.context = context;
      this.ui = ui;
      this.debugInfo = debugInfo;
    }
    
    public void Tick() {
      if (!debugInfo.IsDebugOn || !isOn || unit == null) return;
      
      ui.UpdateText(unit.ToString());
    }
                                                       
    public void SelectUnitModel(Coord coord) {
      if (!debugInfo.IsDebugOn) return;
      
      unit = context.GetUnit(coord);
    }

    public void SetActive(bool isOn) {
      this.isOn = isOn;
      ui.gameObject.SetActive(isOn);
    }

    readonly ModelContext context;
    readonly ModelUI ui;
    readonly DebugInfo debugInfo;
    Unit unit;
    bool isOn;
  }
}