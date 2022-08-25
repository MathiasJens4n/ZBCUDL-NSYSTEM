using LoanSystemWPFMini.Model;
using LoanSystemWPFMini.Managers.IManagers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LoanSystemWPFMini.Managers
{
    public class TypeManager : IManager<LoanSystemLibraryMini.Type>
    {
        private string _baseURL = "Types";
        private APIHelper _apiHelper;

        public TypeManager(APIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<LoanSystemLibraryMini.Type>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(_baseURL))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<LoanSystemLibraryMini.Type> types = await response.Content.ReadAsAsync<List<LoanSystemLibraryMini.Type>>();
                    return types;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<LoanSystemLibraryMini.Type> Get(int id)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(_baseURL + $"/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    LoanSystemLibraryMini.Type type = await response.Content.ReadAsAsync<LoanSystemLibraryMini.Type>();
                    return type;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<LoanSystemLibraryMini.Type> Create(LoanSystemLibraryMini.Type type)
        {
            string content = JsonConvert.SerializeObject(type);

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync(_baseURL, new StringContent(content, Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    LoanSystemLibraryMini.Type createdType = await response.Content.ReadAsAsync<LoanSystemLibraryMini.Type>();
                    return createdType;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task Update(LoanSystemLibraryMini.Type type)
        {
            string content = JsonConvert.SerializeObject(type);

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PutAsync(_baseURL, new StringContent(content, Encoding.UTF8, "application/json")))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task Delete(int id)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.DeleteAsync(_baseURL + $"/{id}"))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}