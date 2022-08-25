using LoanSystemLibraryMini;
using LoanSystemWPFMini.Managers.IManagers;
using LoanSystemWPFMini.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LoanSystemWPFMini.Managers
{
    public class StatusManager : IBaseManager<Status>
    {
        private string _baseURL = "statuses";
        private APIHelper _apiHelper;

        public StatusManager(APIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<Status>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(_baseURL))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<Status> statuses = await response.Content.ReadAsAsync<List<Status>>();
                    return statuses;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}