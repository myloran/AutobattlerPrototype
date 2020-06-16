using UnityEngine;

namespace View.Views {
  public class TileView : MonoBehaviour {
    void Start() {
      var meshRenderer = GetComponent<MeshRenderer>();
      meshRenderer.material = material = new Material(meshRenderer.material);
    }

    public void Highlight() => material.color = Color.green;
    public void Unhighlight() => material.color = Color.white;
    public void Debug(Color color) => material.color = color; 
  
    Material material;
  }
}
