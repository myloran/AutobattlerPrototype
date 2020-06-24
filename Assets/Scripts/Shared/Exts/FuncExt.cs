using System;

namespace Shared.Exts {
  public static class FuncExt {
    public static Func<T, bool> Negate<T>(this Func<T, bool> self) => t => !self(t);
  }
}