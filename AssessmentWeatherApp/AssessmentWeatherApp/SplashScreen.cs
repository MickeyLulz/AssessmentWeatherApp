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
using System.Threading;

namespace AssessmentWeatherApp
{
    [Activity(Label = "NZ Weather", MainLauncher = true, NoHistory = true, Theme = "@style/Theme.Splash", Icon = "@drawable/nzweather", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Thread.Sleep(100);

            StartActivity(typeof(MainActivity));
        }
    }
}