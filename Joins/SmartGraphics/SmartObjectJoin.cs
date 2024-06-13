using Crestron.SimplSharpPro;
using System;

namespace SimplSharpTools.Joins.SmartGraphics
{
	public abstract class SmartObjectJoin<T>
	{
		protected readonly SmartObject smartObject;
		public readonly uint id;

		public SmartObjectJoin(SmartObject smartObject, uint id) 
		{
			this.smartObject = smartObject;
			this.id = id;

			smartObject.SigChange += SmartObject_SigChange;
		}

		public void Dispose()
		{
			smartObject.SigChange -= SmartObject_SigChange;
			GC.SuppressFinalize(this);
		}

		public abstract event Action<T> OnChange;

		public abstract T Value { get; set; }

		protected abstract void SmartObject_SigChange(GenericBase _, SmartObjectEventArgs args);
	}
}
