using System.Collections;
using UnityEngine;
using View.Exts;

namespace View {
  public class MoveRoutine {
    public MoveRoutine(UnitView fromUnit, TileView fromTile, TileView toTile, float time, MonoBehaviour obj) {
      this.time = time;
      endTime = Time.realtimeSinceStartup + time;
      this.fromUnit = fromUnit;
      this.fromTile = fromTile;
      this.toTile = toTile;
      obj.StartCoroutine(MoveCoroutine());
    }
    
    IEnumerator MoveCoroutine() {
      var height = fromUnit.Height;
      var fromPosition = fromTile.transform.position.WithY(height);
      var toPosition = toTile.transform.position.WithY(height);
      
      while (Time.realtimeSinceStartup < endTime) {
        var t = time - (endTime - Time.realtimeSinceStartup) / time;
        fromUnit.transform.position = Vector3.Lerp(fromPosition, toPosition, t);

        yield return null;
      }
    }
    
    readonly float time, endTime;
    readonly UnitView fromUnit;
    readonly TileView fromTile, toTile;
  }
}