using System.Collections.Generic;

namespace Shared.Exts {
  public static class EnumerableExt {
    public static IEnumerable<T> AsEnumerable<T>(this T item) {
      yield return item;
    }
  }
}