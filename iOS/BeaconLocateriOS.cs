using System;
using ARArt;
using Xamarin.Forms;
using ARArt.iOS;
using MonoTouch.CoreLocation;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using ARArt.Models;
using ARArt.Common;

[assembly: Dependency (typeof(BeaconLocateriOS))]

namespace ARArt
{
	public class BeaconLocateriOS : IBeaconLocater
	{

		CLLocationManager locationManager;
		readonly string roximityUuid = AppSettings.IBeaconID;
		readonly string roximityBeaconId = "iBeacon";
		CLBeaconRegion rBeaconRegion;
		public bool found;
		List<BeaconItem> beacons;
		bool paused;

		public BeaconLocateriOS ()
		{
			//iBeacon-Einstellungen laden und die Suche nach iBeacon beginnen
			SetupBeaconRanging ();
			locationManager.StartMonitoring (rBeaconRegion);
			locationManager.RequestState (rBeaconRegion);
		}

		/// <summary>
		/// iBeacon-Suche pausieren
		/// </summary>
		public void PauseTracking ()
		{
			paused = true;
		}

		/// <summary>
		/// iBeacon-Suche fortsetzten
		/// </summary>
		public void ResumeTracking ()
		{
			paused = false;
		}


		/// <summary>
		/// Gibt die verfügbaren iBeacon zurück
		/// </summary>
		/// <returns>The available beacons.</returns>
		public List<BeaconItem> GetAvailableBeacons ()
		{
			return !paused ? beacons : null;
		}

		/// <summary>
		/// Initialisieren der iBeacon
		/// </summary>
		private void SetupBeaconRanging ()
		{
			//LocationManager zum Zugriff auf Ortungsdienste
			locationManager = new CLLocationManager ();
			locationManager.RequestAlwaysAuthorization ();
			beacons = new List<BeaconItem> ();

			var rUuid = new NSUuid (roximityUuid);
			//iBeacon mit ID erstellen
			rBeaconRegion = new CLBeaconRegion (rUuid, roximityBeaconId);

			//Benachrichtung bei verfügbaren iBeacon und beim verlassen von iBeacon aktivieren
			rBeaconRegion.NotifyEntryStateOnDisplay = true;
			rBeaconRegion.NotifyOnEntry = true;
			rBeaconRegion.NotifyOnExit = true;

			//Suche nach iBeacon starten und Methode zuweisen die Aufgerufen werden sollen, sobald ein iBeacon gefunden wurde
			locationManager.RegionEntered += HandleRegionEntered;
			locationManager.RegionLeft += HandleRegionLeft;
			locationManager.DidDetermineState += HandleDidDetermineState;
			locationManager.DidRangeBeacons += HandleDidRangeBeacons;
		}
		/// <summary>
		/// Suche nach bestimmten iBeacon stoppen
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void HandleRegionLeft (object sender, CLRegionEventArgs e)
		{
			if (e.Region.Identifier.Equals (roximityBeaconId)) {
				locationManager.StopRangingBeacons (rBeaconRegion);
			} 
		}

		/// <summary>
		/// Benarichtigung wenn ein iBeacon in der nähe ist
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void HandleRegionEntered (object sender, CLRegionEventArgs e)
		{

			Console.WriteLine ("Region entered: " + e.Region.Identifier);
			if (e.Region.Identifier.Equals (roximityBeaconId)) {
				locationManager.StartRangingBeacons (rBeaconRegion);
				var notification = new UILocalNotification { AlertBody = "Ein Bild ist in der nähe" };
				UIApplication.SharedApplication.PresentLocationNotificationNow (notification);
			} 
		}

		/// <summary>
		/// Starten oder Stoppen der iBeacon-Entfernungsberechnung wenn ein iBeacon in der nähe ist oder den Bereich verfässt
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void HandleDidDetermineState (object sender, CLRegionStateDeterminedEventArgs e)
		{
			if (e.Region.Identifier.Equals (roximityBeaconId)) {
				if (e.State == CLRegionState.Inside) {
					Console.WriteLine ("Inside roximity beacon region [{0}]", e.Region.Identifier);
					locationManager.StartRangingBeacons (rBeaconRegion);
		
				} else if (e.State == CLRegionState.Outside) {
					Console.WriteLine ("Outside roximity beacon region");
					locationManager.StopRangingBeacons (rBeaconRegion);
				}
			} 
		}

		/// <summary>
		/// Entfernung zu iBeacon bestimmen und iBeacon speichern
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void HandleDidRangeBeacons (object sender, CLRegionBeaconsRangedEventArgs e)
		{
			if (e.Beacons.Length > 0) {
				foreach (var b in e.Beacons) {

					//if (b.Proximity == CLProximity.Immediate) {
						Console.WriteLine ("UUID: {0} | Major: {1} | Minor: {2} | Accuracy: {3} | Proximity: {4} | RSSI: {5}", b.ProximityUuid, b.Major, b.Minor, b.Accuracy, b.Proximity, b.Rssi);
						var exists = false;
						for (int i = 0; i < beacons.Count; i++) {
							if (beacons [i].Minor.ToString().Equals (b.Minor.ToString ())) {
								beacons [i].CurrentDistance = Math.Round (b.Accuracy, 2);
								SetProximity (b, beacons [i]);
								exists = true;
							}
						}
						if (!exists) {
							var newBeacon = new BeaconItem {
								Minor = (int)b.Minor,
								Name = "",
								CurrentDistance = Math.Round (b.Accuracy, 2)
							};
							SetProximity (b, newBeacon);
							beacons.Add (newBeacon);
						}
					//}
				}
			}
		}

		/// <summary>
		/// Setzen der Entfernung
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="dest">Destination.</param>
		void SetProximity (CLBeacon source, BeaconItem dest)
		{

			Proximity p = Proximity.Unknown;

			switch (source.Proximity) {
			case CLProximity.Immediate:
				p = Proximity.Immediate;
				break;
			case CLProximity.Near:
				p = Proximity.Near;
				break;
			case CLProximity.Far:
				p = Proximity.Far;
				break;
			}

			if (p > dest.Proximity || p < dest.Proximity) {
				dest.ProximityChangeTimestamp = DateTime.Now;
			} 

			dest.Proximity = p;
		}
	}
}

