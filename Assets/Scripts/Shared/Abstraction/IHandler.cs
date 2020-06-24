namespace Shared.Abstraction {
  public interface IHandler<in T> {
    void Handle(T e);
  }
}