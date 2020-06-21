using System;
using Controller.Exts;
using UniRx;
using UnityEngine;

namespace Controller.Update {
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

    bool IsMouseDown() => Input.GetMouseButtonDown(0); //TODO: make const LeftClick
    bool IsMouseHeld() => Input.GetMouseButton(0);
    bool IsMouseUp() => Input.GetMouseButtonUp(0);

    readonly TickController tickController;
    readonly CompositeDisposable disposable = new CompositeDisposable();
  }
}