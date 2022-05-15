using System.Collections.Generic;
using Shared.Primitives;

namespace Shared.Abstraction {
  public interface IInfoGetter<T> where T : IInfo {
    Dictionary<string, T> Infos { get; }
  }
}