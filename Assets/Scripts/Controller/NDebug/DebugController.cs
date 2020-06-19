using Controller.Update;
using Shared.OkwyLogging;
using UnityEngine;
using View.UIs;

namespace Controller.NDebug {
  public class DebugController : MonoBehaviour, ITick {
    public DebugInfo Info;
    public bool WasLogEnabled { get; private set; }
    public bool WasDebugOn { get; private set; }

    public void Init(UnitTooltipUI unitTooltipUI) {
      unitTooltipUICanvas = unitTooltipUI.GetComponent<Canvas>();
      WasLogEnabled = Info.IsLogEnabled;
      CheckLog();
    }

    public void Tick() {
      if (Info.IsLogEnabled != WasLogEnabled) {
        WasLogEnabled = Info.IsLogEnabled;
        CheckLog();
      }
      
      if (Info.IsDebugOn != WasDebugOn) {
        WasDebugOn = Info.IsDebugOn;
        CheckDebug();
      }
    }

    void CheckDebug() {
      if (Info.IsDebugOn) {
        unitTooltipUICanvas.enabled = false;
      }
      else {
        unitTooltipUICanvas.enabled = true;
      }
    }

    void CheckLog() {
      if (Info.IsLogEnabled)
        MainLog.DefaultInit();
      else
        MainLog.ResetAppenders();
    }

    Canvas unitTooltipUICanvas;
  }
}