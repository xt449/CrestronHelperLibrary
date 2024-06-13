using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;
using Crestron.SimplSharpPro.DeviceSupport;

namespace SimplSharpTools.Joins.SmartGraphics
{
	public static class SGDFileHelper
	{
		public static bool LoadLocalSGDFile(this BasicTriListWithSmartObject smartGraphicsInterface, string fileName)
		{
			var path = Path.Combine(Directory.GetApplicationDirectory(), fileName);

			if (!File.Exists(path))
			{
				ErrorLog.Error("SGD file '{0}' not found in program directory! Set file to 'Copy always'.", fileName);
				return false;
			}

			smartGraphicsInterface.LoadSmartObjects(path);

			return true;
		}
	}
}
