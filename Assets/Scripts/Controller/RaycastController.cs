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