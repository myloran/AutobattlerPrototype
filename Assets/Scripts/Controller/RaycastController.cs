using System;
using UniRx;
using UnityEngine;

namespace Controller {
  public class RaycastController : ITick {
    public RaycastController(Camera camera, int globalLayer, int unitLayer, 
        UnitTooltipController controller) {
      this.camera = camera;
      this.globalLayer = globalLayer;
      this.unitLayer = unitLayer;
      this.controller = controller;
    }

    public IObservable<RaycastHit> OnUnitHit =>
      Observable.EveryUpdate()
        .Where(_ => Input.GetMouseButtonDown(0))
        .Select(_ => FireRaycast())
        .Select(CheckHitUnit)
        .Where(_ => _.isHit)
        .Select(_ => _.hit);

    Ray FireRaycast() => camera.ScreenPointToRay(Input.mousePosition);

    (bool isHit, RaycastHit hit) CheckHitUnit(Ray ray) {
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

    readonly UnitTooltipController controller;
    readonly Camera camera;
    readonly int globalLayer;
    readonly int unitLayer;
    static readonly Okwy.Logging.Logger log = Okwy.Logging.MainLog.GetLogger(nameof(RaycastController));
  }
}