using System;
using Controller.Exts;
using UniRx;
using UnityEngine;
using static Shared.Const;

namespace Controller.Update {
  public class InputController : IDisposable {
    public IObservable<long> OnMouseDown,
      OnMouseHeld,
      OnMouseUp;

    public InputController(TickController tickController) => this.tickController = tickController;

    public void InitObservables() {  
      OnMouseDown = tickController.OnUpdate.Where(IsMouseDown).Connect(disposable);
      OnMouseHeld = tickController.OnUpdate.Where(IsMouseHeld).Connect(disposable);
      OnMouseUp = tickController.OnUpdate.Where(IsMouseUp);
    }

    public void Dispose() => disposable.Clear();

    bool IsMouseDown() => Input.GetMouseButtonDown(LeftButton);
    bool IsMouseHeld() => Input.GetMouseButton(LeftButton);
    bool IsMouseUp() => Input.GetMouseButtonUp(LeftButton);

    readonly TickController tickController;
    readonly CompositeDisposable disposable = new CompositeDisposable();
  }
}