using System.Collections.Generic;

namespace Shared.Exts {
  public static class DictionaryExt {
    public static Dictionary<T, R> With<T, R>(this Dictionary<T, R> first, 
        Dictionary<T, R> second) {
      var units = new Dictionary<T, R>();
      
      foreach (var (coord, unit) in first) units[coord] = unit;
      foreach (var (coord, unit) in second) units[coord] = unit;
      
      return units;  
    }
  }
}