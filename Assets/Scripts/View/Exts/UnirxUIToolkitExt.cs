using System;
using UniRx;
using UnityEngine.UIElements;

namespace View.Exts {
  public static class UnirxUIToolkitExt {
    // public static void Sub(this Button button, Action action) =>
    //   button.onClick.AsObservable().Subscribe(_ => action()).AddTo(button);
    //
    // public static IObservable<Unit> AsObservable(this UnityEngine.Events.UnityEvent unityEvent)
    // {
    //   return Observable.FromEvent<UnityAction>(h => new UnityAction(h), h => unityEvent.AddListener(h), h => unityEvent.RemoveListener(h));
    // }
  }
}