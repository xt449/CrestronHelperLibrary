using Crestron.SimplSharpPro.DeviceSupport;
using System;

namespace CrestronHelperLibrary.SmartGraphics.Joins
{
	public class AnalogJoin : IJoin<ushort>
	{
		protected readonly BasicTriList triList;
		public readonly uint id;

		internal AnalogJoin(BasicTriList triList, uint id)
		{
			this.triList = triList;
			this.id = id;
		}

		public ushort Value
		{
			get => triList.UShortOutput[id].UShortValue;
			set => triList.UShortInput[id].UShortValue = value;
		}

		public event Action<ushort>? OnChange;

		internal void Change(ushort value) => OnChange?.Invoke(value);
	}
}
