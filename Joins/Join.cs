using Crestron.SimplSharpPro.DeviceSupport;
using System;

namespace SimplSharpTools.Joins
{
	public abstract class Join<T> : IDisposable
	{
		protected readonly BasicTriList triList;
		public readonly uint id;

		public Join(BasicTriList triList, uint id)
		{
			this.triList = triList;
			this.id = id;

			this.triList.SigChange += TriList_SigChange;
		}

		public void Dispose()
		{
			triList.SigChange -= TriList_SigChange;
			GC.SuppressFinalize(this);
		}

		public abstract event Action<T> OnChange;

		public abstract T Value { get; set; }

		protected abstract void TriList_SigChange(BasicTriList _, Crestron.SimplSharpPro.SigEventArgs args);
	}
}
