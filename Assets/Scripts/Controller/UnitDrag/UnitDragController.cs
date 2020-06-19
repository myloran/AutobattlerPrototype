using System;
using Controller.Exts;
using Controller.NTile;
using Shared;
using UniRx;
using UnityEngine;
using View.NUnit;

namespace Controller.UnitDrag {
  public class UnitDragController : IDisposable {
    public IObservable<CoordChangedEvent> CoordChanged;
    public IObservable<DragEndedEvent> DragEnded;
    
    public UnitDragController(RaycastController raycastController, CoordFinder coordFinder,
        InputController inputController, IPredicate<DragInfo> canStartDrag) {
      this.raycastController = raycastController;
      this.canStartDrag = canStartDrag;
      this.coordFinder = coordFinder;
      this.inputController = inputController;
    }
    
    public void Init() {
      HandleStartDrag();
      HandleDrag();
      HandleStopDrag();
    }
    
    void HandleStartDrag() =>
      inputController.OnMouseDown
        .Select(raycastController.FireRaycast)
        .SelectWhere(raycastController.RaycastHitsUnit)
        .Select(DragInfo)
        .Where(canStartDrag.Check)
        .Subscribe(StartDrag)
        .AddTo(disposable);

    void HandleDrag() {
      CoordChanged = inputController.OnMouseHeld
        .Where(IsDragging)
        .SelectWhere(raycastController.RaycastPlane)
        .Select(MoveUnit)
        .SelectWhere(CoordIsChanged)
        .Publish()
        .Connect(disposable);
      
      CoordChanged.Subscribe(UpdateLastCoord).AddTo(disposable);
    }

    void HandleStopDrag() {
      DragEnded = inputController.OnMouseUp
        .Where(IsDragging)
        .Select(Coords)
        .Publish()
        .Connect(disposable);
      
      DragEnded.Subscribe(StopDrag).AddTo(disposable);
    }

    DragEndedEvent Coords() => new DragEndedEvent(startCoord, lastCoord);
    void UpdateLastCoord(CoordChangedEvent e) => lastCoord = e.To;
    bool IsDragging() => unit != null;
    
    void StartDrag(DragInfo info) {
      unit = info.Unit;
      startCoord = info.StartCoord;
    }

    DragInfo DragInfo(RaycastHit hit) {
      var u = hit.transform.GetComponent<UnitView>();
      var c = coordFinder.FindClosest(u);
      return new DragInfo(u, c);
    }
    
    Vector3 MoveUnit(Vector3 mousePosition) => 
      unit.transform.position = mousePosition + new Vector3(0, 1, 0);

    (bool, CoordChangedEvent) CoordIsChanged(Vector3 mousePosition) {
      var coord = coordFinder.Find(mousePosition);
      
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
    readonly CoordFinder coordFinder;
    readonly InputController inputController;
    readonly IPredicate<DragInfo> canStartDrag;
    readonly CompositeDisposable disposable = new CompositeDisposable();
    Coord startCoord;
    Coord lastCoord = Coord.Invalid;
    UnitView unit;
  }
}