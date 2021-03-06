﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Model.Model;
using Xamarin.Forms;

namespace MasterChefApp.Services
{
    public class DataConvert
    {
        [JsonProperty("orderDetail")]
        public OrderDetail OrderDetail { get; set; }
    }
    public class JsonDetach
    {
        [JsonProperty("data")]
        public DataConvert Data { get; set; }
    }

    public class ResponseDetach
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
    public class RabbitConnect
    {
        public void ReceiveNotifyRabbitMQ()
        {
            try
            {
                ConnectionFactory factory = new ConnectionFactory();
                factory.UserName = "xarzdlrm";
                factory.Password = "Be9K-7C4C1sO9hiJTJwZSHITqm7NX6LS";
                factory.VirtualHost = "xarzdlrm";
                factory.HostName = "baboon.rmq.cloudamqp.com";
                IConnection conn = factory.CreateConnection();

                var channel = conn.CreateModel();

                var consumer = new EventingBasicConsumer(channel);
                var myChanel = channel.QueueDeclare("masterChefqueu").QueueName;
                channel.QueueBind(myChanel, "directExchange", "CHEF");

                Console.WriteLine("connecting to listen");
                consumer.Received += (model, ea) =>
                {
                    var tag = ea.ConsumerTag;
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    if (message != null)
                    {
                        var data = JsonConvert.DeserializeObject<ResponseDetach>(message);
                        var json = data.Text.Replace(@"\", "");

                        var realData = JsonConvert.DeserializeObject<JsonDetach>(json);

                        MessagingCenter.Send<RabbitConnect, OrderDetail>(this, "AddDetailQuere", realData.Data.OrderDetail);
                    }
                  

                };
                channel.BasicConsume(queue: myChanel,
                                     autoAck: true,
                                     consumer: consumer);

            }
            catch
            {
                Console.WriteLine("Khong the ket noi rabbit");
            }
        }
    }
}
