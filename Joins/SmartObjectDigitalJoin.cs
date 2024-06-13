using Crestron.SimplSharpPro;
using System;

namespace SimplSharpTools.Joins
{
	public class SmartObjectDigitalJoin : IJoin<bool>
	{
		protected readonly SmartObject smartObject;
		public readonly string id;

		internal SmartObjectDigitalJoin(SmartObject smartObject, string id)
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
