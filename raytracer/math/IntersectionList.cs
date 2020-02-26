using System.Collections;
using System.Collections.Generic;

namespace raytracer.math
{
	public class IntersectionList
	{
		#region Fields

		private readonly List<Intersection> _innerList = new List<Intersection>();

		/// <summary>
		/// Set to true when a new item is added.
		/// Sort the list if necessary before retrieving an index, then set to false.
		/// </summary>
		private bool _needsSorting = false;

		#endregion

		#region Constructors

		public IntersectionList(params Intersection[] intersections)
		{
			_innerList.AddRange(intersections);
			_needsSorting = true;
		}

		#endregion

		#region Properties

		public Intersection this[int index]
		{
			get
			{
				if (_needsSorting)
				{
					_innerList.Sort((x, y) => x.T.CompareTo(y.T));
					_needsSorting = false;
				}
				return _innerList[index];
			}
		}

		public int Count => _innerList.Count;

		#endregion

		#region Methods

		public Intersection Hit()
		{
			if (_needsSorting)
			{
				_innerList.Sort((x, y) => x.T.CompareTo(y.T));
				_needsSorting = false;
			}
			for (var n = 0; n < _innerList.Count; n++)
			{
				if (_innerList[n].T >= 0)
				{
					return _innerList[n];
				}
			}
			return Intersection.Empty();
		}

		#endregion
	}
}
