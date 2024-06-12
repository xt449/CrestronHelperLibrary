using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using System;

namespace SimplSharpTools.Joins
{
	internal class AnalogJoin : Join<ushort>
	{
		public AnalogJoin(BasicTriList triList, uint id) : base(triList, id)
		{
		}

		public override event Action<ushort> OnChange;

		public override ushort Value
		{
			get => triList.UShortOutput[id].UShortValue;
			set => triList.UShortInput[id].UShortValue = value;
		}

		protected override void TriList_SigChange(BasicTriList _, SigEventArgs args)
		{
			if (args.Sig.Type == eSigType.UShort && args.Sig.Number == id)
			{
				OnChange(args.Sig.UShortValue);
			}
		}
	}
}
