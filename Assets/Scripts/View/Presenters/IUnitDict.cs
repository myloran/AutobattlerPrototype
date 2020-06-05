using System.Collections.Generic;
using Shared;

namespace View.Presenters {
  public interface IUnitDict<T> : IEnumerable<KeyValuePair<Coord, T>> {
    T this[Coord coord] { get; set; }
    IEnumerable<T> Values { get; }
    int Count { get; }
    void Remove(Coord coord);
    
    bool Contains(Coord coord);
    void Instantiate(string name, Coord coord, EPlayer player);
    void Destroy(Coord coord);
    (bool, Coord) InstantiateToStart(string name, EPlayer player);
    Coord DestroyFromEnd(); //TODO: change to void
    void Clear();
  }
}