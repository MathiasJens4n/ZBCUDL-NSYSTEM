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
    public class LoanManager : IManager<Loan>
    {
        private string _baseURL = "loans";
        private APIHelper _apiHelper;

        public LoanManager(APIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<Loan>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(_baseURL))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<Loan> loans = await response.Content.ReadAsAsync<List<Loan>>();
                    return loans;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<Loan> Get(int id)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(_baseURL + $"/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    Loan loan = await response.Content.ReadAsAsync<Loan>();
                    return loan;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<Loan> Create(Loan loan)
        {
            string content = JsonConvert.SerializeObject(loan);

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync(_baseURL, new StringContent(content, Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    Loan createdLoan = await response.Content.ReadAsAsync<Loan>();
                    return createdLoan;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task Update(Loan loan)
        {
            string content = JsonConvert.SerializeObject(loan);

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