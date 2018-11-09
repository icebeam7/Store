using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MyStore.Models;
using Newtonsoft.Json;

namespace MyStore.Services
{
    public class StoreWebApiClient : HttpClient
    {
        private static readonly StoreWebApiClient instance = new StoreWebApiClient();
        static StoreWebApiClient() { }

        private StoreWebApiClient() : base()
        {
            Timeout = TimeSpan.FromMilliseconds(15000);
            MaxResponseContentBufferSize = 256000;
            BaseAddress = new Uri(Constants.StoreWebApiURL);
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static StoreWebApiClient Instance
        {
            get
            {
                return instance;
            }
        }

        public async Task<List<T>> GetItems<T>(string service)
        {
            var response = await GetAsync(service);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<T>>(content);
            }
            //throw new Exception(response.ReasonPhrase);
            return default(List<T>);
        }

        public async Task<List<T>> GetItems<T>(string service, string method)
        {
            var response = await GetAsync($"{service}/{method}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<T>>(content);
            }
            //throw new Exception(response.ReasonPhrase);
            return default(List<T>);
        }

        public async Task<T> GetItem<T>(string service, int id)
        {
            var response = await GetAsync($"{service}/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            //throw new Exception(response.ReasonPhrase);
            return default(T);
        }

        public async Task<T> GetItem<T>(string service, string method)
        {
            var response = await GetAsync($"{service}/{method}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            //throw new Exception(response.ReasonPhrase);
            return default(T);
        }

        public async Task<T> PostItem<T>(string service, T item)
        {
            var body = JsonConvert.SerializeObject(item);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await PostAsync(service, content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }
            //throw new Exception(response.ReasonPhrase);
            return default(T);
        }

        public async Task<bool> PutItem<T>(string service, T item, int id)
        {
            var body = JsonConvert.SerializeObject(item);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await PutAsync($"{service}/{id}", content);

            if (response.IsSuccessStatusCode)
                return true;
            //throw new Exception(response.ReasonPhrase);
            return false;
        }

        public async Task<bool> DeleteItem<T>(string service, int id)
        {
            var response = await DeleteAsync($"{service}/{id}");
            if (response.IsSuccessStatusCode)
                return true;
            //throw new Exception(response.ReasonPhrase);
            return false;
        }

        public async Task<EmployeeDTO> Login(string username, string password)
        {
            //var credentials = new { username = username, password = MD5Security.ToMD5Hash(password) };
            var credentials = new { username = "rrobles", password = MD5Security.ToMD5Hash("rrobles") };

            var body = JsonConvert.SerializeObject(credentials);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await PostAsync("Employees/Login", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<EmployeeDTO>(json);
            }
            //throw new Exception(response.ReasonPhrase);
            return default(EmployeeDTO);
        }

        public async Task<CustomerDTO> LoginCustomer(string username, string password)
        {
            var credentials = new { username = username, password = MD5Security.ToMD5Hash(password) };

            var body = JsonConvert.SerializeObject(credentials);
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await PostAsync("Customers/Login", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CustomerDTO>(json);
            }
            //throw new Exception(response.ReasonPhrase);
            return default(CustomerDTO);
        }
    }
}

