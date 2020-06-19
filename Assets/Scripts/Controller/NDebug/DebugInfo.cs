using UnityEngine;

namespace Controller.NDebug {
  [CreateAssetMenu(fileName = "DebugInfo", menuName = "Data/Debug", order = 1)]
  public class DebugInfo : ScriptableObject {
    public bool IsLogEnabled,
      IsDebugOn;
  }
}