using Crestron.SimplSharpPro;
using System;

namespace SimplSharpTools.Joins.SmartGraphics
{
	public class SmartObjectAnalogJoin : SmartObjectJoin<ushort>
	{
		public SmartObjectAnalogJoin(SmartObject smartObject, uint id) : base(smartObject, id)
		{
		}

		public override event Action<ushort> OnChange;

		public override ushort Value
		{
			get => smartObject.UShortOutput[id].UShortValue;
			set => smartObject.UShortInput[id].UShortValue = value;
		}

		protected override void SmartObject_SigChange(GenericBase _, SmartObjectEventArgs args)
		{
			if (args.Sig.Type == eSigType.UShort && args.Sig.Number == id)
			{
				OnChange?.Invoke(args.Sig.UShortValue);
			}
		}
	}
}
