namespace Controller.UnitDrag {
  public interface IHandler<in T> {
    void Handle(T e);
  }
}