using Crestron.SimplSharpPro;
using System;

namespace SimplSharpTools.Joins
{
	public class SmartObjectSerialJoin : IJoin<string>
	{
		protected readonly SmartObject smartObject;
		public readonly string id;

		internal SmartObjectSerialJoin(SmartObject smartObject, string id)
		{
			this.smartObject = smartObject;
			this.id = id;
		}

		public string Value
		{
			get => smartObject.StringOutput[id].StringValue;
			set => smartObject.StringInput[id].StringValue = value;
		}

		public event Action<string> OnChange;

		internal void Change(string value) => OnChange?.Invoke(value);
	}
}
