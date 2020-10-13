namespace Controller.Test {
  public struct Entry<TValue>
  {
    public int[] Keys;
    public TValue[] Values;
    public Entry(int count)
    {
      Keys = new int[count];
      Values = new TValue[count];
    }
    public bool TryGetValue(int key, out TValue value)
    {
      for (int j = 0; j < Keys.Length; ++j)
      {
        if (Keys[j] == key)
        {
          value = Values[j];
          return true;
        }
      }
      value = default(TValue);
      return false;
    }
  }
}