namespace Controller.UnitDrag {
  public interface IPredicate<in T> {
    bool Check(T t);
  }
}