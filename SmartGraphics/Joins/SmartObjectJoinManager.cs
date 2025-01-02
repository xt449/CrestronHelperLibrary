using Crestron.SimplSharpPro;
using System.Collections.Generic;

namespace CrestronHelperLibrary.SmartGraphics.Joins
{
	public class SmartObjectJoinManager
	{
		private readonly Dictionary<string, SmartObjectDigitalJoin> digitalJoins = new Dictionary<string, SmartObjectDigitalJoin>();
		private readonly Dictionary<string, SmartObjectAnalogJoin> analogJoins = new Dictionary<string, SmartObjectAnalogJoin>();
		private readonly Dictionary<string, SmartObjectSerialJoin> serialJoins = new Dictionary<string, SmartObjectSerialJoin>();

		public readonly SmartObject smartObject;

		internal SmartObjectJoinManager(SmartObject smartObject)
		{
			this.smartObject = smartObject;

			this.smartObject.SigChange += SmartObject_SigChange;
		}

		public SmartObjectDigitalJoin GetDigitalJoin(string id)
		{
			if (digitalJoins.TryGetValue(id, out SmartObjectDigitalJoin? digitalJoin))
			{
				return digitalJoin;
			}

			return digitalJoins[id] = new SmartObjectDigitalJoin(smartObject, id);
		}

		public SmartObjectAnalogJoin GetAnalogJoin(string id)
		{
			if (analogJoins.TryGetValue(id, out SmartObjectAnalogJoin? analogJoin))
			{
				return analogJoin;
			}

			return analogJoins[id] = new SmartObjectAnalogJoin(smartObject, id);
		}

		public SmartObjectSerialJoin GetSerialJoin(string id)
		{
			if (serialJoins.TryGetValue(id, out SmartObjectSerialJoin? serialJoin))
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
					digitalJoins[args.Sig.Name].Change(args.Sig.BoolValue);
					break;
				case eSigType.UShort:
					analogJoins[args.Sig.Name].Change(args.Sig.UShortValue);
					break;
				case eSigType.String:
					serialJoins[args.Sig.Name].Change(args.Sig.StringValue);
					break;
			}
		}
	}
}
