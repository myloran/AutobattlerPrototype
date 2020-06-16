using System;
using Controller.Exts;
using Controller.Update;
using UnityEngine;

namespace Controller {
  public class InputController {
    public IObservable<long> OnMouseDown,
      OnMouseHeld,
      OnMouseUp;

    public InputController(TickController tickController) => this.tickController = tickController;

    public void Init() {
      OnMouseDown = tickController.OnUpdate.Where(IsMouseDown);
      OnMouseHeld = tickController.OnUpdate.Where(IsMouseHeld);
      OnMouseUp = tickController.OnUpdate.Where(IsMouseUp);
    }

    bool IsMouseDown() => Input.GetMouseButtonDown(0);
    bool IsMouseHeld() => Input.GetMouseButton(0);
    bool IsMouseUp() => Input.GetMouseButtonUp(0);

    readonly TickController tickController;
  }
}