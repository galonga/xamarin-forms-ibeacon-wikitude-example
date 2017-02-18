using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ARArt.Models;

namespace ARArt
{
    public interface IBeaconLocater
    {
        List<BeaconItem> GetAvailableBeacons();
        void PauseTracking();
        void ResumeTracking();
    }
}
