using System.Collections.Generic;

namespace Shared {
  public interface IUnitDict<T> : IEnumerable<KeyValuePair<Coord, T>> {
    Dictionary<Coord, T> Units { get; }
    T this[Coord coord] { get; set; }
    IEnumerable<T> Values { get; }
    int Count { get; }
    void Remove(Coord coord);
    
    bool Has(Coord coord);
    void Instantiate(string name, Coord coord, EPlayer player);
    void Destroy(Coord coord);
    (bool, Coord) InstantiateToStart(string name, EPlayer player);
    Coord DestroyFromEnd(EPlayer player); //TODO: change to void
    void DestroyAll();
  }
}