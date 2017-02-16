using System;
using ARArt.Pages;
using Xamarin.Forms;

namespace ARArt
{
    public class App
    {
        public static MainPage mainPage;

        public static Page GetMainPage()
        {
            var mainNav = new NavigationPage(new MainPage());

            return mainNav;
        }
    }
}
