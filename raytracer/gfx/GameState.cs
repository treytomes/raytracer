using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace raytracer
{
	public abstract class GameState : IDisposable
	{
		#region Constructors

		protected GameState()
		{
		}

		#endregion

		#region Properties

		public bool HasFocus
		{
			get
			{
				return App.Current.MainWindow.IsFocused && (GameStateController.Instance.CurrentState == this);
			}
		}

		public bool IsUpdateable { get; protected set; } = true;

		public bool IsRenderable { get; protected set; } = true;

		protected int ScreenWidth { get; } = ScreenController.Instance.Width;

		protected int ScreenHeight { get; } = ScreenController.Instance.Height;

		private List<ActionTimer> Timers { get; } = new List<ActionTimer>();

		#endregion

		#region Methods

		public virtual void HandleEvent(KeyEventArgs e)
		{
		}

		public virtual void Update(GameTime gameTime)
		{
			for (var n = 0; n < Timers.Count; n++)
			{
				var timer = Timers[n];
				timer.Update(gameTime);
				if (timer.IsDead)
				{
					Timers.RemoveAt(n);
					n--;
				}
			}
		}

		public virtual void Render(GameTime gameTime)
		{
		}

		protected void EnterState(GameState state)
		{
			GameStateController.Instance.EnterState(state);
		}

		protected void LeaveState()
		{
			GameStateController.Instance.LeaveState();
		}

		protected void SwitchStates(GameState state)
		{
			GameStateController.Instance.SwitchStates(state);
		}

		protected ActionTimer AddTimer(ActionTimer timer)
		{
			Timers.Add(timer);
			return timer;
		}

		protected ActionTimer AddTimer(Action<ActionTimer> action, int milliseconds, bool repeat = false)
		{
			var timer = ActionTimer.FromMilliseconds(action, milliseconds, repeat);
			Timers.Add(timer);
			return timer;
		}

		#region IDisposable Support

		/// <summary>
		/// To detect redundant calls.
		/// </summary>
		private bool _disposedValue = false;

		/// <summary>
		/// Dispose managed state (managed objects).
		/// </summary>
		protected virtual void DisposeManaged()
		{
		}

		/// <summary>
		/// Free unmanaged resources (unmanaged objects) and override a finalizer below.
		/// Set large fields to null.
		/// </summary>
		protected virtual void DisposeUnmanaged()
		{
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					DisposeManaged();
				}

				DisposeUnmanaged();

				_disposedValue = true;
			}
		}

		~GameState()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(false);
		}

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		#endregion
	}
}
