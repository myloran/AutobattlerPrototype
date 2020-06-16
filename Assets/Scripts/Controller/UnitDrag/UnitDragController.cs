using Controller.Exts;
using Controller.NTile;
using Shared;
using UniRx;
using UnityEngine;
using View.Views;

namespace Controller.UnitDrag {
  public class UnitDragController {
    public UnitDragController(RaycastController raycastController, CoordFinder coordFinder,
        InputController inputController, IHandler<EndDragEvent> endDragHandler,
        IHandler<CoordChangedEvent> coordChangedHandler, IPredicate<DragInfo> canStartDrag) {
      this.raycastController = raycastController;
      this.endDragHandler = endDragHandler;
      this.coordChangedHandler = coordChangedHandler;
      this.canStartDrag = canStartDrag;
      this.coordFinder = coordFinder;
      this.inputController = inputController;
    }
    
    public void Init() {
      inputController.OnMouseDown
        .Select(raycastController.FireRaycast)
        .SelectWhere(raycastController.RaycastHitsUnit)
        .Select(PrepareDragInfo)
        .Where(canStartDrag.Check)
        .Subscribe(StartDrag);

      inputController.OnMouseHeld
        .Where(IsDragging)
        .SelectWhere(raycastController.RaycastPlane)
        .Select(MoveUnit)
        .SelectWhere(IsCoordChanged)
        .Do(coordChangedHandler.Handle)
        .Subscribe(UpdateLastCoord);
      
      inputController.OnMouseUp
        .Where(IsDragging)
        .Select(GetCoords)
        .Do(endDragHandler.Handle)
        .Subscribe(StopDrag);
    }

    EndDragEvent GetCoords() => new EndDragEvent(startCoord, lastCoord);
    void UpdateLastCoord(CoordChangedEvent e) => lastCoord = e.To;
    bool IsDragging() => unit != null;
    
    void StartDrag(DragInfo info) {
      unit = info.Unit;
      startCoord = info.StartCoord;
    }

    DragInfo PrepareDragInfo(RaycastHit hit) {
      var u = hit.transform.GetComponent<UnitView>();
      var c = coordFinder.FindClosest(u);
      return new DragInfo(u, c);
    }
    
    Vector3 MoveUnit(Vector3 mousePosition) => 
      unit.transform.position = mousePosition + new Vector3(0, 1, 0);

    (bool, CoordChangedEvent) IsCoordChanged(Vector3 mousePosition) {
      var coord = coordFinder.Find(mousePosition);
      
      return coord == lastCoord 
        ? (false, default) 
        : (true, new CoordChangedEvent(lastCoord, coord));
    }
    
    void StopDrag() {
      unit = null;
      lastCoord = Coord.Invalid;
    }

    readonly RaycastController raycastController;
    readonly CoordFinder coordFinder;
    readonly InputController inputController;
    readonly IPredicate<DragInfo> canStartDrag;
    readonly IHandler<CoordChangedEvent> coordChangedHandler;
    readonly IHandler<EndDragEvent> endDragHandler;
    Coord startCoord;
    Coord lastCoord = Coord.Invalid;
    UnitView unit;
  }
}