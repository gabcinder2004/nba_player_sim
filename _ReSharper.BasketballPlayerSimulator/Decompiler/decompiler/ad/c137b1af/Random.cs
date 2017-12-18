// Type: System.Random
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System.Runtime;
using System.Runtime.InteropServices;

namespace System
{
  /// <summary>
  /// Represents a pseudo-random number generator, a device that produces a sequence of numbers that meet certain statistical requirements for randomness.
  /// </summary>
  /// <filterpriority>1</filterpriority>
  [ComVisible(true)]
  [__DynamicallyInvokable]
  [Serializable]
  public class Random
  {
    private int[] SeedArray = new int[56];
    private int inext;
    private int inextp;
    private const int MBIG = 2147483647;
    private const int MSEED = 161803398;
    private const int MZ = 0;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Random"/> class, using a time-dependent default seed value.
    /// </summary>
    [__DynamicallyInvokable]
    public Random()
      : this(Environment.TickCount)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:System.Random"/> class, using the specified seed value.
    /// </summary>
    /// <param name="Seed">A number used to calculate a starting value for the pseudo-random number sequence. If a negative number is specified, the absolute value of the number is used. </param>
    [__DynamicallyInvokable]
    public Random(int Seed)
    {
      int num1 = 161803398 - (Seed == int.MinValue ? int.MaxValue : Math.Abs(Seed));
      this.SeedArray[55] = num1;
      int num2 = 1;
      for (int index1 = 1; index1 < 55; ++index1)
      {
        int index2 = 21 * index1 % 55;
        this.SeedArray[index2] = num2;
        num2 = num1 - num2;
        if (num2 < 0)
          num2 += int.MaxValue;
        num1 = this.SeedArray[index2];
      }
      for (int index1 = 1; index1 < 5; ++index1)
      {
        for (int index2 = 1; index2 < 56; ++index2)
        {
          this.SeedArray[index2] -= this.SeedArray[1 + (index2 + 30) % 55];
          if (this.SeedArray[index2] < 0)
            this.SeedArray[index2] += int.MaxValue;
        }
      }
      this.inext = 0;
      this.inextp = 21;
      Seed = 1;
    }

    /// <summary>
    /// Returns a random number between 0.0 and 1.0.
    /// </summary>
    /// 
    /// <returns>
    /// A double-precision floating point number greater than or equal to 0.0, and less than 1.0.
    /// </returns>
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    [__DynamicallyInvokable]
    protected virtual double Sample()
    {
      return (double) this.InternalSample() * 4.6566128752458E-10;
    }

    private int InternalSample()
    {
      int num1 = this.inext;
      int num2 = this.inextp;
      int index1;
      if ((index1 = num1 + 1) >= 56)
        index1 = 1;
      int index2;
      if ((index2 = num2 + 1) >= 56)
        index2 = 1;
      int num3 = this.SeedArray[index1] - this.SeedArray[index2];
      if (num3 == int.MaxValue)
        --num3;
      if (num3 < 0)
        num3 += int.MaxValue;
      this.SeedArray[index1] = num3;
      this.inext = index1;
      this.inextp = index2;
      return num3;
    }

    /// <summary>
    /// Returns a nonnegative random number.
    /// </summary>
    /// 
    /// <returns>
    /// A 32-bit signed integer greater than or equal to zero and less than <see cref="F:System.Int32.MaxValue"/>.
    /// </returns>
    /// <filterpriority>1</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    [__DynamicallyInvokable]
    public virtual int Next()
    {
      return this.InternalSample();
    }

    /// <summary>
    /// Returns a random number within a specified range.
    /// </summary>
    /// 
    /// <returns>
    /// A 32-bit signed integer greater than or equal to <paramref name="minValue"/> and less than <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/> but not <paramref name="maxValue"/>. If <paramref name="minValue"/> equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.
    /// </returns>
    /// <param name="minValue">The inclusive lower bound of the random number returned. </param><param name="maxValue">The exclusive upper bound of the random number returned. <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>. </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="minValue"/> is greater than <paramref name="maxValue"/>. </exception><filterpriority>1</filterpriority>
    [__DynamicallyInvokable]
    public virtual int Next(int minValue, int maxValue)
    {
      if (minValue > maxValue)
      {
        throw new ArgumentOutOfRangeException("minValue", Environment.GetResourceString("Argument_MinMaxValue", (object) "minValue", (object) "maxValue"));
      }
      else
      {
        long num = (long) maxValue - (long) minValue;
        if (num <= (long) int.MaxValue)
          return (int) (this.Sample() * (double) num) + minValue;
        else
          return (int) ((long) (this.GetSampleForLargeRange() * (double) num) + (long) minValue);
      }
    }

    /// <summary>
    /// Returns a nonnegative random number less than the specified maximum.
    /// </summary>
    /// 
    /// <returns>
    /// A 32-bit signed integer greater than or equal to zero, and less than <paramref name="maxValue"/>; that is, the range of return values ordinarily includes zero but not <paramref name="maxValue"/>. However, if <paramref name="maxValue"/> equals zero, <paramref name="maxValue"/> is returned.
    /// </returns>
    /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue"/> must be greater than or equal to zero. </param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="maxValue"/> is less than zero. </exception><filterpriority>1</filterpriority>
    [__DynamicallyInvokable]
    public virtual int Next(int maxValue)
    {
      if (maxValue >= 0)
        return (int) (this.Sample() * (double) maxValue);
      throw new ArgumentOutOfRangeException("maxValue", Environment.GetResourceString("ArgumentOutOfRange_MustBePositive", new object[1]
      {
        (object) "maxValue"
      }));
    }

    /// <summary>
    /// Returns a random number between 0.0 and 1.0.
    /// </summary>
    /// 
    /// <returns>
    /// A double-precision floating point number greater than or equal to 0.0, and less than 1.0.
    /// </returns>
    /// <filterpriority>1</filterpriority>
    [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
    [__DynamicallyInvokable]
    public virtual double NextDouble()
    {
      return this.Sample();
    }

    /// <summary>
    /// Fills the elements of a specified array of bytes with random numbers.
    /// </summary>
    /// <param name="buffer">An array of bytes to contain random numbers. </param><exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> is null. </exception><filterpriority>1</filterpriority>
    [__DynamicallyInvokable]
    public virtual void NextBytes(byte[] buffer)
    {
      if (buffer == null)
        throw new ArgumentNullException("buffer");
      for (int index = 0; index < buffer.Length; ++index)
        buffer[index] = (byte) (this.InternalSample() % 256);
    }

    private double GetSampleForLargeRange()
    {
      int num = this.InternalSample();
      if (this.InternalSample() % 2 == 0)
        num = -num;
      return ((double) num + 2147483646.0) / 4294967293.0;
    }
  }
}
