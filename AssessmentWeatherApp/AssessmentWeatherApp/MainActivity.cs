using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace AssessmentWeatherApp
{
    [Activity(Label = "AssessmentWeatherApp", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        LinearLayout llBackground;

        TextView txtCityName, txtTempNow, txtCondNow, txtDateNow, txtMaxMinNow;
        TextView txtDay1, txtDay2, txtDay3, txtDay4, txtDay5;
        TextView txtMin1, txtMin2, txtMin3, txtMin4, txtMin5;
        TextView txtMax1, txtMax2, txtMax3, txtMax4, txtMax5;               

        ImageView imgCondNow, imgDay1, imgDay2, imgDay3, imgDay4, imgDay5;

        RESTHandler objRest;

        RootObject objRootObj;

        string apiLocation, imgIcon, passImgIcon, nightTime, currentTime;

        int currentHour;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);           

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Weather);            

            txtCityName = FindViewById<TextView>(Resource.Id.txtCityName);
            txtTempNow = FindViewById<TextView>(Resource.Id.txtTempNow);
            txtCondNow = FindViewById<TextView>(Resource.Id.txtCondNow);
            txtDateNow = FindViewById<TextView>(Resource.Id.txtDateNow);
            txtMaxMinNow = FindViewById<TextView>(Resource.Id.txtMaxMinNow);           

            txtDay1 = FindViewById<TextView>(Resource.Id.txtDay1);
            txtDay2 = FindViewById<TextView>(Resource.Id.txtDay2);
            txtDay3 = FindViewById<TextView>(Resource.Id.txtDay3);
            txtDay4 = FindViewById<TextView>(Resource.Id.txtDay4);
            txtDay5 = FindViewById<TextView>(Resource.Id.txtDay5);

            txtMin1 = FindViewById<TextView>(Resource.Id.txtMin1);
            txtMin2 = FindViewById<TextView>(Resource.Id.txtMin2);
            txtMin3 = FindViewById<TextView>(Resource.Id.txtMin3);
            txtMin4 = FindViewById<TextView>(Resource.Id.txtMin4);
            txtMin5 = FindViewById<TextView>(Resource.Id.txtMin5);

            txtMax1 = FindViewById<TextView>(Resource.Id.txtMax1);
            txtMax2 = FindViewById<TextView>(Resource.Id.txtMax2);
            txtMax3 = FindViewById<TextView>(Resource.Id.txtMax3);
            txtMax4 = FindViewById<TextView>(Resource.Id.txtMax4);
            txtMax5 = FindViewById<TextView>(Resource.Id.txtMax5);

            imgCondNow = FindViewById<ImageView>(Resource.Id.imgCondNow);
            imgDay1 = FindViewById<ImageView>(Resource.Id.imgDay1);
            imgDay2 = FindViewById<ImageView>(Resource.Id.imgDay2);
            imgDay3 = FindViewById<ImageView>(Resource.Id.imgDay3);
            imgDay4 = FindViewById<ImageView>(Resource.Id.imgDay4);
            imgDay5 = FindViewById<ImageView>(Resource.Id.imgDay5);

            currentHour = DateTime.Now.Hour;

            llBackground = FindViewById<LinearLayout>(Resource.Id.llBackground);
            if (currentHour >= 8 && currentHour < 18)
            {
                llBackground.SetBackgroundResource(Resource.Drawable.backgroundday);
            }
            else if (currentHour >= 20 && currentHour < 5)
            {
                nightTime = "night";
                llBackground.SetBackgroundResource(Resource.Drawable.backgroundnight);
            }
            else
            {
                llBackground.SetBackgroundResource(Resource.Drawable.backgrounddusk);
            }

            txtCityName.Text = "Hamilton";
            apiLocation = "zmw:00000.3.93186";

            imgCondNow.SetImageResource(Resource.Drawable.weatherunknown);
            imgDay1.SetImageResource(Resource.Drawable.weatherunknown);
            imgDay2.SetImageResource(Resource.Drawable.weatherunknown);
            imgDay3.SetImageResource(Resource.Drawable.weatherunknown);
            imgDay4.SetImageResource(Resource.Drawable.weatherunknown);
            imgDay5.SetImageResource(Resource.Drawable.weatherunknown);

            LoadWeatherData();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            menu.Add("Auckland");
            menu.Add("Hamilton");
            menu.Add("Wellington");
            menu.Add("Christchurch");
            menu.Add("Exit");

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.TitleFormatted.ToString() == "Auckland")
            {
                txtCityName.Text = "Auckland";
                apiLocation = "NewZealand/Auckland";
            }
            else if (item.TitleFormatted.ToString() == "Hamilton")
            {
                txtCityName.Text = "Hamilton";
                apiLocation = "zmw: 00000.3.93186";
            }
            else if (item.TitleFormatted.ToString() == "Wellington")
            {
                txtCityName.Text = "Wellington";
                apiLocation = "zmw:00000.1.93436";
            }
            else if (item.TitleFormatted.ToString() == "Christchurch")
            {
                txtCityName.Text = "Christchurch";
                apiLocation = "zmw:00000.1.93780";
            }
            else if (item.TitleFormatted.ToString() == "Exit")
            {
                this.Finish();
            }

            LoadWeatherData();
            return base.OnOptionsItemSelected(item);
        }

        private async void LoadWeatherData()         
        {
            currentTime = DateTime.Now.ToString();
            txtDateNow.Text = currentTime;

            try
            {
                objRest = new RESTHandler(@"http://api.wunderground.com/api/0495186545d6aae5/geolookup/conditions/forecast10day/q/" + apiLocation + ".json");

                objRootObj = await objRest.ExecuteRequestAsync();
                
                DisplayWeatherToday();
                DisplayWeatherForecast();
            }
            catch (Exception e)
            {
                Toast.MakeText(this, "Error: " + e.Message, ToastLength.Long);
            }
        }

        private void DisplayWeatherToday()
        {
            imgIcon = objRootObj.current_observation.icon;
            passImgIcon = imgIcon;

            if (nightTime != "night")
            {
                GetWeatherImageDay(imgIcon);
            }
            else
            {
                GetWeatherImageNight(imgIcon);
            }

            txtTempNow.Text = objRootObj.current_observation.temp_c.ToString() + " °C";

            if (objRootObj.current_observation.weather != "")
            {
                txtCondNow.Text = objRootObj.current_observation.weather;
            }
            else
            {
                txtCondNow.Text = objRootObj.forecast.simpleforecast.forecastday[0].conditions;
            }

            txtMaxMinNow.Text = "Max " + objRootObj.forecast.simpleforecast.forecastday[0].high.celsius.ToString() + " °C / Min " + objRootObj.forecast.simpleforecast.forecastday[0].low.celsius.ToString() + " °C";            
        }

        private void DisplayWeatherForecast() 
        {
            txtDay1.Text = objRootObj.forecast.simpleforecast.forecastday[1].date.day.ToString() + " " + objRootObj.forecast.simpleforecast.forecastday[1].date.monthname_short.ToString();
            txtMax1.Text = objRootObj.forecast.simpleforecast.forecastday[1].high.celsius.ToString() + " °C";
            txtMin1.Text = objRootObj.forecast.simpleforecast.forecastday[1].low.celsius.ToString() + " °C";
            imgIcon = objRootObj.forecast.simpleforecast.forecastday[1].icon;
            GetWeatherImageDay1(imgIcon);

            txtDay2.Text = objRootObj.forecast.simpleforecast.forecastday[2].date.day.ToString() + " " + objRootObj.forecast.simpleforecast.forecastday[2].date.monthname_short.ToString();
            txtMax2.Text = objRootObj.forecast.simpleforecast.forecastday[2].high.celsius.ToString() + " °C";
            txtMin2.Text = objRootObj.forecast.simpleforecast.forecastday[2].low.celsius.ToString() + " °C";
            imgIcon = objRootObj.forecast.simpleforecast.forecastday[2].icon;
            GetWeatherImageDay2(imgIcon);

            txtDay3.Text = objRootObj.forecast.simpleforecast.forecastday[3].date.day.ToString() + " " + objRootObj.forecast.simpleforecast.forecastday[3].date.monthname_short.ToString();
            txtMax3.Text = objRootObj.forecast.simpleforecast.forecastday[3].high.celsius.ToString() + " °C";
            txtMin3.Text = objRootObj.forecast.simpleforecast.forecastday[3].low.celsius.ToString() + " °C";
            imgIcon = objRootObj.forecast.simpleforecast.forecastday[3].icon;
            GetWeatherImageDay3(imgIcon);

            txtDay4.Text = objRootObj.forecast.simpleforecast.forecastday[4].date.day.ToString() + " " + objRootObj.forecast.simpleforecast.forecastday[4].date.monthname_short.ToString();
            txtMax4.Text = objRootObj.forecast.simpleforecast.forecastday[4].high.celsius.ToString() + " °C";
            txtMin4.Text = objRootObj.forecast.simpleforecast.forecastday[4].low.celsius.ToString() + " °C";
            imgIcon = objRootObj.forecast.simpleforecast.forecastday[4].icon;
            GetWeatherImageDay4(imgIcon);

            txtDay5.Text = objRootObj.forecast.simpleforecast.forecastday[5].date.day.ToString() + " " + objRootObj.forecast.simpleforecast.forecastday[5].date.monthname_short.ToString();
            txtMax5.Text = objRootObj.forecast.simpleforecast.forecastday[5].high.celsius.ToString() + " °C";
            txtMin5.Text = objRootObj.forecast.simpleforecast.forecastday[5].low.celsius.ToString() + " °C";
            imgIcon = objRootObj.forecast.simpleforecast.forecastday[5].icon;
            GetWeatherImageDay5(imgIcon);
        }

        public void GetWeatherImageDay(string imgicon)
        {
            if (imgicon == "clear" || imgicon == "sunny")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weatherclear);
            }
            else if (imgicon == "cloudy")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weatherclouds);
            }
            else if (imgicon == "mostlycloudy" || imgicon == "partlysunny")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weathermanyclouds);
            }
            else if (imgicon == "partlycloudy" || imgicon == "mostlysunny")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weatherfewclouds);
            }
            else if (imgicon == "chancerain")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weathershowersday);
            }
            else if (imgicon == "rain")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weathershowers);
            }
            else if (imgicon == "chancesnow")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weathersnowscatteredday);
            }
            else if (imgicon == "snow")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weathersnow);
            }
            else if (imgicon == "chancetstorms")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weatherstormday);
            }
            else if (imgicon == "tstorms")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weatherstorm);
            }
            else if (imgicon == "chanceflurries" || imgicon == "flurries")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weathersnowscattered);
            }
            else if (imgicon == "chancesleet" || imgicon == "sleet")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weathersnowrain);
            }
            else if (imgicon == "fog")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weatherfog);
            }
            else if (imgicon == "hazy")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weatherhazy);
            }
            else if (imgicon == "unknown" || imgicon == "")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weatherunknown);
            }
        }

        public void GetWeatherImageNight(string imgicon)
        {
            if (imgicon == "clear")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weatherclearnight);
            }
            else if (imgicon == "cloudy")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weathermanyclouds);
            }
            else if (imgicon == "mostlycloudy")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weathermanyclouds);
            }
            else if (imgicon == "partlycloudy")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weatherfewcloudsnight);
            }
            else if (imgicon == "chancerain")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weathershowersscatterednight);
            }
            else if (imgicon == "rain")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weathershowersnight);
            }
            else if (imgicon == "chancesnow")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weathersnowscatterednight);
            }
            else if (imgicon == "snow")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weathersnow);
            }
            else if (imgicon == "chancetstorms")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weatherstormnight);
            }
            else if (imgicon == "tstorms")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weatherstorm);
            }
            else if (imgicon == "chanceflurries" || imgicon == "flurries")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weathersnowscattered);
            }
            else if (imgicon == "chancesleet" || imgicon == "sleet")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weathersnowrain);
            }
            else if (imgicon == "fog")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weatherfog);
            }
            else if (imgicon == "hazy")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weatherhazy);
            }
            else if (imgicon == "unknown" || imgicon == "")
            {
                imgCondNow.SetImageResource(Resource.Drawable.weatherunknown);
            }
        }

        public void GetWeatherImageDay1(string imgicon)
        {
            if (imgicon == "clear" || imgicon == "sunny")
            {
                imgDay1.SetImageResource(Resource.Drawable.weatherclear);
            }
            else if (imgicon == "cloudy")
            {
                imgDay1.SetImageResource(Resource.Drawable.weatherclouds);
            }
            else if (imgicon == "mostlycloudy" || imgicon == "partlysunny")
            {
                imgDay1.SetImageResource(Resource.Drawable.weathermanyclouds);
            }
            else if (imgicon == "partlycloudy" || imgicon == "mostlysunny")
            {
                imgDay1.SetImageResource(Resource.Drawable.weatherfewclouds);
            }
            else if (imgicon == "chancerain")
            {
                imgDay1.SetImageResource(Resource.Drawable.weathershowersday);
            }
            else if (imgicon == "rain")
            {
                imgDay1.SetImageResource(Resource.Drawable.weathershowers);
            }
            else if (imgicon == "chancesnow")
            {
                imgDay1.SetImageResource(Resource.Drawable.weathersnowscatteredday);
            }
            else if (imgicon == "snow")
            {
                imgDay1.SetImageResource(Resource.Drawable.weathersnow);
            }
            else if (imgicon == "chancetstorms")
            {
                imgDay1.SetImageResource(Resource.Drawable.weatherstormday);
            }
            else if (imgicon == "tstorms")
            {
                imgDay1.SetImageResource(Resource.Drawable.weatherstorm);
            }
            else if (imgicon == "chanceflurries" || imgicon == "flurries")
            {
                imgDay1.SetImageResource(Resource.Drawable.weathersnowscattered);
            }
            else if (imgicon == "chancesleet" || imgicon == "sleet")
            {
                imgDay1.SetImageResource(Resource.Drawable.weathersnowrain);
            }
            else if (imgicon == "fog")
            {
                imgDay1.SetImageResource(Resource.Drawable.weatherfog);
            }
            else if (imgicon == "hazy")
            {
                imgDay1.SetImageResource(Resource.Drawable.weatherhazy);
            }
            else if (imgicon == "unknown" || imgicon == "")
            {
                imgDay1.SetImageResource(Resource.Drawable.weatherunknown);
            }
        }

        public void GetWeatherImageDay2(string imgicon)
        {
            if (imgicon == "clear" || imgicon == "sunny")
            {
                imgDay2.SetImageResource(Resource.Drawable.weatherclear);
            }
            else if (imgicon == "cloudy")
            {
                imgDay2.SetImageResource(Resource.Drawable.weatherclouds);
            }
            else if (imgicon == "mostlycloudy" || imgicon == "partlysunny")
            {
                imgDay2.SetImageResource(Resource.Drawable.weathermanyclouds);
            }
            else if (imgicon == "partlycloudy" || imgicon == "mostlysunny")
            {
                imgDay2.SetImageResource(Resource.Drawable.weatherfewclouds);
            }
            else if (imgicon == "chancerain")
            {
                imgDay2.SetImageResource(Resource.Drawable.weathershowersday);
            }
            else if (imgicon == "rain")
            {
                imgDay2.SetImageResource(Resource.Drawable.weathershowers);
            }
            else if (imgicon == "chancesnow")
            {
                imgDay2.SetImageResource(Resource.Drawable.weathersnowscatteredday);
            }
            else if (imgicon == "snow")
            {
                imgDay2.SetImageResource(Resource.Drawable.weathersnow);
            }
            else if (imgicon == "chancetstorms")
            {
                imgDay2.SetImageResource(Resource.Drawable.weatherstormday);
            }
            else if (imgicon == "tstorms")
            {
                imgDay2.SetImageResource(Resource.Drawable.weatherstorm);
            }
            else if (imgicon == "chanceflurries" || imgicon == "flurries")
            {
                imgDay2.SetImageResource(Resource.Drawable.weathersnowscattered);
            }
            else if (imgicon == "chancesleet" || imgicon == "sleet")
            {
                imgDay2.SetImageResource(Resource.Drawable.weathersnowrain);
            }
            else if (imgicon == "fog")
            {
                imgDay2.SetImageResource(Resource.Drawable.weatherfog);
            }
            else if (imgicon == "hazy")
            {
                imgDay2.SetImageResource(Resource.Drawable.weatherhazy);
            }
            else if (imgicon == "unknown" || imgicon == "")
            {
                imgDay2.SetImageResource(Resource.Drawable.weatherunknown);
            }
        }

        public void GetWeatherImageDay3(string imgicon)
        {
            if (imgicon == "clear" || imgicon == "sunny")
            {
                imgDay3.SetImageResource(Resource.Drawable.weatherclear);
            }
            else if (imgicon == "cloudy")
            {
                imgDay3.SetImageResource(Resource.Drawable.weatherclouds);
            }
            else if (imgicon == "mostlycloudy" || imgicon == "partlysunny")
            {
                imgDay3.SetImageResource(Resource.Drawable.weathermanyclouds);
            }
            else if (imgicon == "partlycloudy" || imgicon == "mostlysunny")
            {
                imgDay3.SetImageResource(Resource.Drawable.weatherfewclouds);
            }
            else if (imgicon == "chancerain")
            {
                imgDay3.SetImageResource(Resource.Drawable.weathershowersday);
            }
            else if (imgicon == "rain")
            {
                imgDay3.SetImageResource(Resource.Drawable.weathershowers);
            }
            else if (imgicon == "chancesnow")
            {
                imgDay3.SetImageResource(Resource.Drawable.weathersnowscatteredday);
            }
            else if (imgicon == "snow")
            {
                imgDay3.SetImageResource(Resource.Drawable.weathersnow);
            }
            else if (imgicon == "chancetstorms")
            {
                imgDay3.SetImageResource(Resource.Drawable.weatherstormday);
            }
            else if (imgicon == "tstorms")
            {
                imgDay3.SetImageResource(Resource.Drawable.weatherstorm);
            }
            else if (imgicon == "chanceflurries" || imgicon == "flurries")
            {
                imgDay3.SetImageResource(Resource.Drawable.weathersnowscattered);
            }
            else if (imgicon == "chancesleet" || imgicon == "sleet")
            {
                imgDay3.SetImageResource(Resource.Drawable.weathersnowrain);
            }
            else if (imgicon == "fog")
            {
                imgDay3.SetImageResource(Resource.Drawable.weatherfog);
            }
            else if (imgicon == "hazy")
            {
                imgDay3.SetImageResource(Resource.Drawable.weatherhazy);
            }
            else if (imgicon == "unknown" || imgicon == "")
            {
                imgDay3.SetImageResource(Resource.Drawable.weatherunknown);
            }
        }

        public void GetWeatherImageDay4(string imgicon)
        {
            if (imgicon == "clear" || imgicon == "sunny")
            {
                imgDay4.SetImageResource(Resource.Drawable.weatherclear);
            }
            else if (imgicon == "cloudy")
            {
                imgDay4.SetImageResource(Resource.Drawable.weatherclouds);
            }
            else if (imgicon == "mostlycloudy" || imgicon == "partlysunny")
            {
                imgDay4.SetImageResource(Resource.Drawable.weathermanyclouds);
            }
            else if (imgicon == "partlycloudy" || imgicon == "mostlysunny")
            {
                imgDay4.SetImageResource(Resource.Drawable.weatherfewclouds);
            }
            else if (imgicon == "chancerain")
            {
                imgDay4.SetImageResource(Resource.Drawable.weathershowersday);
            }
            else if (imgicon == "rain")
            {
                imgDay4.SetImageResource(Resource.Drawable.weathershowers);
            }
            else if (imgicon == "chancesnow")
            {
                imgDay4.SetImageResource(Resource.Drawable.weathersnowscatteredday);
            }
            else if (imgicon == "snow")
            {
                imgDay4.SetImageResource(Resource.Drawable.weathersnow);
            }
            else if (imgicon == "chancetstorms")
            {
                imgDay4.SetImageResource(Resource.Drawable.weatherstormday);
            }
            else if (imgicon == "tstorms")
            {
                imgDay4.SetImageResource(Resource.Drawable.weatherstorm);
            }
            else if (imgicon == "chanceflurries" || imgicon == "flurries")
            {
                imgDay4.SetImageResource(Resource.Drawable.weathersnowscattered);
            }
            else if (imgicon == "chancesleet" || imgicon == "sleet")
            {
                imgDay4.SetImageResource(Resource.Drawable.weathersnowrain);
            }
            else if (imgicon == "fog")
            {
                imgDay4.SetImageResource(Resource.Drawable.weatherfog);
            }
            else if (imgicon == "hazy")
            {
                imgDay4.SetImageResource(Resource.Drawable.weatherhazy);
            }
            else if (imgicon == "unknown" || imgicon == "")
            {
                imgDay4.SetImageResource(Resource.Drawable.weatherunknown);
            }
        }

        public void GetWeatherImageDay5(string imgicon)
        {
            if (imgicon == "clear" || imgicon == "sunny")
            {
                imgDay5.SetImageResource(Resource.Drawable.weatherclear);
            }
            else if (imgicon == "cloudy")
            {
                imgDay5.SetImageResource(Resource.Drawable.weatherclouds);
            }
            else if (imgicon == "mostlycloudy" || imgicon == "partlysunny")
            {
                imgDay5.SetImageResource(Resource.Drawable.weathermanyclouds);
            }
            else if (imgicon == "partlycloudy" || imgicon == "mostlysunny")
            {
                imgDay5.SetImageResource(Resource.Drawable.weatherfewclouds);
            }
            else if (imgicon == "chancerain")
            {
                imgDay5.SetImageResource(Resource.Drawable.weathershowersday);
            }
            else if (imgicon == "rain")
            {
                imgDay5.SetImageResource(Resource.Drawable.weathershowers);
            }
            else if (imgicon == "chancesnow")
            {
                imgDay5.SetImageResource(Resource.Drawable.weathersnowscatteredday);
            }
            else if (imgicon == "snow")
            {
                imgDay5.SetImageResource(Resource.Drawable.weathersnow);
            }
            else if (imgicon == "chancetstorms")
            {
                imgDay5.SetImageResource(Resource.Drawable.weatherstormday);
            }
            else if (imgicon == "tstorms")
            {
                imgDay5.SetImageResource(Resource.Drawable.weatherstorm);
            }
            else if (imgicon == "chanceflurries" || imgicon == "flurries")
            {
                imgDay5.SetImageResource(Resource.Drawable.weathersnowscattered);
            }
            else if (imgicon == "chancesleet" || imgicon == "sleet")
            {
                imgDay5.SetImageResource(Resource.Drawable.weathersnowrain);
            }
            else if (imgicon == "fog")
            {
                imgDay5.SetImageResource(Resource.Drawable.weatherfog);
            }
            else if (imgicon == "hazy")
            {
                imgDay5.SetImageResource(Resource.Drawable.weatherhazy);
            }
            else if (imgicon == "unknown" || imgicon == "")
            {
                imgDay5.SetImageResource(Resource.Drawable.weatherunknown);
            }
        }
    }
}

