using UnityEngine;
using UnityEngine.UI;

namespace View.NUnit.UI {
  public class SilenceCross : MonoBehaviour {
    public SilenceCross Init(Camera mainCamera) {
      this.mainCamera = mainCamera;
      canvas = GetComponent<Canvas>();
      canvas.enabled = false;
      return this;
    }
    
    void Update() => UpdateRotation();
    public void Show() => canvas.enabled = true;
    public void Hide() => canvas.enabled = false;

    void UpdateRotation() {
      var position = transform.position;
      var targetPosition = mainCamera.transform.position; 
      var target = new Vector3(position.x, -targetPosition.y, -targetPosition.z);
      transform.LookAt(target);
    }
    
    Image leftBar;
    Image rightBar;
    Camera mainCamera;
    Canvas canvas;
  }
}