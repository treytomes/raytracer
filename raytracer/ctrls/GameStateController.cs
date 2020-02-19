using System.Collections.Generic;

namespace raytracer
{
	public class GameStateController
	{
		#region Fields

		private List<GameState> _states;

		#endregion

		#region Constructors

		private GameStateController()
		{
			_states = new List<GameState>();
		}

		#endregion

		#region Properties

		public static GameStateController Instance { get; } = new GameStateController();

		public int Count
		{
			get
			{
				return _states.Count;
			}
		}

		public GameState CurrentState
		{
			get
			{
				return (Count > 0) ? _states[0] : null;
			}
		}

		#endregion

		#region Methods

		public void EnterState(GameState state)
		{
			_states.Insert(0, state);
		}

		public void LeaveState()
		{
			_states[0].Dispose();
			_states.RemoveAt(0);
		}

		public void SwitchStates(GameState state)
		{
			LeaveState();
			EnterState(state);
		}

		public void Update(GameTime gameTime)
		{
			for (var index = 0; index < _states.Count; index++)
			{
				var state = _states[index];

				if (state.IsUpdateable)
				{
					state.Update(gameTime);
				}
			}
		}

		public void Render(GameTime gameTime)
		{
			for (var index = _states.Count - 1; index >= 0; index--)
			{
				var state = _states[index];

				if (state.IsRenderable)
				{
					state.Render(gameTime);
				}
			}
		}

		#endregion
	}
}
