using System;
using Controller.Exts;
using Controller.Update;
using UniRx;
using UnityEngine;
using View.NTile;
using View.NUnit;

namespace Controller.NUnit {
  public class UnitSelectionController {
    public IObservable<UnitSelectedEvent> UnitSelected;
    public IObservable<Unit> UnitDeselected;
    
    public UnitSelectionController(InputController inputController, 
        RaycastController raycastController, CoordFinder coordFinder) {
      this.inputController = inputController;
      this.raycastController = raycastController;
      this.coordFinder = coordFinder;
    }
    
    public void SubToInput(CompositeDisposable disposable) {
      InitUnitSelected();
      InitUnitDeselected();
      
      void InitUnitDeselected() {
        UnitDeselected = inputController.OnMouseUp
          .Where(_ => !UIExt.IsPointerOverUIElement())
          .Select(raycastController.FireRaycast)
          .Where(_ => !raycastController.RaycastHitsUnit(_).isHit) //TODO: do one raycast with layers combines
          .Where(raycastController.RaycastHitsGlobalCollider)
          .AsUnitObservable();
      }

      void InitUnitSelected() {
        UnitSelected = inputController.OnMouseDown
          .Select(raycastController.FireRaycast)
          .SelectWhere(raycastController.RaycastHitsUnit)
          .Select(DragInfo)
          .Connect(disposable);
      }
    }


    UnitSelectedEvent DragInfo(RaycastHit hit) {
      var unit = hit.transform.GetComponent<UnitView>();
      var coord = coordFinder.FindClosestCoord(unit.transform.position, unit.Player);
      return new UnitSelectedEvent(unit, coord);
    }

    readonly InputController inputController;
    readonly RaycastController raycastController;
    readonly CoordFinder coordFinder;
  }
}