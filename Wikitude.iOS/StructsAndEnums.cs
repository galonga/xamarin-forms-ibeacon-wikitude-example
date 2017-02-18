using System;

namespace Wikitude.Architect
{

	public enum WTAugmentedRealityMode : uint {
		_Geo = 1,
		_IR,
		_Both
	}

	public enum WTScreenshotCaptureMode : uint {
		_Cam,
		_CamAndWebView
	}
		
	public enum WTScreenshotSaveMode : uint {
		_PhotoLibrary = 1,
		_BundleDirectory = 2,
		_Delegate = 3
	}

	public enum WTScreenshotSaveOptions : uint {
		CallDelegateOnSuccess = 1 << 0,
		SavingWithoutOverwriting = 1 << 1,
		None = 0
	}
}
