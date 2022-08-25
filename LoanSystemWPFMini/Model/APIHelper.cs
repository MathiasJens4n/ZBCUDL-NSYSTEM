using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LoanSystemWPFMini.Model
{
    public class APIHelper
    {
        private string _baseURL = "http://10.125.169.3:26546/api/";
        public HttpClient ApiClient { get; set; }

        public void InitializeClient()
        {
            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri(_baseURL);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}