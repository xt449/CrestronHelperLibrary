using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;
using Crestron.SimplSharpPro;
using Crestron.SimplSharpPro.DeviceSupport;
using System.Collections.Generic;

namespace CrestronHelperLibrary.SmartGraphics.Joins
{
	public class JoinManager
	{
		private readonly Dictionary<uint, DigitalJoin> digitalJoins = new Dictionary<uint, DigitalJoin>();
		private readonly Dictionary<uint, AnalogJoin> analogJoins = new Dictionary<uint, AnalogJoin>();
		private readonly Dictionary<uint, SerialJoin> serialJoins = new Dictionary<uint, SerialJoin>();
		private readonly Dictionary<uint, SmartObjectJoinManager> smartObjects = new Dictionary<uint, SmartObjectJoinManager>();

		public readonly BasicTriListWithSmartObject triList;

		public JoinManager(BasicTriListWithSmartObject triList, string? sgdFileName = null)
		{
			this.triList = triList;

			this.triList.SigChange += TriList_SigChange;

			if (sgdFileName != null)
			{
				string path = Path.Combine(Directory.GetApplicationDirectory(), sgdFileName);

				if (!File.Exists(path))
				{
					ErrorLog.Error("SGD file '{0}' not found in program directory! Set file to 'Copy always'.", sgdFileName);
				}

				this.triList.LoadSmartObjects(path);
			}
		}

		public DigitalJoin GetDigitalJoin(uint id)
		{
			if (digitalJoins.TryGetValue(id, out DigitalJoin? digitalJoin))
			{
				return digitalJoin;
			}

			return digitalJoins[id] = new DigitalJoin(triList, id);
		}

		public AnalogJoin GetAnalogJoin(uint id)
		{
			if (analogJoins.TryGetValue(id, out AnalogJoin? analogJoin))
			{
				return analogJoin;
			}

			return analogJoins[id] = new AnalogJoin(triList, id);
		}

		public SerialJoin GetSerialJoin(uint id)
		{
			if (serialJoins.TryGetValue(id, out SerialJoin? serialJoin))
			{
				return serialJoin;
			}

			return serialJoins[id] = new SerialJoin(triList, id);
		}

		public SmartObjectJoinManager GetSmartObject(uint id)
		{
			if (smartObjects.TryGetValue(id, out SmartObjectJoinManager? smartObject))
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
