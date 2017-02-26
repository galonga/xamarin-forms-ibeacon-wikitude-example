using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;
using CoreLocation;
using CoreBluetooth;
using Foundation;
using ObjCRuntime;
using CoreMotion;
using System.Collections.Generic;
using CoreAnimation;
using CoreGraphics;
using System.Linq;
using Wikitude.Architect;
using ARArt;
using System.IO;
using System.Json;
using MonoTouch.Dialog;
using ARArt.Pages;
using ARArt.Common;

[assembly: ExportRenderer(typeof(ARArt.Pages.ARPage), typeof(ARArt.iOS.Renderer.ARPageRenderer))]

namespace ARArt.iOS.Renderer
{
    public class ARPageRenderer : PageRenderer
    {
        protected WTArchitectView arView;
        protected string arID;

        public string WorldOrUrl { get; private set; }

        public bool IsUrl { get; private set; }

        public ARPageRenderer() : base()
        {
            IsUrl = false;

        }

        /// <summary>
        /// Raises the element changed event.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            var page = e.NewElement as ARPage;
            arID = page.ArObjectId;
            this.Title = "Exhibit ID: " + arID;

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //register notification for iO8
            var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert, null);
            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);

            //set fullscreen on iOS7
            if (UIDevice.CurrentDevice.CheckSystemVersion(7, 0)) {
                EdgesForExtendedLayout = UIRectEdge.None;
            }

            //Create AR View
            if (WTArchitectView.IsDeviceSupportedForAugmentedRealityMode(WTAugmentedRealityMode._Geo)) {
                arView = new WTArchitectView(UIScreen.MainScreen.Bounds, null, WTAugmentedRealityMode._Geo);
                arView.SetLicenseKey(AppSettings.WikitudeSDKkeyiOS);
                //Set path to wikitude components
                var absoluteWorldUrl = NSBundle.MainBundle.BundleUrl.AbsoluteString + "wikitude/" + arID + "/index.html";
                var u = new NSUrl(absoluteWorldUrl);
                //load AR world
                arView.LoadArchitectWorldFromUrl(u);

                View.AddSubview(arView);
            } else {
                var adErr = new UIAlertView("Unsupported Device", "This device is not capable of running ARchitect Worlds. Requirements are: iOS 5 or higher, iPhone 3GS or higher, iPad 2 or higher. Note: iPod Touch 4th and 5th generation are only supported in WTARMode_IR.", null, "OK", null);
                adErr.Show();
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            //start AR View
            base.ViewDidAppear(animated);

            if (arView != null) {
                arView.Start();
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            //Allow other 3d models
            App.mainPage.FoundBeacon = false;

            if (arView != null) {
                //resume tracking of beacons
                App.mainPage.ResumeTracking();

                arView.Stop();
            }
        }
    }
}
