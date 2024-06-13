using Crestron.SimplSharpPro;
using System;

namespace SimplSharpTools.Joins.SmartGraphics
{
	public class SmartObjectDigitalJoin : SmartObjectJoin<bool>
	{
		public SmartObjectDigitalJoin(SmartObject smartObject, uint id) : base(smartObject, id)
		{
		}

		public override event Action<bool> OnChange;

		public override bool Value
		{
			get => smartObject.BooleanOutput[id].BoolValue;
			set => smartObject.BooleanInput[id].BoolValue = value;
		}

		protected override void SmartObject_SigChange(GenericBase _, SmartObjectEventArgs args)
		{
			if (args.Sig.Type == eSigType.Bool && args.Sig.Number == id)
			{
				OnChange?.Invoke(args.Sig.BoolValue);
			}
		}
	}
}
