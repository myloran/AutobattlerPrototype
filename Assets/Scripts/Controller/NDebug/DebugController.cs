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
      if (Info.IsLogEnabled != wasLogEnabled) {
        wasLogEnabled = Info.IsLogEnabled;
        CheckLog();
      }
      
      // if (Info.IsDebugOn != wasDebugOn) {
      //   wasDebugOn = Info.IsDebugOn;
      //   CheckLog();
      // }
    }

    void CheckLog() {
      if (Info.IsLogEnabled)
        MainLog.DefaultInit();
      else
        MainLog.ResetAppenders();
    }

    bool wasLogEnabled,
      wasDebugOn;
  }
}