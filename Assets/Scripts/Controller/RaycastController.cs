using System;
using Controller.Exts;
using Controller.Update;
using Shared.OkwyLogging;
using UniRx;
using UnityEngine;
using Logger = Shared.OkwyLogging.Logger;

namespace Controller {
  public class RaycastController : ITick {
    public RaycastController(Camera camera, int globalLayer, int unitLayer, 
        UnitTooltipController controller) {
      this.camera = camera;
      this.globalLayer = globalLayer;
      this.unitLayer = unitLayer;
      this.controller = controller;
    }

    public Ray FireRaycast() => camera.ScreenPointToRay(Input.mousePosition);

    public (bool isHit, RaycastHit hit) RaycastHitsUnit(Ray ray) {
      var isHit = Physics.Raycast(ray, out var hit, 100, unitLayer);
      return (isHit, hit);
    }

    public void Tick() {
      if (!Input.GetMouseButtonDown(0)) return;
      
      var ray = camera.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out _, 100, unitLayer)) return;
      if (UIExt.IsPointerOverUIElement()) return;
        
      if (Physics.Raycast(ray, out _, 100, globalLayer)) {
        controller.Hide();
      }
    }

    public (bool isHit, Vector3 position) RaycastPlane() {
      var plane = new Plane(Vector3.up, new Vector3(0, 0, 0));
      var ray = camera.ScreenPointToRay(Input.mousePosition);

      var isHit = plane.Raycast(ray, out var enter);
      var position = ray.GetPoint(enter);
      
      return (isHit, position);
    }

    readonly UnitTooltipController controller;
    readonly Camera camera;
    readonly int globalLayer;
    readonly int unitLayer;
    static readonly Logger log = MainLog.GetLogger(nameof(RaycastController));
  }
}