using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using System.Collections.Generic;

namespace SimplSharpTools.Joins
{
	public class JoinManager
	{
		public readonly BasicTriListWithSmartObject triList;

		private readonly Dictionary<uint, DigitalJoin> digitalJoins;
		private readonly Dictionary<uint, AnalogJoin> analogJoins;
		private readonly Dictionary<uint, SerialJoin> serialJoins;
		private readonly Dictionary<uint, SmartObjectJoinManager> smartObjects;

		public JoinManager(BasicTriListWithSmartObject triList, string sgdFileName = null)
		{
			this.triList = triList;

			this.triList.SigChange += TriList_SigChange;

			if (sgdFileName != null)
			{
				var path = Path.Combine(Directory.GetApplicationDirectory(), sgdFileName);

				if (!File.Exists(path))
				{
					ErrorLog.Error("SGD file '{0}' not found in program directory! Set file to 'Copy always'.", sgdFileName);
				}

				this.triList.LoadSmartObjects(path);
			}
		}

		public DigitalJoin GetDigitialJoin(uint id)
		{
			if (digitalJoins.TryGetValue(id, out var digitalJoin))
			{
				return digitalJoin;
			}

			return digitalJoins[id] = new DigitalJoin(triList, id);
		}

		public AnalogJoin GetAnalogJoin(uint id)
		{
			if (analogJoins.TryGetValue(id, out var analogJoin))
			{
				return analogJoin;
			}

			return analogJoins[id] = new AnalogJoin(triList, id);
		}

		public SerialJoin GetSerialJoin(uint id)
		{
			if (serialJoins.TryGetValue(id, out var serialJoin))
			{
				return serialJoin;
			}

			return serialJoins[id] = new SerialJoin(triList, id);
		}

		public SmartObjectJoinManager GetSmartObject(uint id)
		{
			if (smartObjects.TryGetValue(id, out var smartObject))
			{
				return smartObject;
			}

			return smartObjects[id] = new SmartObjectJoinManager(triList.SmartObjects[id]);
		}

		private void TriList_SigChange(BasicTriList _, SigEventArgs args)
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
