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
    public class LoanRuleManager : IManager<LoanRule>
    {
        private string _baseURL = "loanrules";
        private APIHelper _apiHelper;

        public LoanRuleManager(APIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<LoanRule>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(_baseURL))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<LoanRule> loanrules = await response.Content.ReadAsAsync<List<LoanRule>>();
                    return loanrules;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<LoanRule> Get(int id)
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(_baseURL + $"/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    LoanRule loanrule = await response.Content.ReadAsAsync<LoanRule>();
                    return loanrule;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task<LoanRule> Create(LoanRule loanRule)
        {
            string content = JsonConvert.SerializeObject(loanRule);

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync(_baseURL, new StringContent(content, Encoding.UTF8, "application/json")))
            {
                if (response.IsSuccessStatusCode)
                {
                    LoanRule createdLoanRule = await response.Content.ReadAsAsync<LoanRule>();
                    return createdLoanRule;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public async Task Update(LoanRule loanRule)
        {
            string content = JsonConvert.SerializeObject(loanRule);

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