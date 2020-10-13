using System.Collections.Generic;
using System.Diagnostics;
using Model.NUnit;

namespace Controller.Test {
  public class TestEntryPerformance {
    public void Test() {
      var loopIterations = 10;
      var executionTimes = 1_000_000;
      
      for (int j = 0; j < loopIterations; j++) {
        var maxElements = (j+1) * 5;
        var indexToTest = maxElements / 2;
        
        var unitDict = new Dictionary<int, int>();
        var unitEntries = new Entry<int>(maxElements);
      
        for (int i = 0; i < maxElements; i++) {
          unitDict[i] = i;
          unitEntries.Keys[i] = i;
          unitEntries.Values[i] = i;
        }
      
        var watch = new Stopwatch();
        watch.Start();
        for (int i = 0; i < executionTimes; i++) {
          unitDict.TryGetValue(indexToTest, out _);
        }
        watch.Stop();
        UnityEngine.Debug.Log("dict: " + watch.ElapsedMilliseconds);
      
        watch.Reset();
        watch.Start();
        for (int i = 0; i < executionTimes; i++) {
          unitEntries.TryGetValue(indexToTest, out _);
        }
      
        watch.Stop();
        UnityEngine.Debug.Log("entry" + watch.ElapsedMilliseconds);
      }
    }
  }
}