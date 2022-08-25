using LoanSystemLibraryMini;
using LoanSystemWPFMini.Managers.IManagers;
using LoanSystemWPFMini.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LoanSystemWPFMini.Managers
{
    internal class DepartmentManager : IBaseManager<Department>
    {
        private string _baseURL = "departments";
        private APIHelper _apiHelper;

        public DepartmentManager(APIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<Department>> GetAll()
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(_baseURL))
            {
                if (response.IsSuccessStatusCode)
                {
                    List<Department> departmentList = await response.Content.ReadAsAsync<List<Department>>();
                    return departmentList;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
