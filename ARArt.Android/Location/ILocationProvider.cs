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

namespace ARArt.Android
{
    /// <summary>
    /// I location provider.
    /// </summary>
    public interface ILocationProvider
    {
        void OnResume();

        void OnPause();
    }
}
