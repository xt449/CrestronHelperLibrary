using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using System;

namespace SimplSharpTools.Joins
{
	public class DigitalJoin : Join<bool>
	{
		public DigitalJoin(BasicTriList triList, uint id) : base(triList, id)
		{
		}

		public override event Action<bool> OnChange;

		public override bool Value
		{
			get => triList.BooleanOutput[id].BoolValue;
			set => triList.BooleanInput[id].BoolValue = value;
		}

		protected override void TriList_SigChange(BasicTriList _, SigEventArgs args)
		{
			if (args.Sig.Type == eSigType.Bool && args.Sig.Number == id)
			{
				OnChange(args.Sig.BoolValue);
			}
		}
	}
}
