using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;

namespace raytracer
{
	public class GameController : IDisposable
	{
		public enum LoopState
		{
			Running,
			Paused,
			Shutdown
		}

		#region Constants

		private const int TARGET_FPS = 60;

		#endregion

		#region Fields

		private ConcurrentQueue<KeyEventArgs> _events;

		#endregion

		#region Constructors

		private GameController()
		{
			_events = new ConcurrentQueue<KeyEventArgs>();

			State = LoopState.Running;
			FramesPerSecond = TARGET_FPS;
			UpdatesPerSecond = TARGET_FPS;

			GameStateController.Instance.EnterState(new RayTracerGameState());

			Task.Factory.StartNew(RunUpdate);
			//Task.Factory.StartNew(RunRender);
		}

		#endregion

		#region Properties

		public static GameController Instance { get; private set; }

		public LoopState State { get; set; }

		public int FramesPerSecond
		{
			get
			{
				return (int)(1.0 / FrameInterval.TotalSeconds);
			}
			set
			{
				if (FramesPerSecond != value)
				{
					FrameInterval = TimeSpan.FromSeconds(1.0 / value);
				}
			}
		}

		public int UpdatesPerSecond
		{
			get
			{
				return (int)(1.0 / UpdateInterval.TotalSeconds);
			}
			set
			{
				if (UpdatesPerSecond != value)
				{
					UpdateInterval = TimeSpan.FromSeconds(1.0 / value);
				}
			}
		}

		protected TimeSpan FrameInterval { get; private set; }

		protected TimeSpan UpdateInterval { get; private set; }

		protected GameState CurrentState
		{
			get
			{
				return GameStateController.Instance.CurrentState;
			}
			set
			{
				GameStateController.Instance.EnterState(value);
			}
		}

		#endregion

		#region Methods

		public static GameController Initialize()
		{
			Instance = new GameController();
			return Instance;
		}

		public void HandleEvent(KeyEventArgs e)
		{
			_events.Enqueue(e);
			//CurrentState.HandleEvent(e);
		}

		public void RunUpdate()
		{
			var gameTimer = Stopwatch.StartNew();
			var updateFrameTimer = Stopwatch.StartNew();
			var renderFrameTimer = Stopwatch.StartNew();

			while ((State != LoopState.Shutdown) && (CurrentState != null))
			{
				if (State == LoopState.Paused)
				{
					gameTimer.Stop();
					renderFrameTimer.Stop();
					while (State == LoopState.Paused) ;
					gameTimer.Start();
					renderFrameTimer.Start();
				}

				if (updateFrameTimer.Elapsed >= UpdateInterval)
				{
					// TODO: Probably better to handle events here, so framerate is respected.
					KeyEventArgs e;
					while (_events.TryDequeue(out e))
					{
						CurrentState.HandleEvent(e);
					}

					CurrentState?.Update(new GameTime(gameTimer.Elapsed, updateFrameTimer.Elapsed));
					updateFrameTimer.Restart();
				}

				if (renderFrameTimer.Elapsed >= FrameInterval)
				{
					CurrentState?.Render(new GameTime(gameTimer.Elapsed, renderFrameTimer.Elapsed));
					ScreenController.Instance.Refresh();
					renderFrameTimer.Restart();
				}
			}

			if (CurrentState == null)
			{
				App.Current.Dispatcher.Invoke(() => App.Current.Shutdown(0));
			}
		}

		//public void RunUpdate()
		//{
		//	var gameTimer = Stopwatch.StartNew();
		//	var updateFrameTimer = Stopwatch.StartNew();

		//	while ((State != LoopState.Shutdown) && (CurrentState != null))
		//	{
		//		if (State == LoopState.Paused)
		//		{
		//			gameTimer.Stop();
		//			while (State == LoopState.Paused) ;
		//			gameTimer.Start();
		//		}

		//		if (updateFrameTimer.Elapsed >= UpdateInterval)
		//		{
		//			// TODO: Probably better to handle events here, so framerate is respected.
		//			KeyEventArgs e;
		//			while (_events.TryDequeue(out e))
		//			{
		//				CurrentState.HandleEvent(e);
		//			}

		//			CurrentState.Update(new GameTime(gameTimer.Elapsed, updateFrameTimer.Elapsed));
		//			updateFrameTimer.Restart();
		//		}
		//	}
		//}

		//public void RunRender()
		//{
		//	var gameTimer = Stopwatch.StartNew();
		//	var renderFrameTimer = Stopwatch.StartNew();

		//	while ((State != LoopState.Shutdown) && (CurrentState != null))
		//	{
		//		if (State == LoopState.Paused)
		//		{
		//			gameTimer.Stop();
		//			renderFrameTimer.Stop();
		//			while (State == LoopState.Paused) ;
		//			gameTimer.Start();
		//			renderFrameTimer.Start();
		//		}

		//		if (renderFrameTimer.Elapsed >= FrameInterval)
		//		{
		//			CurrentState.Render(new GameTime(gameTimer.Elapsed, renderFrameTimer.Elapsed));
		//			ScreenController.Instance.Refresh();
		//			renderFrameTimer.Restart();
		//		}
		//	}

		//	renderFrameTimer.Stop();
		//}

		#endregion

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					State = LoopState.Shutdown;
					//_processorTask.Wait();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~GameController() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}
