using UnityEngine;

namespace Controller {
  public class RaycastController : MonoBehaviour {
    void Awake() {
      cam = Camera.main;
      globalLayer = LayerMask.GetMask("Terrain", "GlobalCollider");
      unitLayer = LayerMask.GetMask("Unit");
    }

    public void Init(UnitTooltipController controller) {
      this.controller = controller;
    }

    void Update() {
      if (Input.GetMouseButtonDown(0)) {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out _, 100, unitLayer)) return;
        if (UIExt.IsPointerOverUIElement()) return;
        
        if (Physics.Raycast(ray, out _, 100, globalLayer)) {
          controller.Hide();
        }
      }
    }
    
    UnitTooltipController controller;
    Camera cam;
    int globalLayer;
    int unitLayer;
    static readonly Okwy.Logging.Logger log = Okwy.Logging.MainLog.GetLogger(nameof(RaycastController));
  }
}