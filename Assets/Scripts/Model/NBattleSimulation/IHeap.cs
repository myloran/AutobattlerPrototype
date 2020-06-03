using System.Collections.Generic;
using Model.NBattleSimulation.Commands;

namespace Model.NBattleSimulation {
  public interface IHeap<TKey, TValue> {
    KeyValuePair<TKey, TValue> Min();
    void RemoveMin();
    ICommand this[float time] { set; }
  }
}