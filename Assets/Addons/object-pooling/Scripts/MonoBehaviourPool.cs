// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using Object = UnityEngine.Object;
//
// namespace MonsterLove.Collections {
//   public class MonoBehaviourPool {
//     public void warmPool(GameObject prefab, int size) {
//       if (prefabLookup.ContainsKey(prefab)) throw new Exception("Pool for prefab " + prefab.name + " has already been created");
//
//       var pool = new ObjectPool<GameObject>(() => { return InstantiatePrefab(prefab); }, size);
//       prefabLookup[prefab] = pool;
//     }
//
//     public GameObject spawnObject(GameObject prefab) {
//       return spawnObject(prefab, Vector3.zero, Quaternion.identity);
//     }
//
//     public GameObject spawnObject(GameObject prefab, Vector3 position, Quaternion rotation) {
//       if (!prefabLookup.ContainsKey(prefab)) WarmPool(prefab, 1);
//
//       var pool = prefabLookup[prefab];
//
//       var clone = pool.GetItem();
//       clone.transform.position = position;
//       clone.transform.rotation = rotation;
//       clone.SetActive(true);
//
//       instanceLookup.Add(clone, pool);
//       return clone;
//     }
//
//     public void releaseObject(GameObject clone) {
//       clone.SetActive(false);
//
//       if (instanceLookup.ContainsKey(clone)) {
//         instanceLookup[clone].ReleaseItem(clone);
//         instanceLookup.Remove(clone);
//       }
//       else {
//         Debug.LogWarning("No pool contains the object: " + clone.name);
//       }
//     }
//
//
//     GameObject InstantiatePrefab(GameObject prefab) {
//       var go = Object.Instantiate(prefab) as GameObject;
//       if (root != null) go.transform.parent = root;
//       return go;
//     }
//
//     public void PrintStatus() {
//       foreach (var keyVal in prefabLookup)
//         Debug.Log(string.Format("Object Pool for Prefab: {0} In Use: {1} Total {2}", keyVal.Key.name,
//           keyVal.Value.CountUsedItems, keyVal.Value.Count));
//     }
//
//
//     public static void WarmPool(GameObject prefab, int size) {
//       Instance.warmPool(prefab, size);
//     }
//
//     public static GameObject SpawnObject(GameObject prefab) {
//       return Instance.spawnObject(prefab);
//     }
//
//     public static GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation) {
//       return Instance.spawnObject(prefab, position, rotation);
//     }
//
//     public static void ReleaseObject(GameObject clone) {
//       Instance.releaseObject(clone);
//     }
//     
//     readonly Dictionary<GameObject, ObjectPool<GameObject>> prefabLookup =
//       new Dictionary<GameObject, ObjectPool<GameObject>>();
//
//     readonly Dictionary<GameObject, ObjectPool<GameObject>> instanceLookup =
//       new Dictionary<GameObject, ObjectPool<GameObject>>();
//     
//     Transform root;
//   }
// }