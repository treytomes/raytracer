using System;

namespace raytracer
{
	public struct GameTime
	{
		public readonly TimeSpan TotalTime;
		public readonly TimeSpan ElapsedTime;

		public GameTime(TimeSpan totalTime, TimeSpan elapsedTime)
		{
			TotalTime = totalTime;
			ElapsedTime = elapsedTime;
		}
	}
}
