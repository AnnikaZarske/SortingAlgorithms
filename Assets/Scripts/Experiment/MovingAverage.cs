using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastMovingAverageDouble
{
    // https://stackoverflow.com/questions/12884600/how-to-calculate-simple-moving-average-faster-in-c/41722174#41722174
    /// <summary>
    /// Adjust this as you see fit
    /// </summary>
    const int RecalculateEveryXValues = 60;

    /// <summary>
    /// Initializes moving average for specified window size
    /// </summary>
    /// <param name="_WindowSize">Size of moving average window between 2 and MaximumWindowSize
    /// Note: this value should not be too large and also bear in mind the possibility of overflow and floating point error as this class internally keeps a sum of the values within the window</param>
    public FastMovingAverageDouble(int _WindowSize)
    {
        if (_WindowSize < 2)
        {
            _WindowSize = 2;
        }
        m_WindowSize = _WindowSize;
    }
    private object SyncRoot = new object();
    private Queue<double> Buffer = new Queue<double>();
    private int m_WindowSize;
    private double m_MovingAverage = 0d;
    private double MovingSum = 0d;
    private bool BufferFull;
    private int Counter = 0;

    /// <summary>
    /// Calculated moving average
    /// </summary>
    public double MovingAverage
    {
        get
        {
            lock (SyncRoot)
            {
                return m_MovingAverage;
            }
        }
    }

    /// <summary>
    /// Size of moving average window set by constructor during intialization
    /// </summary>
    public int WindowSize
    {
        get
        {
            return m_WindowSize;
        }
    }

    /// <summary>
    /// Add new value to sequence and recalculate moving average seee <see cref="MovingAverage"/>
    /// </summary>
    /// <param name="NewValue">New value to be added</param>
    public void AddValue(double NewValue)
    {
        lock (SyncRoot)
        {
            Buffer.Enqueue(NewValue);
            MovingSum += NewValue;
            if (!BufferFull)
            {
                int BufferSize = Buffer.Count;
                BufferFull = BufferSize == WindowSize;
                m_MovingAverage = MovingSum / BufferSize;
            }
            else
            {
                Counter += 1;
                if (Counter > RecalculateEveryXValues)
                {
                    MovingSum = 0;
                    foreach (double BufferValue in Buffer)
                    {
                        MovingSum += BufferValue;
                    }
                    Counter = 0;
                }
                MovingSum -= Buffer.Dequeue();
                m_MovingAverage = MovingSum / WindowSize;
            }
        }
    }
}
