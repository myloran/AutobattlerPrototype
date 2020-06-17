using Shared.OkwyLogging;
using UnityEngine;

namespace Controller.NDebug {
  public class DebugController : MonoBehaviour {
    public DebugInfo Info;

    void Awake() {
      wasLogEnabled = Info.IsLogEnabled;
      CheckLog();
    }

    void Update() {
      if (Info.IsLogEnabled == wasLogEnabled) return;
      
      wasLogEnabled = Info.IsLogEnabled;
      CheckLog();
    }

    void CheckLog() {
      if (Info.IsLogEnabled)
        MainLog.DefaultInit();
      else
        MainLog.ResetAppenders();
    }

    bool wasLogEnabled;
  }
}