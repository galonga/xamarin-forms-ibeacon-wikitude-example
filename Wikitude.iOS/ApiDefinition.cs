using System;
using System.Drawing;

using MonoTouch.ObjCRuntime;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreMotion;
using MonoTouch.CoreLocation;

namespace Wikitude.Architect
{
	[BaseType(typeof(NSObject), Name="WTArchitectViewDelegate")]
	[Model]
	[Protocol]
	public partial interface WTArchitectViewDelegate {

		[Export ("architectView:didFinishLoad:"), EventArgs("ArchitectViewFinishLoad")]
		void DidFinishLoad (WTArchitectView architectView, NSUrl url);

		[Export ("architectView:didFailLoadWithError:"), EventArgs ("ArchitectViewLoadFailed")]
		void DidFailLoadWithError (WTArchitectView architectView, NSError error);

		[Export ("architectView:invokedURL:"), EventArgs ("ArchitectViewInvokedURL")]
		void InvokedURL (WTArchitectView architectView, NSUrl url);

		[Export ("architectView:didCaptureScreenWithContext:"), EventArgs("ArchitectViewCaptureScreen")]
		void DidCaptureScreenWithContext (WTArchitectView architectView, NSDictionary context);

		[Export ("architectView:didFailCaptureScreenWithError:"), EventArgs("ArchitectViewFailCaptureScreen")]
		void DidFailCaptureScreenWithError (WTArchitectView architectView, NSError error);	
	}


	[BaseType (typeof(UIView), Name="WTArchitectView", Delegates=new string[] { "WeakDelegate" }, Events=new Type[] { typeof(WTArchitectViewDelegate) })]
	public partial interface WTArchitectView {

		[Field ("kWTScreenshotBundleDirectoryKey", "__Internal")]
		NSString WTScreenshotBundleDirectoryKey { get; }

		[Field ("kWTScreenshotSaveModeKey", "__Internal")]
		NSString WTScreenshotSaveModeKey { get; }

		[Field ("kWTScreenshotCaptureModeKey", "__Internal")]
		NSString WTScreenshotCaptureModeKey { get; }

		[Field ("kWTScreenshotImageKey", "__Internal")]
		NSString WTScreenshotImageKey { get; }


		[Export("delegate"), NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap("WeakDelegate")]
		WTArchitectViewDelegate Delegate { get;set; }

		[Export ("isRunning")]
		bool IsRunning { get; }

		[Export ("desiredLocationAccuracy")]
		Double DesiredLocationAccuracy { get; set; }

		[Export ("desiredDistanceFilter")]
		Double DesiredDistanceFilter { get; set; }

		[Export ("shouldWebViewRotate")]
		bool ShouldWebViewRotate { get; set; }

		[Static, Export ("isDeviceSupportedForAugmentedRealityMode:")]
		bool IsDeviceSupportedForAugmentedRealityMode (WTAugmentedRealityMode supportedARMode);

		[Static, Export ("versionNumber")]
		string VersionNumber { get; }

		[Export ("initWithFrame:motionManager:augmentedRealityMode:")]
		IntPtr Constructor (RectangleF frame, [NullAllowed] CMMotionManager motionManagerOrNil, WTAugmentedRealityMode augmentedRealityMode);

		[Export ("initializeWithKey:motionManager:")]
		void InitializeWithKey (string key, CMMotionManager motionManager);

		[Export ("setLicenseKey:")]
		void SetLicenseKey (string licenseKey);

		[Export ("loadArchitectWorldFromUrl:")]
		void LoadArchitectWorldFromUrl (NSUrl architectWorldUrl);

		[Export ("callJavaScript:")]
		void CallJavaScript (string javaScript);

		[Export ("injectLocationWithLatitude:longitude:altitude:accuracy:")]
		void InjectLocation (Double latitude, Double longitude, Double altitude, Double accuracy);

		[Export ("injectLocationWithLatitude:longitude:accuracy:")]
		void InjectLocation (Double latitude, Double longitude, Double accuracy);

		[Export ("useInjectedLocation")]
		bool UseInjectedLocation { set; }

		[Export ("isUsingInjectedLocation")]
		bool IsUsingInjectedLocation { get; }

		[Export ("cullingDistance")]
		float CullingDistance { get; set; }

		[Export ("clearCache")]
		void ClearCache ();

		[Export ("setShouldRotate:toInterfaceOrientation:")]
		void SetShouldRotateToInterfaceOrientation (bool shouldAutoRotate, UIInterfaceOrientation interfaceOrientation);

		[Export ("isRotatingToInterfaceOrientation")]
		bool IsRotatingToInterfaceOrientation { get; }

		[Export ("stop")]
		void Stop ();

		[Export ("start")]
		void Start ();

		[Export ("motionManager")]
		CMMotionManager MotionManager { get; }

		[Export ("captureScreenWithMode:usingSaveMode:saveOptions:context:")]
		void CaptureScreenWithMode (WTScreenshotCaptureMode captureMode, WTScreenshotSaveMode saveMode, WTScreenshotSaveOptions options, NSDictionary context);
	}
}
