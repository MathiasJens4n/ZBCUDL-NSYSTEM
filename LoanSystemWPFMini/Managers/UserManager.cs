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
    public class UserManager : IManager<User>
    {
        private string _baseURL = "Users";
        private APIHelper _apiHelper;

        public UserManager(APIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<User>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(_baseURL))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<User> Users = await response.Content.ReadAsAsync<List<User>>();
                    return Users;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<User> Get(int id)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(_baseURL + "/" + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    User User = await response.Content.ReadAsAsync<User>();
                    return User;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<User> Create(User user)
        {
            string content = JsonConvert.SerializeObject(user);

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync(_baseURL, new StringContent(content, Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    User createdUser = await response.Content.ReadAsAsync<User>();
                    return createdUser;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task Update(User user)
        {
            string content = JsonConvert.SerializeObject(user);

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