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

using RestSharp;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AssessmentWeatherApp
{
    public class RESTHandler
    {
        private string url;
        private IRestResponse response;
        private RestRequest request;

        public RESTHandler()
        {
            url = "";
        }

        public RESTHandler(string lurl)
        {
            url = lurl;
            request = new RestRequest();
        }

        public void AddParameter(string name, string value)
        {
            if (request != null)
            {
                request.AddParameter(name, value);
            }
        }

        public async Task<RootObject> ExecuteRequestAsync()
        {
            var client = new RestClient(url);

            response = await client.ExecuteTaskAsync(request);

            RootObject objRoot = new RootObject();
            objRoot = JsonConvert.DeserializeObject<RootObject>(response.Content);

            return objRoot;
        }
    }
}