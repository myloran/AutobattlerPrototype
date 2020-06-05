using System.Collections;
using System.Collections.Generic;
using Shared;

namespace View.Presenters {
  public abstract class BaseUnitDict<T> : IUnitDict<T> {
    public virtual T this[Coord coord] {
      get => Units[coord];
      set => Units[coord] = value;
    }

    public IEnumerable<T> Values => Units.Values;
    public int Count => Units.Count;

    IEnumerator<KeyValuePair<Coord, T>> IEnumerable<KeyValuePair<Coord, T>>.GetEnumerator() => Units.GetEnumerator();

    public IEnumerator GetEnumerator() => Units.GetEnumerator();
    public bool Contains(Coord coord) => Units.ContainsKey(coord);
    public void Remove(Coord coord) => Units.Remove(coord);

    public void Instantiate(string name, Coord coord, EPlayer player) => 
      Units[coord] = Create(name, coord, player);

    public void Destroy(Coord coord) {
      Remove(Units[coord]);
      Units.Remove(coord);
    } 

    public (bool, Coord) InstantiateToStart(string name, EPlayer player) {
      for (int x = 0; x < 10; x++) {
        var coord = new Coord(x, -1);
        if (Units.ContainsKey(coord)) continue;

        Units[coord] = Create(name, coord, player);
        return (true, new Coord(x, -1));
      }

      return (false, default);
    }
    
    public Coord DestroyFromEnd() {
      for (int x = 9; x >= 0; x--) {
        var coord = new Coord(x, -1);
        if (!Units.ContainsKey(coord)) continue;
        
        Remove(Units[coord]);
        Units.Remove(coord);
        return coord;
      }
      return Coord.Invalid;
    }
    
    public void Clear() {
      foreach (var unit in Units.Values) {
        Remove(unit);
      }
      Units.Clear();
    }

    protected abstract T Create(string name, Coord coord, EPlayer player);
    protected virtual void Remove(T unit) {}
    
    protected readonly Dictionary<Coord, T> Units = new Dictionary<Coord, T>(10);
  }
}