using System;
using System.Threading;

namespace DataProcessor.Utility
{
	public class ReliableTimer : DataProcessor.Utility.ITimer
	{
		private Timer _timer;
		private int _interval = Timeout.Infinite;
		private bool _enabled;
		private bool _disposed;

		public event TimerCallback Tick
		{
			add
			{
				_timer = new Timer(value, null, Timeout.Infinite, Timeout.Infinite);
			}
			remove
			{ }
		}

		public void Start()
		{
			_enabled = true;
			_timer.Change(_interval, _interval);
		}

		public bool Enabled
		{
			get { return _enabled; }
			set
			{
				if (value)
				{
					Start();
				}
				else
				{
					Stop();
				}
			}
		}

		public int Interval
		{
			get { return _interval; }

			set
			{
				_interval = value;
				if (_enabled)
				{
					Start();
				}
			}
		}

		public void Stop()
		{
			_timer.Change(Timeout.Infinite, Timeout.Infinite);
			_enabled = false;
		}

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			if (disposing)
			{
				if (_timer != null)
				{
					_timer.Dispose();
				}
			}

			_timer = null;
			_disposed = true;
		}
	}
}