using System.Linq;
using Controller.Update;
using View.UIs;
using UniRx;
using Controller.NUnit;
using Model.NBattleSimulation;
using Model.NUnit.Abstraction;
using UnityEngine;
using View.NTile;

namespace Controller.NDebug {
  public class UnitModelDebugController : ITick {
    public UnitModelDebugController(PlayerContext playerContext, Board board, ModelUI ui, DebugInfo debugInfo, 
        UnitSelectionController unitSelectionController, AiHeap heap, CoordFinder coordFinder) {
      this.playerContext = playerContext;
      this.board = board;
      this.ui = ui;
      this.debugInfo = debugInfo;
      this.unitSelectionController = unitSelectionController;
      this.heap = heap;
      this.coordFinder = coordFinder;
    }

    public void SubToUnitSelection(CompositeDisposable disposable) {
      unitSelectionController.UnitSelected.Subscribe(SelectUnitModel).AddTo(disposable);
      // unitSelectionController.UnitDeselected.Subscribe(Hide).AddTo(disposable);
    }

    public void Tick() {
      if (!debugInfo.IsDebugOn || !isOn || unit == null) return;
      
      ui.UpdateText(unit + $"\nCurrentTime:{heap.CurrentTime}");

      if (unit.Ability.TilesSelected.Any()) {
        foreach (var tile in unit.Ability.TilesSelected) {
          var from = coordFinder.PositionAt(tile);
          Debug.DrawLine(from, from + Vector3.up, Color.blue);
        }
      }
      else {
        foreach (var target in unit.Ability.TargetsSelected) {
          var from = coordFinder.PositionAt(target.Coord);
          Debug.DrawLine(from, from + Vector3.up, Color.blue);
        }
      }

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
    readonly AiHeap heap;
    readonly CoordFinder coordFinder;
    IUnit unit;
    bool isOn;
  }
}