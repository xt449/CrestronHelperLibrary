using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using System;

namespace SimplSharpTools.Joins
{
	internal class SerialJoin : Join<string>
	{
		public SerialJoin(BasicTriList triList, uint id) : base(triList, id)
		{
		}

		public override event Action<string> OnChange;

		public override string Value
		{
			get => triList.StringOutput[id].StringValue;
			set => triList.StringInput[id].StringValue = value;
		}

		protected override void TriList_SigChange(BasicTriList _, SigEventArgs args)
		{
			if (args.Sig.Type == eSigType.String && args.Sig.Number == id)
			{
				OnChange(args.Sig.StringValue);
			}
		}
	}
}
