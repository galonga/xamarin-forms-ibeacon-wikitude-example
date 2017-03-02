using System;
using Xamarin.Forms;
using ImageCircle.Forms.Plugin.Abstractions;

namespace ARArt.Views
{
    public class BeaconCell : ViewCell
    {
        public BeaconCell()
        {
            int photoHeight = Device.OnPlatform(50, 50, 80);
            int photoWidth = Device.OnPlatform(80, 50, 80);

            var iconCircleImage = new Image {
                HeightRequest = photoHeight,
                WidthRequest = photoWidth,
                Aspect = Aspect.Fill,
                HorizontalOptions = LayoutOptions.Center,
            };

            iconCircleImage.SetBinding(Image.SourceProperty, "ImageID");

            var nameLabel = new Label {
                YAlign = TextAlignment.Center,
                Text = "Exhibit: "
            };

            var nameContent = new Label {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            nameContent.SetBinding(Label.TextProperty, "Name");

            nameContent.Focused += delegate {
                DependencyService.Get<IBeaconLocater>().PauseTracking();
            };

            nameContent.Unfocused += delegate {
                DependencyService.Get<IBeaconLocater>().ResumeTracking();
            };

            var nameLayout = new StackLayout {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 0, 5, 0),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { nameLabel, nameContent }
            };

            var idLabel = new Label {
                YAlign = TextAlignment.Center,
                Text = "ID: "
            };

            var idContent = new Label {
                YAlign = TextAlignment.Center
            };

            idContent.SetBinding(Label.TextProperty, "Minor");

            var idLayout = new StackLayout {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 0, 5, 0),
                Children = { idLabel, idContent }
            };

            var distanceLabel = new Label {
                YAlign = TextAlignment.Center,
                Text = "Distance: ",
            };

            var distanceContent = new Label {
                YAlign = TextAlignment.Center,
                TextColor = Color.Red
            };

            distanceContent.SetBinding(Label.TextProperty, "DistanceString");

            var distanceLayout = new StackLayout {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(5, 0, 5, 0),
                Children = { distanceLabel, distanceContent }
            };

            var layout = new StackLayout {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(20, 10, 0, 10),
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children = {
                    iconCircleImage,
                    new StackLayout {
                        Padding = new Thickness (20, 10, 0, 10),
                        Orientation = StackOrientation.Vertical,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        VerticalOptions = LayoutOptions.StartAndExpand,
                        Children = { nameLayout,
                            new StackLayout{
                                Orientation = StackOrientation.Horizontal,
                                Padding = new Thickness (20, 10, 0, 10),
                                VerticalOptions = LayoutOptions.StartAndExpand,
                                Children = {
                                    idLayout,
                                    distanceLayout
                                }
                            }
                        }
                    }
                }
            };

            View = layout;
        }
    }
}