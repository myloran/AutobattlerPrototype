using Controller.Exts;
using Controller.Update;
using UnityEngine;
using Logger = Shared.Addons.OkwyLogging.Logger;

namespace Controller {
  public class RaycastController {
    public RaycastController(Camera camera, int globalLayer, int unitLayer) {
      this.camera = camera;
      this.globalLayer = globalLayer;
      this.unitLayer = unitLayer;
    }
    
    public Ray FireRaycast() => camera.ScreenPointToRay(Input.mousePosition);

    public (bool isHit, RaycastHit hit) RaycastHitsUnit(Ray ray) {
      var isHit = Physics.Raycast(ray, out var hit, 100, unitLayer);
      return (isHit, hit);
    }
    
    public (bool isHit, RaycastHit hit) RaycastHitsGlobalCollider(Ray ray) {
      var isHit = Physics.Raycast(ray, out var hit, 100, globalLayer);
      return (isHit, hit);
    }

    public (bool isHit, Vector3 position) RaycastPlane() {
      var plane = new Plane(Vector3.up, new Vector3(0, 0, 0));
      var ray = camera.ScreenPointToRay(Input.mousePosition);

      var isHit = plane.Raycast(ray, out var enter);
      var position = ray.GetPoint(enter);
      
      return (isHit, position);
    }

    readonly Camera camera;
    readonly int globalLayer;
    readonly int unitLayer;
  }
}