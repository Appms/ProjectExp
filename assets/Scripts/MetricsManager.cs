using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MetricsManager
{
	public static float MetricScore  = 0.5f;

	public class AvarageManager
	{
		private List<float> _values = new List<float>();
		
		public float Avarage
		{
			get
			{
				float result = 0.0f;

				foreach (float fl in _values)
				{
					result += fl;
				}

				return result / _values.Count;
			}
		}

		public void AddValue(float pValue)
		{
			_values.Add(pValue);
		}
	}

	public class CounterManager
	{
		private int _counter;

		public int Counter
		{
			get { return _counter; }
		}

		public void Add(int pAmount = 1)
		{
			_counter += pAmount;
		}

		public void Reset()
		{
			_counter = 0;
		}
	}

	public class RatioManager
	{
		private int _firstCount;
		private int _secondCount;

		public float Ratio
		{
			get
			{
				return _firstCount / _secondCount;
			}
		}

		public void AddFirst(int pAmount = 1)
		{
			_firstCount += pAmount;
		}

		public void AddSecond(int pAmount = 1)
		{
			_secondCount += pAmount;
		}

		public void Reset()
		{
			_firstCount = 0;
			_secondCount = 0;
		}
	}
}
