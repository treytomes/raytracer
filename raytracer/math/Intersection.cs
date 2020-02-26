namespace raytracer.math
{
	public struct Intersection
	{
		#region Fields

		public readonly float T;
		public readonly object Object;

		#endregion

		#region Constructors

		public Intersection(float t, object obj)
		{
			T = t;
			Object = obj;
		}

		#endregion

		#region Methods

		public static Intersection Empty()
		{
			return new Intersection(0, null);
		}

		public override string ToString()
		{
			return $"Intersection({T},{Object})";
		}

		#endregion
	}
}
