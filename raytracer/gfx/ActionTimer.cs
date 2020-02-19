using System;

namespace raytracer
{
	public class ActionTimer
	{
		#region Fields

		private Action<ActionTimer> _action;
		private TimeSpan _duration;
		private TimeSpan _remaining;

		#endregion

		#region Constructors

		private ActionTimer(Action<ActionTimer> action, TimeSpan duration, bool repeat)
		{
			_action = action;
			_duration = duration;
			IsRepeating = repeat;
			_remaining = duration;
		}

		#endregion

		#region Properties

		public bool IsDead
		{
			get
			{
				return (_remaining <= TimeSpan.Zero) && !IsRepeating;
			}
		}

		public bool IsRepeating { get; set; }

		#endregion

		#region Methods

		public static ActionTimer Do(Action<ActionTimer> action, TimeSpan duration, bool repeat = false)
		{
			return new ActionTimer(action, duration, repeat);
		}

		public static ActionTimer FromSeconds(Action<ActionTimer> action, double seconds, bool repeat = false)
		{
			return new ActionTimer(action, TimeSpan.FromSeconds(seconds), repeat);
		}

		public static ActionTimer FromMilliseconds(Action<ActionTimer> action, double milliseconds, bool repeat = false)
		{
			return new ActionTimer(action, TimeSpan.FromMilliseconds(milliseconds), repeat);
		}

		public void Reset()
		{
			_remaining = _duration;
		}

		public void Update(GameTime gameTime)
		{
			_remaining = _remaining.Subtract(gameTime.ElapsedTime);
			if (_remaining <= TimeSpan.Zero)
			{
				_action(this);
				if (IsRepeating)
				{
					_remaining = _duration;
				}
			}
		}

		#endregion
	}
}
