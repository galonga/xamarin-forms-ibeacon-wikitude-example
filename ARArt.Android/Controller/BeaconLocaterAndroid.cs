using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.App;
using Android.Content;
using Wikitude.Architect;
using ARArt;
using RadiusNetworks.IBeaconAndroid;
using System.Collections.Generic;
using ARArt.Android;
using Android.Support.V4.App;
using ARArt.Common;
using ARArt.Models;

[assembly: Dependency(typeof(BeaconLocaterAndroid))]
namespace ARArt.Android
{
    public class BeaconLocaterAndroid : Java.Lang.Object, IBeaconLocater, IBeaconConsumer
    {
        readonly string uuid = AppSettings.IBeaconID;
        readonly string beaconId = "iBeacon";

        IBeaconManager iBeaconManager;
        MonitorNotifier monitorNotifier;
        RangeNotifier rangeNotifier;
        Region monitoringRegion;
        Region rangingRegion;
        Context context;
        bool paused;
        List<BeaconItem> beacons;

        /// <summary>
        /// Initializes a new instance of the <see cref="ARArt.Android.BeaconLocaterAndroid"/> class.
        /// </summary>
        public BeaconLocaterAndroid()
        {
            beacons = new List<BeaconItem>();
            context = Xamarin.Forms.Forms.Context;
            iBeaconManager = IBeaconManager.GetInstanceForApplication(context);
            monitorNotifier = new MonitorNotifier();
            rangeNotifier = new RangeNotifier();
            monitoringRegion = new Region(beaconId, uuid, null, null);
            rangingRegion = new Region(beaconId, uuid, null, null);
            iBeaconManager.Bind(this);
            rangeNotifier.DidRangeBeaconsInRegionComplete += RangingBeaconsInRegion;

        }

        /// <summary>
        /// Get beacons
        /// </summary>
        /// <returns>The available beacons.</returns>
        public List<BeaconItem> GetAvailableBeacons()
        {
            return !paused ? beacons : null;
        }

        /// <summary>
        /// Pause tracking.
        /// </summary>
        public void PauseTracking()
        {
            paused = true;
        }

        /// <summary>
        /// Resumes tracking.
        /// </summary>
        public void ResumeTracking()
        {
            paused = false;
        }

        /// <summary>
        /// iBeacons in Regionen X.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void RangingBeaconsInRegion(object sender, RangeEventArgs e)
        {
            if (e.Beacons.Count > 0) {
                foreach (var b in e.Beacons) {
                    if ((ProximityType)b.Proximity != ProximityType.Unknown) {
                        var exists = false;
                        for (int i = 0; i < beacons.Count; i++) {
                            if (beacons[i].Minor == b.Minor) {
                                beacons[i].CurrentDistance = Math.Round(b.Accuracy, 2);
                                SetProximity(b, beacons[i]);
                                exists = true;
                            }
                        }
                        if (!exists) {
                            var newBeacon = new BeaconItem {
                                Minor = b.Minor,
                                Name = "",
                                CurrentDistance = Math.Round(b.Accuracy, 2)
                            };
                            SetProximity(b, newBeacon);
                            beacons.Add(newBeacon);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the proximity.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="dest">Destination.</param>
        void SetProximity(IBeacon source, BeaconItem dest)
        {

            Proximity p = Proximity.Unknown;

            switch ((ProximityType)source.Proximity) {
                case ProximityType.Immediate:
                p = Proximity.Immediate;
                break;
                case ProximityType.Near:
                p = Proximity.Near;
                break;
                case ProximityType.Far:
                p = Proximity.Far;
                break;
            }

            if (p > dest.Proximity || p < dest.Proximity) {
                dest.ProximityChangeTimestamp = DateTime.Now;
            }

            dest.Proximity = p;
        }

        /// <summary>
        /// Raises the I beacon service connect event.
        /// </summary>
        public void OnIBeaconServiceConnect()
        {
            iBeaconManager.SetMonitorNotifier(monitorNotifier);
            iBeaconManager.SetRangeNotifier(rangeNotifier);

            iBeaconManager.StartMonitoringBeaconsInRegion(monitoringRegion);
            iBeaconManager.StartRangingBeaconsInRegion(rangingRegion);
        }

        /// <summary>
        /// Gets the application context.
        /// </summary>
        /// <value>The application context.</value>
        public Context ApplicationContext {
            get { return this.context; }
        }

        /// <summary>
        /// iBeacon Service binder.
        /// </summary>
        /// <returns><c>true</c>, if service was bound, <c>false</c> otherwise.</returns>
        /// <param name="intent">Intent.</param>
        /// <param name="connection">Connection.</param>
        /// <param name="bind">Bind.</param>
        public bool BindService(Intent intent, IServiceConnection connection, Bind bind)
        {
            return context.BindService(intent, connection, bind);
        }

        /// <summary>
        /// iBeacon Service unbinder.
        /// </summary>
        /// <param name="connection">Connection.</param>
        public void UnbindService(IServiceConnection connection)
        {
            context.UnbindService(connection);
        }
    }
}
