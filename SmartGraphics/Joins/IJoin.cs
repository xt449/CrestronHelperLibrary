using System;

namespace CrestronHelperLibrary.SmartGraphics.Joins
{
	public interface IJoin<T>
	{
		T Value { get; set; }

		event Action<T> OnChange;
	}
}
