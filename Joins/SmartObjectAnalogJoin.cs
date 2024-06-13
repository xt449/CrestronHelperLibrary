using Crestron.SimplSharpPro;
using System;

namespace SimplSharpTools.Joins
{
	public class SmartObjectAnalogJoin : IJoin<ushort>
	{
		protected readonly SmartObject smartObject;
		public readonly uint id;

		public SmartObjectAnalogJoin(SmartObject smartObject, uint id)
		{
			this.smartObject = smartObject;
			this.id = id;
		}

		public ushort Value
		{
			get => smartObject.UShortOutput[id].UShortValue;
			set => smartObject.UShortInput[id].UShortValue = value;
		}

		public event Action<ushort> OnChange;

		internal void Change(ushort value) => OnChange?.Invoke(value);
	}
}
