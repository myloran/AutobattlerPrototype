using System.Collections.Generic;
using System.Reflection;
using Shared.Exts;
using UnityEngine.UIElements;

namespace View.UIs {
  public static class UIReferencerExt {
    const int MaxDepth = 10;

    public static void FillReferences(this IUIReferencer iui, VisualElement element) {
      foreach (var field in iui.GetType().GetFields())
        FindReference(iui, field, element.AsEnumerable(), MaxDepth);
    }
    
    public static void FillReferences(this IUIReferencer iui, UIDocument document) {
      foreach (var field in iui.GetType().GetFields())
        FindReference(iui, field, document.rootVisualElement.AsEnumerable(), MaxDepth);
    }

    static void FindReference(IUIReferencer iui, FieldInfo field, IEnumerable<VisualElement> children,
      int maxDepth) {
      if (maxDepth <= 0) return;

      var isFound = LoopChildren(iui, field, children);
      if (isFound) return;

      var newChildren = new List<VisualElement>();
      foreach (var child in children)
        newChildren.AddRange(child.Children());

      FindReference(iui, field, newChildren, maxDepth - 1);

      static bool LoopChildren(IUIReferencer ui, FieldInfo field, IEnumerable<VisualElement> children) {
        foreach (var child in children) {
          if (child.name == field.Name) {
            field.SetValue(ui, child);
            return true;
          }
        }

        return false;
      }
    }
  }
}