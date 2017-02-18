﻿using System;
using RadiusNetworks.IBeaconAndroid;

namespace ARArt.Android
{
    public class MonitorEventArgs : EventArgs
    {
        public Region Region { get; set; }
        public int State { get; set; }
    }

    /// <summary>
    /// Monitor notifier. Serves BeaconLocaterAndroid the region.
    /// </summary>
    public class MonitorNotifier : Java.Lang.Object, IMonitorNotifier
    {
        public event EventHandler<MonitorEventArgs> DetermineStateForRegionComplete;
        public event EventHandler<MonitorEventArgs> EnterRegionComplete;
        public event EventHandler<MonitorEventArgs> ExitRegionComplete;

        public void DidDetermineStateForRegion(int p0, Region p1)
        {
            OnDetermineStateForRegionComplete();
        }

        public void DidEnterRegion(Region p0)
        {
            OnEnterRegionComplete();
        }

        public void DidExitRegion(Region p0)
        {
            OnExitRegionComplete();
        }

        private void OnDetermineStateForRegionComplete()
        {
            if (DetermineStateForRegionComplete != null) {
                DetermineStateForRegionComplete(this, new MonitorEventArgs());
            }
        }

        private void OnEnterRegionComplete()
        {
            if (EnterRegionComplete != null) {
                EnterRegionComplete(this, new MonitorEventArgs());
            }
        }

        private void OnExitRegionComplete()
        {
            if (ExitRegionComplete != null) {
                ExitRegionComplete(this, new MonitorEventArgs());
            }
        }
    }
}
