using UnityEngine;
using UnityEngine.UIElements;

namespace View.UIToolkit {
  public class CommandDebugWindowUI : MonoBehaviour {
    public UIDocument Document;
    public VisualTreeAsset CommandTemplate,
      CommandRowTemplate,
      EventTemplate;
  }
}