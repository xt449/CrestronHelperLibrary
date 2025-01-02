using Crestron.SimplSharpPro.DeviceSupport;
using System;

namespace CrestronHelperLibrary.SmartGraphics.Joins
{
	public class SerialJoin : IJoin<string>
	{
		protected readonly BasicTriList triList;
		public readonly uint id;

		internal SerialJoin(BasicTriList triList, uint id)
		{
			this.triList = triList;
			this.id = id;
		}

		public string Value
		{
			get => triList.StringOutput[id].StringValue;
			set => triList.StringInput[id].StringValue = value;
		}

		public event Action<string>? OnChange;

		internal void Change(string value) => OnChange?.Invoke(value);
	}
}
