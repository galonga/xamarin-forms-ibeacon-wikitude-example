using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Wikitude.Architect;
using Xamarin.Forms.Platform.Android;
using ImageCircle.Forms.Plugin.Droid;


namespace ARArt.Android
{
    [Activity(Label = "ARArt.Android.Android", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AndroidActivity
    {
        /// <summary>
        /// Raises the create event.
        /// </summary>
        /// <param name="bundle">Bundle.</param>
        protected override void OnCreate(Bundle bundle)
        {
            //Wikitude ArchitectView
            ArchitectView architectView;

            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);

            ImageCircleRenderer.Init();

            SetPage(App.GetMainPage());
        }
    }
}
