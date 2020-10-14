using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterLove.Collections {
  public class ObjectPool<T> {
    public ObjectPool(Func<T> factoryFunc, int initialSize) {
      this.factoryFunc = factoryFunc;

      list = new List<ObjectPoolContainer<T>>(initialSize);
      lookup = new Dictionary<T, ObjectPoolContainer<T>>(initialSize);

      Warm(initialSize);
    }
    
    public int Count => list.Count;
    public int CountUsedItems => lookup.Count;
    

    void Warm(int capacity) {
      for (var i = 0; i < capacity; i++) 
        CreateContainer();
    }

    ObjectPoolContainer<T> CreateContainer() {
      var container = new ObjectPoolContainer<T> {Item = factoryFunc()};
      list.Add(container);
      return container;
    }

    public T GetItem() {
      ObjectPoolContainer<T> container = null;
      
      for (var i = 0; i < list.Count; i++) {
        lastIndex++;
        
        if (lastIndex > list.Count - 1) 
          lastIndex = 0;

        if (list[lastIndex].Used) continue;

        container = list[lastIndex];
        break;
      }

      if (container == null) 
        container = CreateContainer();

      container.Consume();
      lookup[container.Item] = container;
      
      return container.Item;
    }

    public void ReleaseItem(T item) {
      if (lookup.ContainsKey(item)) {
        var container = lookup[item];
        container.Release();
        lookup.Remove(item);
      }
      else Debug.LogWarning("This object pool does not contain the item provided: " + item);
    }

    readonly List<ObjectPoolContainer<T>> list;
    readonly Dictionary<T, ObjectPoolContainer<T>> lookup; //remove dictionary and keep track of index
    readonly Func<T> factoryFunc;
    int lastIndex;
  }
}