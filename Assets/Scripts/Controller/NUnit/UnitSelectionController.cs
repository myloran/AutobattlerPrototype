using System;
using Controller.Exts;
using Controller.NTile;
using Controller.Update;
using UniRx;
using UnityEngine;
using View.NUnit;

namespace Controller.NUnit {
  public class UnitSelectionController : IDisposable {
    public IObservable<UnitSelectedEvent> UnitSelected;
    public IObservable<Unit> UnitDeselected;
    
    public UnitSelectionController(InputController inputController, 
        RaycastController raycastController, CoordFinder coordFinder) {
      this.inputController = inputController;
      this.raycastController = raycastController;
      this.coordFinder = coordFinder;
    }
    
    public void Init() {
      UnitSelected = inputController.OnMouseDown
        .Select(raycastController.FireRaycast)
        .SelectWhere(raycastController.RaycastHitsUnit)
        .Select(DragInfo)
        .Connect(disposable);

      UnitDeselected = inputController.OnMouseUp
        .Where(_ => !UIExt.IsPointerOverUIElement())
        .Select(raycastController.FireRaycast)
        .Where(_ => !raycastController.RaycastHitsUnit(_).isHit)
        .Where(raycastController.RaycastHitsGlobalCollider)
        .AsUnitObservable();
    }

    UnitSelectedEvent DragInfo(RaycastHit hit) {
      var u = hit.transform.GetComponent<UnitView>();
      var c = coordFinder.FindClosest(u);
      return new UnitSelectedEvent(u, c);
    }

    public void Dispose() => disposable?.Dispose();
    
    readonly InputController inputController;
    readonly RaycastController raycastController;
    readonly CoordFinder coordFinder;
    readonly CompositeDisposable disposable = new CompositeDisposable();
  }
}