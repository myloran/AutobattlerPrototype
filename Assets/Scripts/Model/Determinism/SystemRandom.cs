using System;
using Shared.Addons.Examples.FixMath;

//It's embedded version of System.Random
//It's ~3.5 times slower, supports max range up to 32000, has reduced accuracy
//But it should be deterministic across platforms and supports FixedMath 
public class SystemRandom {
  int[] SeedArray = new int[56];
  const int MBIG = 2147483647;
  const int MSEED = 161803398;
  const int MZ = 0;
  int inext;
  int inextp;

  public SystemRandom()
    : this(Environment.TickCount) { }

  public SystemRandom(int Seed) {
    var num1 = 161803398 - (Seed == int.MinValue ? int.MaxValue : Math.Abs(Seed));
    SeedArray[55] = num1;
    var num2 = 1;
    for (var index1 = 1; index1 < 55; ++index1) {
      var index2 = 21 * index1 % 55;
      SeedArray[index2] = num2;
      num2 = num1 - num2;
      if (num2 < 0)
        num2 += int.MaxValue;
      num1 = SeedArray[index2];
    }

    for (var index1 = 1; index1 < 5; ++index1)
    for (var index2 = 1; index2 < 56; ++index2) {
      SeedArray[index2] -= SeedArray[1 + (index2 + 30) % 55];
      if (SeedArray[index2] < 0)
        SeedArray[index2] += int.MaxValue;
    }

    inext = 0;
    inextp = 21;
    Seed = 1;
  }

  protected virtual F64 Sample() {
    return new F64(InternalSample()) * new F64(4.6566128752458E-10);
  }

  int InternalSample() {
    var inext = this.inext;
    var inextp = this.inextp;
    int index1;
    if ((index1 = inext + 1) >= 56)
      index1 = 1;
    int index2;
    if ((index2 = inextp + 1) >= 56)
      index2 = 1;
    var num = SeedArray[index1] - SeedArray[index2];
    if (num == int.MaxValue)
      --num;
    if (num < 0)
      num += int.MaxValue;
    SeedArray[index1] = num;
    this.inext = index1;
    this.inextp = index2;
    return num;
  }

  public virtual int Next() {
    return InternalSample();
  }

  double GetSampleForLargeRange() {
    var num = InternalSample();
    if (InternalSample() % 2 == 0)
      num = -num;
    return ((double) num + 2147483646.0) / 4294967293.0;
  }
  //
  // public virtual int Next(int minValue, int maxValue)
  // {
  //   if (minValue > maxValue)
  //     throw new ArgumentOutOfRangeException(nameof (minValue), $"{nameof(maxValue)}: {maxValue}");
  //   
  //   var num = maxValue - minValue;
  //   
  //   return (int) (Sample() * num) + minValue;
  // }

  public virtual int Next(int maxValue) {
    if (maxValue < 0)
      throw new ArgumentOutOfRangeException(nameof(maxValue), $"{nameof(maxValue)}: {maxValue}");
    return F32.FloorToInt((Sample() * maxValue).F32);
  }

  // public virtual double NextDouble()
  // {
  //   return this.Sample();
  // }

  public virtual void NextBytes(byte[] buffer) {
    if (buffer == null)
      throw new ArgumentNullException(nameof(buffer));
    for (var index = 0; index < buffer.Length; ++index)
      buffer[index] = (byte) (InternalSample() % 256);
  }
}