using Crestron.SimplSharpPro;
using System;

namespace SimplSharpTools.Joins
{
	public class SmartObjectDigitalJoin : IJoin<bool>
	{
		protected readonly SmartObject smartObject;
		public readonly uint id;

		public SmartObjectDigitalJoin(SmartObject smartObject, uint id)
		{
			this.smartObject = smartObject;
			this.id = id;
		}

		public bool Value
		{
			get => smartObject.BooleanOutput[id].BoolValue;
			set => smartObject.BooleanInput[id].BoolValue = value;
		}

		public event Action<bool> OnChange;

		internal void Change(bool value) => OnChange?.Invoke(value);
	}
}
