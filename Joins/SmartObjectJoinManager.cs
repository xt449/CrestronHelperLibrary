using Crestron.SimplSharpPro;
using System.Collections.Generic;

namespace SimplSharpTools.Joins
{
	public class SmartObjectJoinManager
	{
		public readonly SmartObject smartObject;

		private readonly Dictionary<uint, SmartObjectDigitalJoin> digitalJoins;
		private readonly Dictionary<uint, SmartObjectAnalogJoin> analogJoins;
		private readonly Dictionary<uint, SmartObjectSerialJoin> serialJoins;

		internal SmartObjectJoinManager(SmartObject smartObject)
		{
			this.smartObject = smartObject;

			this.smartObject.SigChange += SmartObject_SigChange;
		}

		public SmartObjectDigitalJoin GetDigitalJoin(uint id)
		{
			if (digitalJoins.TryGetValue(id, out var digitalJoin))
			{
				return digitalJoin;
			}

			return digitalJoins[id] = new SmartObjectDigitalJoin(smartObject, id);
		}

		public SmartObjectAnalogJoin GetAnalogJoin(uint id)
		{
			if (analogJoins.TryGetValue(id, out var analogJoin))
			{
				return analogJoin;
			}

			return analogJoins[id] = new SmartObjectAnalogJoin(smartObject, id);
		}

		public SmartObjectSerialJoin GetSerialJoin(uint id)
		{
			if (serialJoins.TryGetValue(id, out var serialJoin))
			{
				return serialJoin;
			}

			return serialJoins[id] = new SmartObjectSerialJoin(smartObject, id);
		}

		private void SmartObject_SigChange(GenericBase _, SmartObjectEventArgs args)
		{
			switch (args.Sig.Type)
			{
				case eSigType.Bool:
					digitalJoins[args.Sig.Number].Change(args.Sig.BoolValue);
					break;
				case eSigType.UShort:
					analogJoins[args.Sig.Number].Change(args.Sig.UShortValue);
					break;
				case eSigType.String:
					serialJoins[args.Sig.Number].Change(args.Sig.StringValue);
					break;
			}
		}
	}
}
