using Crestron.SimplSharpPro;
using System;

namespace SimplSharpTools.Joins.SmartGraphics
{
	public class SmartObjectSerialJoin : SmartObjectJoin<string>
	{
		public SmartObjectSerialJoin(SmartObject smartObject, uint id) : base(smartObject, id)
		{
		}

		public override event Action<string> OnChange;

		public override string Value
		{
			get => smartObject.StringOutput[id].StringValue;
			set => smartObject.StringInput[id].StringValue = value;
		}

		protected override void SmartObject_SigChange(GenericBase _, SmartObjectEventArgs args)
		{
			if (args.Sig.Type == eSigType.String && args.Sig.Number == id)
			{
				OnChange?.Invoke(args.Sig.StringValue);
			}
		}
	}
}
