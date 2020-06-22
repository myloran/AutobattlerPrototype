using System;
using Controller.UnitDrag;
using UniRx;

namespace Controller.NUnit {
  public class UnitMoveController : IDisposable {
    public UnitMoveController(PlayerSharedContext context, UnitDragController unitDragController) {
      this.context = context;
      this.unitDragController = unitDragController;
    }

    public void SubToDrag() => unitDragController.DragEnded.Subscribe(MoveUnit).AddTo(disposable);
    public void Dispose() => disposable.Clear();

    void MoveUnit(DragEndedEvent e) => context.Move(e.Start, e.Last); //TODO: expose unitMoved event so that battle simulation will subscribe and add the unit to simulation

    readonly PlayerSharedContext context;
    readonly UnitDragController unitDragController;
    readonly CompositeDisposable disposable = new CompositeDisposable();
  }
}