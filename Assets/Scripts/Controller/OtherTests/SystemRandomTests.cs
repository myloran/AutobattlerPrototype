using System;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Random = System.Random;

namespace Controller.NDebug {
  public class SystemRandomTests {
    public void ExecuteTests() {
      if (isOk) return;
      isOk = true;

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
      
      TestOld();
      TestNew();
    }

    void TestOld() {
      int min = int.MaxValue, max = int.MinValue;
      for (int i = 0; i < maxIterations; i++) {
        var num = random.Next(maxValue);
        randomNumbers.Add(num);
        max = Math.Max(max, num);
        min = Math.Min(min, num);
      }

      Debug.Log($"min: {min}");
      Debug.Log($"max: {max}");
    }    
    
    void TestNew() {
      int min = int.MaxValue, max = int.MinValue;
      for (int i = 0; i < maxIterations; i++) {
        var num = r2.Next(maxValue);
        if (randomNumbers[i] != num) incorrectNumbers++;
        max = Math.Max(max, num);
        min = Math.Min(min, num);
      }

      Debug.Log($"min: {min}");
      Debug.Log($"max: {max}");
      Debug.Log($"incorrectNumbers: {incorrectNumbers}");
    }
    
    void BenchmarkOld() {
      for (int i = 0; i < maxIterations; i++) random.Next(maxValue);
    }    
    
    void BenchmarkNew() {
      for (int i = 0; i < maxIterations; i++) r2.Next(maxValue);
    }

    readonly Random r2 = new Random(0);
    readonly SystemRandom random = new SystemRandom(0);
    readonly List<int> randomNumbers = new List<int>();
    const int maxValue = 32000;
    const int maxIterations = 10_000_000;
    int incorrectNumbers;
    bool isOk;
  }
}