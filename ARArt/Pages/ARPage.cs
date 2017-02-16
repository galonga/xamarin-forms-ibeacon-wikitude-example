using System;
using Xamarin.Forms;

namespace ARArt.Pages
{
    public class ARPage : ContentPage
    {
        public string ArObjectId;

        public ARPage(string arObjectId)
        {
            this.ArObjectId = arObjectId;
        }
    }
}
