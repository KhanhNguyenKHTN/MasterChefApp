using Model.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class JsonDetach
    {
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public ObservableCollection<OrderDetail> Data { get; set; }
    }
    public class MainService
    {
        private HttpClient client;

        public MainService()
        {
            client = new HttpClient() { BaseAddress = new Uri(Global.GlobalInfo.BaseUrl) };
        }

        public async Task<ObservableCollection<OrderDetail>> GetListOrderByStatus(string status)
        {
            try
            {
                string url = @"/order-detail/status/" + status;

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    //JsonDetach det = new JsonDetach();
                    var Items = JsonConvert.DeserializeObject<JsonDetach>(content);

                    return Items.Data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        public async Task<ObservableCollection<OrderDetail>> GetListPicAndStatus(Pic cus, string status)
        {
            try
            {
                string url = @"/order-detail/pic/"+ cus.UserInfo.UserId +@"/type/" + status;

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    //JsonDetach det = new JsonDetach();
                    var Items = JsonConvert.DeserializeObject<JsonDetach>(content);

                    return Items.Data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
