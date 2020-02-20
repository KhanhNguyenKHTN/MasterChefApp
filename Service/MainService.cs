﻿using Model.Model;
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
                    return new ObservableCollection<OrderDetail>();
                }
            }
            catch (Exception ex)
            {

                return new ObservableCollection<OrderDetail>();
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
                    return new ObservableCollection<OrderDetail>();
                }
            }
            catch (Exception ex)
            {
                return new ObservableCollection<OrderDetail>();
            }
        }

        public async Task<bool> AssignChef(Pic pic, OrderDetail detail)
        {
            try
            {
                string url = @"/order-detail/pic/id/" +detail.OrderDetailId;
                string json = @"{ ""Id"": " + detail.OrderDetailId + @" , ""empId"": " + pic.EmployeeId + @" }";
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {

                }
                else
                {
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
