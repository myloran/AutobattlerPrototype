using Shared;
using UnityEngine;

namespace View {
  public interface IUnitViewFactory {
    UnitView Create(string name, Coord coord, EPlayer player);
  }
}