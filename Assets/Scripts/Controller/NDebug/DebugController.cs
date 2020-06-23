using Controller.Update;
using Infrastructure.OkwyLoggingUnity;
using Shared.Addons.OkwyLogging;
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
      CheckDebug();
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

    void CheckDebug() => unitTooltipUICanvas.enabled = !Info.IsDebugOn;

    void CheckLog() {
      if (Info.IsLogEnabled)
        OkwyDefaultLog.DefaultInit();
      else
        MainLog.ResetAppenders();
    }

    Canvas unitTooltipUICanvas;
  }
}