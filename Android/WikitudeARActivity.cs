using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;
using Android.Hardware;
using Android.Util;
using Wikitude.Architect;
using ARArt;
using Java.IO;
using ARArt.Common;

namespace ARArt.Android
{
    /// <summary>
    /// Wikitude view.
    /// </summary>
    [Activity(Label = "WikitudeView")]
    public class WikitudeARActivity : Activity, ArchitectView.ISensorAccuracyChangeListener, ILocationListener
    {
        protected ArchitectView architectView;
        protected Location lastKnownLocation;
        protected ILocationProvider locationProvider;

        string world;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.cam);

            var title = "Exhibit ID:";
            Title = title + Intent.GetStringExtra("id");

            world = "wikitude" + File.Separator + Intent.GetStringExtra("id") + File.Separator + "index.html";

            architectView = FindViewById<ArchitectView>(Resource.Id.architectView);

            var config = new ArchitectView.ArchitectConfig(AppSettings.WikitudeSDKkeyAndroid);
            architectView.OnCreate(config);

            this.architectView.RegisterSensorAccuracyChangeListener(this);
            this.locationProvider = new LocationProvider(this, this);
        }

        /// <summary>
        /// Raises the compass accuracy changed event.
        /// </summary>
        /// <param name="accuracy">Accuracy.</param>
        #region ISensorAccuracyChangeListener implementation
        public void OnCompassAccuracyChanged(int accuracy)
        {
            /* UNRELIABLE = 0, LOW = 1, MEDIUM = 2, Height = 3 */
            if (accuracy < 2 && !this.IsFinishing)
                Toast.MakeText(this, Resource.String.compass_accuracy_low, ToastLength.Long).Show();
        }
        #endregion

        #region ILocationListener implementation

        /// <Docs>The new location, as a Location object.</Docs>
        /// <remarks>Called when the location has changed.</remarks>
        /// <summary>
        /// Raises the location changed event.
        /// </summary>
        /// <param name="location">Location.</param>
        public void OnLocationChanged(Location location)
        {
            if (location != null)
                lastKnownLocation = location;

            if (location.HasAltitude)
                architectView.SetLocation(location.Latitude, location.Longitude, location.Altitude, location.HasAccuracy ? location.Accuracy : 1000);
            else
                architectView.SetLocation(location.Latitude, location.Longitude, location.HasAccuracy ? location.Accuracy : 1000);
        }

        //
        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
        }
        #endregion

        /// <summary>
        /// Raises the resume event.
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();

            if (architectView != null)
                architectView.OnResume();

            if (locationProvider != null)
                locationProvider.OnResume();
        }

        protected override void OnPause()
        {
            base.OnPause();

            if (architectView != null)
                architectView.OnPause();

            if (locationProvider != null)
                locationProvider.OnPause();
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        /// <Docs>Perform any final cleanup before an activity is destroyed.</Docs>
        /// Resum beacon discovery
        /// <summary>
        /// Raises the destroy event.
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (architectView != null) {
                this.architectView.UnregisterSensorAccuracyChangeListener(this);

                architectView.OnDestroy();
            }

            App.mainPage.FoundBeacon = false;
            App.mainPage.ResumeTracking();
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();

            if (architectView != null)
                architectView.OnLowMemory();
        }

        /// <summary>
        /// After initialisation - starts wikitude AR View with created world
        /// </summary>
        /// <param name="savedInstanceState">Saved instance state.</param>
        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);

            if (architectView != null)
                architectView.OnPostCreate();

            try {
                architectView.Load(world);
            } catch (Exception ex) {
                Log.Error("ARArt", ex.ToString());
            }
        }
    }
}
