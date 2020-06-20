using System;
using Controller.Exts;
using Controller.Update;
using UniRx;
using UnityEngine;

namespace Controller {
  public class InputController : IDisposable {
    public IObservable<long> OnMouseDown,
      OnMouseHeld,
      OnMouseUp;

    public InputController(TickController tickController) => this.tickController = tickController;

    public void Init() {  
      OnMouseDown = tickController.OnUpdate.Where(IsMouseDown).Connect(disposable);
      OnMouseHeld = tickController.OnUpdate.Where(IsMouseHeld).Connect(disposable);
      OnMouseUp = tickController.OnUpdate.Where(IsMouseUp);
    }

    public void Dispose() => disposable.Clear();

    bool IsMouseDown() => Input.GetMouseButtonDown(0);
    bool IsMouseHeld() => Input.GetMouseButton(0);
    bool IsMouseUp() => Input.GetMouseButtonUp(0);

    readonly TickController tickController;
    readonly CompositeDisposable disposable = new CompositeDisposable();
  }
}