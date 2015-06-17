﻿using System;
namespace DataProcessor.Utility
{
	public interface ITimer
	{
		void Dispose();
		bool Enabled { get; set; }
		int Interval { get; set; }
		void Start();
		void Stop();
		event System.Threading.TimerCallback Tick;
	}
}
