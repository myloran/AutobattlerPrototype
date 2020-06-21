using System;
using Controller.Exts;
using Controller.NTile;
using Controller.NUnit;
using Controller.Update;
using Shared;
using Shared.Abstraction;
using Shared.Poco;
using UniRx;
using UniRx.Diagnostics;
using UnityEngine;
using View.NUnit;

namespace Controller.UnitDrag {
  public class UnitDragController : IDisposable {
    public IObservable<CoordChangedEvent> CoordChanged;
    public IObservable<DragEndedEvent> DragEnded;
    
    public UnitDragController(RaycastController raycastController, CoordFinderBySelectedPlayer coordFinderBySelectedPlayer,
        InputController inputController, UnitSelectionController unitSelectionController,
        IPredicate<UnitSelectedEvent> canStartDrag) {
      this.raycastController = raycastController;
      this.canStartDrag = canStartDrag;
      this.coordFinderBySelectedPlayer = coordFinderBySelectedPlayer;
      this.inputController = inputController;
      this.unitSelectionController = unitSelectionController;
    }
    
    public void Init() {
      HandleStartDrag();
      HandleDrag();
      HandleStopDrag();
    }
    
    void HandleStartDrag() {
      unitSelectionController.UnitSelected
        .Where(canStartDrag.Check)
        .Subscribe(StartDrag)
        .AddTo(disposable);
    }

    void HandleDrag() {
      CoordChanged = inputController.OnMouseHeld
        .Where(IsDragging)
        .SelectWhere(raycastController.RaycastPlane)
        .Select(MoveUnit)
        .SelectWhere(CoordIsChanged)
        .Connect(disposable);
      
      CoordChanged.Subscribe(UpdateLastCoord).AddTo(disposable);
    }

    void HandleStopDrag() {
      DragEnded = inputController.OnMouseUp
        .Where(IsDragging)
        .Select(Coords)
        .Connect(disposable);
      
      DragEnded.Subscribe(StopDrag).AddTo(disposable);
    }

    DragEndedEvent Coords() => new DragEndedEvent(startCoord, lastCoord);
    void UpdateLastCoord(CoordChangedEvent e) => lastCoord = e.To;
    bool IsDragging() => unit != null;
    
    void StartDrag(UnitSelectedEvent info) {
      unit = info.Unit;
      startCoord = info.StartCoord;
    }

    Vector3 MoveUnit(Vector3 mousePosition) => 
      unit.transform.position = mousePosition + new Vector3(0, 1, 0);

    (bool, CoordChangedEvent) CoordIsChanged(Vector3 mousePosition) {
      var coord = coordFinderBySelectedPlayer.Find(mousePosition);
      
      return coord == lastCoord 
        ? (false, default) 
        : (true, new CoordChangedEvent(lastCoord, coord));
    }
    
    void StopDrag() {
      unit = null;
      lastCoord = Coord.Invalid;
    }

    public void Dispose() => disposable.Clear();

    readonly RaycastController raycastController;
    readonly CoordFinderBySelectedPlayer coordFinderBySelectedPlayer;
    readonly InputController inputController;
    readonly UnitSelectionController unitSelectionController;
    readonly IPredicate<UnitSelectedEvent> canStartDrag;
    readonly CompositeDisposable disposable = new CompositeDisposable();
    Coord startCoord;
    Coord lastCoord = Coord.Invalid;
    UnitView unit;
  }
}