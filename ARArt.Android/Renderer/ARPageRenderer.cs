using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using ARArt.Pages;

[assembly: ExportRenderer(typeof(ARArt.Pages.ARPage), typeof(ARArt.Android.Renderer.ARPageRenderer))]
namespace ARArt.Android.Renderer
{
    /// <summary>
    /// wikitude AR View renderer
    /// </summary>
    public class ARPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            var page = e.NewElement as ARPage;

            var activity = this.Context as Activity;

            var wikitudeActivity = new Intent(activity, typeof(WikitudeARActivity));
            wikitudeActivity.PutExtra("id", page.ArObjectId);

            activity.StartActivity(wikitudeActivity);
        }
    }
}
