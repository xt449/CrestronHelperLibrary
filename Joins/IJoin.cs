using System;

namespace SimplSharpTools.Joins
{
	public interface IJoin<T>
	{
		T Value { get; set; }

		event Action<T> OnChange;
	}
}
