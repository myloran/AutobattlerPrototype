using System;
using UniRx;
using UnityEngine.UI;

namespace Controller {
  public static class UnirxExt {
    public static void Sub(this Button button, Action action) =>
      button.onClick.AsObservable().Subscribe(_ => action()).AddTo(button);
  }
}