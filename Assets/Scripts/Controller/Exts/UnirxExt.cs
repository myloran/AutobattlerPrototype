using System;
using UniRx;
using UnityEngine.UI;

namespace Controller.Exts {
  public static class UnirxExt {
    public static void Sub(this Button button, Action action) =>
      button.onClick.AsObservable().Subscribe(_ => action()).AddTo(button);
    
    public static IDisposable Subscribe<T>(this IObservable<T> self, Action action) => 
      self.Subscribe(_ => action());
    
    public static IObservable<R> SelectWhere<T, R>(this IObservable<T> self, 
      Func<T, (bool isOk, R r)> toTuple) =>
      self.Select(toTuple)
        .Where(tuple => tuple.isOk)
        .Select(t => t.r);

    public static IObservable<R> SelectWhere<T, R>(this IObservable<T> self, 
      Func<(bool, R)> toTuple) =>
      self.Select(_ => toTuple())
        .Where(tuple => tuple.Item1)
        .Select(t => t.Item2);

    public static IObservable<R> Select<T, R>(this IObservable<T> self, Func<R> getR) =>
      self.Select(_ => getR());
    
    public static IObservable<T> Do<T>(this IObservable<T> self, Action action) =>
      self.Select(t => {
        action();
        return t;
      });
    
    public static IObservable<T> Where<T>(this IObservable<T> self, Func<bool> predicate) =>
      self.Where(_ => predicate());
        
    public static IObservable<T> Where<T, R>(this IObservable<T> self, 
      Func<T, (bool isOk, R r)> toTuple) => self
        .SelectMany(t => Observable.Return(t)
          .Select(toTuple)
          .Where(tuple => tuple.isOk)
          .Select(tuple => t));

    public static IObservable<T> Connect<T>(this IObservable<T> self,
        CompositeDisposable disposable) {
      var connectedObservable = self.Publish();
      connectedObservable.Connect().AddTo(disposable);
      return connectedObservable;
    } 
  }
}