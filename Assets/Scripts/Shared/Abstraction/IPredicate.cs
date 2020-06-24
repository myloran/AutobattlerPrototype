namespace Shared.Abstraction {
  public interface IPredicate<in T> {
    bool Check(T t);
  }
}