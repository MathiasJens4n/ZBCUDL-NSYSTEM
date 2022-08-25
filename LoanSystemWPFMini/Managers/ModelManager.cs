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
    public class ModelManager : IManager<LoanSystemLibraryMini.Model>
    {
        private string _baseURL = "Models";
        private APIHelper _apiHelper;

        public ModelManager(APIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<LoanSystemLibraryMini.Model>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(_baseURL))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<LoanSystemLibraryMini.Model> models = await response.Content.ReadAsAsync<List<LoanSystemLibraryMini.Model>>();
                    return models;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<LoanSystemLibraryMini.Model> Get(int id)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(_baseURL + $"/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    LoanSystemLibraryMini.Model model = await response.Content.ReadAsAsync<LoanSystemLibraryMini.Model>();
                    return model;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<LoanSystemLibraryMini.Model> Create(LoanSystemLibraryMini.Model model)
        {
            string content = JsonConvert.SerializeObject(model);

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync(_baseURL, new StringContent(content, Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    LoanSystemLibraryMini.Model createdModel = await response.Content.ReadAsAsync<LoanSystemLibraryMini.Model>();
                    return createdModel;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task Update(LoanSystemLibraryMini.Model model)
        {
            string content = JsonConvert.SerializeObject(model);

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