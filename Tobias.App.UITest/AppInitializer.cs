using System;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Tobias.App.UITest
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                return ConfigureApp.Android.InstalledApp("tobias.app.android").StartApp();
            }

            return ConfigureApp.iOS.StartApp();
        }
    }
}