using Controller.Update;
using View.UIs;
using UniRx;
using Controller.NUnit;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;

namespace Controller.NDebug {
  public class UnitModelDebugController : ITick {
    public UnitModelDebugController(PlayerContext playerContext, Board board,
        ModelUI ui, DebugInfo debugInfo, UnitSelectionController unitSelectionController) {
      this.playerContext = playerContext;
      this.board = board;
      this.ui = ui;
      this.debugInfo = debugInfo;
      this.unitSelectionController = unitSelectionController;
    }

    public void SubToUnitSelection(CompositeDisposable disposable) {
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

    void SelectUnitModel(UnitSelectedEvent e) => 
      unit = board.TryGetUnit(e.StartCoord) ?? playerContext.GetUnit(e.StartCoord);

    readonly PlayerContext playerContext;
    readonly Board board;
    readonly ModelUI ui;
    readonly DebugInfo debugInfo;
    readonly UnitSelectionController unitSelectionController;
    IUnit unit;
    bool isOn;
  }
}