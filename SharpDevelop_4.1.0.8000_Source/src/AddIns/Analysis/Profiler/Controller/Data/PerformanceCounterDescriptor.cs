﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ICSharpCode.Profiler.Controller.Data
{
	/// <summary>
	/// Wraps <see cref="System.Diagnostics.PerformanceCounter" /> to support lazy-loading and easy value collection.
	/// Stores additonal meta data such as min/max allowed value or unit of the values.
	/// </summary>
	[Serializable]
	public class PerformanceCounterDescriptor
	{
		/// <summary>
		/// Gets the category of the performance counter.
		/// </summary>
		public string Category { get; private set; }
		
		/// <summary>
		/// Gets the name of the performance counter.
		/// </summary>
		public string Name { get; private set; }
		
		/// <summary>
		/// Gets the instance (perfmon process id) of the performance counter.
		/// </summary>
		public string Instance { get; private set; }
		
		/// <summary>
		/// Gets the computer the performance counter is executed on.
		/// </summary>
		public string Computer { get; private set; }
		
		/// <summary>
		/// Gets a list of values collected by this performance counter.
		/// </summary>
		public IList<float> Values { get; private set; }
		
		/// <summary>
		/// Gets the minimum allowed value collected by the performance counter.
		/// Returns null if there is no lower bound.
		/// </summary>
		public float? MinValue { get; private set; }
		
		/// <summary>
		/// Gets the maximum allowed value collected by the performance counter.
		/// Returns null if there is no upper bound.
		/// </summary>
		public float? MaxValue { get; private set; }
		
		/// <summary>
		/// Gets a string representation of the unit of the values collected.
		/// </summary>
		public string Unit { get; private set; }
		
		/// <summary>
		/// Gets the format string for display of the collected values.
		/// </summary>
		public string Format { get; private set; }
		
		float defaultValue;
		PerformanceCounter counter;
		
		/// <summary>
		/// Creates a new PerformanceCounterDescriptor.
		/// </summary>
		public PerformanceCounterDescriptor(string category, string name, string instance, string computer,
		                                    float defaultValue, float? minValue, float? maxValue, string unit, string format)
		{
			Category = category;
			Name = name;
			Instance = instance;
			Computer = computer;
			Values = new List<float>();
			this.defaultValue = defaultValue;
			MinValue = minValue;
			MaxValue = maxValue;
			Unit = unit;
			Format = format;
		}
		
		/// <summary>
		/// Creates a new PerformanceCounterDescriptor.
		/// </summary>
		public PerformanceCounterDescriptor(string name, float? minValue, float? maxValue, string unit, string format)
			: this(null, name, null, null, 0, minValue, maxValue, unit, format)
		{
		}
		
		/// <summary>
		/// Returns the perfmon process identifier for a process Id.
		/// If the process is not available (e. g. not running anymore) null is returned.
		/// </summary>
		public static string GetProcessInstanceName(int pid)
		{
			PerformanceCounterCategory cat = new PerformanceCounterCategory("Process");

			string[] instances = cat.GetInstanceNames();
			foreach (string instance in instances) {
				using (PerformanceCounter procIdCounter = new PerformanceCounter("Process", "ID Process", instance, true)) {
					int val = (int)procIdCounter.RawValue;
					if (val == pid)
						return instance;
				}
			}
			
			return null;
		}
		
		/// <summary>
		/// Deletes all collected information.
		/// </summary>
		public void Reset()
		{
			this.Values.Clear();
		}
		
		/// <summary>
		/// Collects a new value. The default value is recorded if any error occurs, while attempting to collect a value.
		/// </summary>
		/// <param name="instanceName"></param>
		public void Collect(string instanceName)
		{
			if (counter == null && Instance != null)
				counter = new PerformanceCounter(Category, Name, instanceName ?? Instance, Computer);
			
			try {
				this.Values.Add(counter.NextValue());
			} catch (Exception e) {
				#if DEBUG
				Console.WriteLine(e.ToString());
				#endif
				this.Values.Add(defaultValue);
			}
		}
		
		/// <summary>
		/// Returns the name of the performance counter.
		/// </summary>
		public override string ToString()
		{
			return Name;
		}
	}
}
