namespace MonsterLove.Collections {
  public class ObjectPoolContainer<T> {
    public T Item { get; set; }
    public bool Used { get; private set; }

    public void Consume() => Used = true;
    public void Release() => Used = false;
  }
}