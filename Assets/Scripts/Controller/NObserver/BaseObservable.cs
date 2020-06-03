using System.Collections.Generic;

namespace Controller.NObserver {
  public abstract class BaseObservable<T> : IObservable<T> {
    protected readonly List<IObserver<T>> Observers = new List<IObserver<T>>();
    
    public void Add(IObserver<T> observer) => Observers.Add(observer);
    public void Remove(IObserver<T> observer) => Observers.Remove(observer);
  }
}