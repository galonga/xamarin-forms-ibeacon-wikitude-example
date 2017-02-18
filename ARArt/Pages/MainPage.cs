using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ARArt.Models;
using ARArt.Views;
using ARArt.Common;

namespace ARArt.Pages
{
    public class MainPage : ContentPage
    {
        ListView listView;
        Label searchingLabel;
        ActivityIndicator spinner;
        IBeaconLocater beaconLocater;
        StackLayout tableLayout;
        StackLayout searchingLayout;
        ObservableCollection<BeaconItem> beaconCollection;
        ARPage arPage;
        int currentBeacon;
        const string debug = "3";

        public bool FoundBeacon;
        public string CurrentARViewId { get; set; }


        public MainPage()
        {
            FoundBeacon = false;
            Title = "Available Items";
            CurrentARViewId = "0";
            beaconLocater = DependencyService.Get<IBeaconLocater>();
            beaconCollection = new ObservableCollection<BeaconItem>();

            //Debug Button to start a AR View
            Button arButton = new Button {
                HorizontalOptions = LayoutOptions.Center,
                Text = "Debug ID " + debug
            };

            arButton.Clicked += (sender, e) => {
                arPage = new ARPage(debug);
                Navigation.PushAsync(arPage);
            };

            listView = new ListView {
                RowHeight = 100,
            };
            listView.ItemTemplate = new DataTemplate(typeof(BeaconCell));
            listView.ItemsSource = beaconCollection;
            listView.ItemTapped += (sender, e) => {
                BeaconFound(((BeaconItem)e.Item));
            };

            tableLayout = new StackLayout {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
                    listView
                }
            };

            searchingLabel = new Label {
                Text = "Please get closer to a picture",
                YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Center
            };

            spinner = new ActivityIndicator {
                IsRunning = true,
                Color = Color.Gray,
            };

            searchingLayout = new StackLayout {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children = {
                    spinner,
                    searchingLabel,
                    arButton
                }
            };

            Content = searchingLayout;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //Search every second for a Beacon
            var timer = new Timer(OnTimerElapsed, null, 0, 1000, true);

            App.mainPage = this;
        }

        public void OnTimerElapsed(object o)
        {
            //get Beacons
            Device.BeginInvokeOnMainThread(() => {
                var list = beaconLocater.GetAvailableBeacons();
                if (list == null) {
                    
                    return;
                } else if (list.Count == 0) {
                    
                    Content = searchingLayout;
                } else if (list.Count > 0) {
                    listView.ItemsSource = null;
                    //sort List - nearest first
                    list.Sort((beacon1, beacon2) => beacon1.CurrentDistance.CompareTo(beacon2.CurrentDistance));
                    listView.ItemsSource = list;

                    Content = tableLayout;

                    //start ARView if beacon is "NEAR" (20cm)
                    if (FoundBeacon == false) {
                        foreach (BeaconItem item in list) {
                            if (item.Proximity == Proximity.Immediate) {
                                if (currentBeacon != item.Minor) {

                                    if (FoundBeacon == false) {
                                        BeaconFound(item);
                                    }

                                    //Stop searching & starting ARView
                                    FoundBeacon = true; 
                                    currentBeacon = item.Minor;
                                }
                            }
                        }
                    }

                }
            });
        }

        public void ResumeTracking()
        {
            beaconLocater.ResumeTracking();
        }

        public void BeaconFound(BeaconItem item)
        {
            if (item.Minor >= 0 && item.Minor <= 3) {
                beaconLocater.PauseTracking();
                arPage = new ARPage(item.Minor.ToString());
                Navigation.PushAsync(arPage);
            }
        }
    }
}
