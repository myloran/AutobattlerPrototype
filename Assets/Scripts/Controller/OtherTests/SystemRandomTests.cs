using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Debug = UnityEngine.Debug;
using Random = System.Random;

namespace Controller.NDebug {
  public class SystemRandomTests {
    public void ExecuteTests() {
      if (isOk) return;
      isOk = true;

      TestOldVersusNew();
      TestReset();
      
      Stopwatch stopWatch = new Stopwatch();
      stopWatch.Start();
      BenchmarkOld();
      stopWatch.Stop();
      Debug.Log($"Before.stopWatch.ElapsedMilliseconds: {stopWatch.ElapsedMilliseconds}");
      
      Stopwatch stopWatch2 = new Stopwatch();
      stopWatch2.Start();
      BenchmarkNew();
      stopWatch2.Stop();
      Debug.Log($"After.stopWatch.ElapsedMilliseconds: {stopWatch2.ElapsedMilliseconds}");
    }

    void TestOldVersusNew() {
      TestOld();
      TestNew();
    }
    
    void TestReset() {
      random.Reset(0);
      TestOld();
      random.Reset(0);
      TestOldReset();
    }

    void TestOld() {
      randomNumbers.Clear();
      incorrectNumbers = 0;
      int min = int.MaxValue; 
      int max = int.MinValue;
      
      for (int i = 0; i < MaxIterations; i++) {
        var num = random.Next(MaxValue);
        randomNumbers.Add(num);
        max = Math.Max(max, num);
        min = Math.Min(min, num);
      }

      Debug.Log($"numbers: {string.Join(",", randomNumbers)}");
      Debug.Log($"min: {min}");
      Debug.Log($"max: {max}");
    }    
    
    void TestOldReset() {
      int min = int.MaxValue; 
      int max = int.MinValue;
      
      for (int i = 0; i < MaxIterations; i++) {
        var num = random.Next(MaxValue);
        if (randomNumbers[i] != num) incorrectNumbers++;
        max = Math.Max(max, num);
        min = Math.Min(min, num);
      }

      Debug.Log($"min: {min}");
      Debug.Log($"max: {max}");
      Debug.Log($"incorrectNumbers when comparing old implementation before and after reset: {incorrectNumbers}");
    }  
    
    void TestNew() {
      int min = int.MaxValue;
      int max = int.MinValue;
      
      for (int i = 0; i < MaxIterations; i++) {
        var num = r2.Next(MaxValue);
        if (randomNumbers[i] != num) incorrectNumbers++;
        max = Math.Max(max, num);
        min = Math.Min(min, num);
      }

      Debug.Log($"min: {min}");
      Debug.Log($"max: {max}");
      Debug.Log($"incorrectNumbers when new implementation is compared to an old one: {incorrectNumbers}");
    }
    
    void BenchmarkOld() {
      for (int i = 0; i < MaxIterations; i++) random.Next(MaxValue);
    }    
    
    void BenchmarkNew() {
      for (int i = 0; i < MaxIterations; i++) r2.Next(MaxValue);
    }

    Random r2 = new Random(0);
    readonly SystemRandomEmbedded random = new SystemRandomEmbedded(0);
    readonly List<int> randomNumbers = new List<int>();
    const int MaxValue = 32000;
    const int MaxIterations = 10_000_000;
    int incorrectNumbers;
    bool isOk;
  }
}