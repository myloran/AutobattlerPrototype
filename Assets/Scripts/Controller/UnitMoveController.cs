using System;
using Controller.UnitDrag;
using UniRx;

namespace Controller {
  public class UnitMoveController : IDisposable {
    public UnitMoveController(WorldContext context, UnitDragController unitDragController) {
      this.context = context;
      this.unitDragController = unitDragController;
    }

    public void Init() => unitDragController.DragEnded.Subscribe(MoveUnit).AddTo(disposable);
    public void Dispose() => disposable.Clear();

    void MoveUnit(DragEndedEvent e) => context.Move(e.Start, e.Last);

    readonly WorldContext context;
    readonly UnitDragController unitDragController;
    readonly CompositeDisposable disposable = new CompositeDisposable();
  }
}