using LoanSystemLibraryMini;
using LoanSystemWPFMini.Managers.IManagers;
using LoanSystemWPFMini.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LoanSystemWPFMini.Managers
{
    public class EquipmentManager : IManager<Equipment>
    {
        private string _baseURL = "equipments";
        private APIHelper _apiHelper;

        public EquipmentManager(APIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<Equipment>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(_baseURL))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<Equipment> equipments = await response.Content.ReadAsAsync<List<Equipment>>();
                    return equipments;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<Equipment> Get(int id)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(_baseURL + "/" + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    Equipment equipment = await response.Content.ReadAsAsync<Equipment>();
                    return equipment;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<Equipment> Create(Equipment equipment)
        {
            string content = JsonConvert.SerializeObject(equipment);

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync(_baseURL, new StringContent(content, Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    Equipment createdEquipment = await response.Content.ReadAsAsync<Equipment>();
                    return createdEquipment;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task Update(Equipment equipment)
        {
            string content = JsonConvert.SerializeObject(equipment);

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PutAsync(_baseURL, new StringContent(content, Encoding.UTF8, "application/json")))
            {
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}