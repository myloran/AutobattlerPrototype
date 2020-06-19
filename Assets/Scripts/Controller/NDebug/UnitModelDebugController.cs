using System;
using Controller.UnitDrag;
using Controller.Update;
using Model;
using View.UIs;
using UniRx;
using Controller.Exts;
using Unit = Model.NUnit.Unit;

namespace Controller.NDebug {
  public class UnitModelDebugController : ITick, IDisposable {
    public UnitModelDebugController(ModelContext context, ModelUI ui, DebugInfo debugInfo,
      UnitSelectionController unitSelectionController) {
      this.context = context;
      this.ui = ui;
      this.debugInfo = debugInfo;
      this.unitSelectionController = unitSelectionController;
    }

    public void Init() {
      unitSelectionController.UnitSelected.Subscribe(SelectUnitModel).AddTo(disposable);
      // unitSelectionController.UnitDeselected.Subscribe(Hide).AddTo(disposable);
    }

    public void Tick() {
      if (!debugInfo.IsDebugOn || !isOn || unit == null) return;
      
      ui.UpdateText(unit.ToString());
    }

    void Hide() => SetActive(false);

    public void SetActive(bool isOn) {
      this.isOn = isOn;
      ui.gameObject.SetActive(isOn);
    }

    public void Dispose() => disposable.Clear();
                                                       
    void SelectUnitModel(UnitSelectedEvent e) {
      if (!debugInfo.IsDebugOn) return;
      
      unit = context.GetUnit(e.StartCoord);
    }

    readonly ModelContext context;
    readonly ModelUI ui;
    readonly DebugInfo debugInfo;
    readonly UnitSelectionController unitSelectionController;
    readonly CompositeDisposable disposable =new CompositeDisposable();
    Unit unit;
    bool isOn;
  }
}