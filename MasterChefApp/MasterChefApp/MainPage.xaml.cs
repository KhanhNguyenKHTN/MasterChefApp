using MasterChefApp.Controls.Audio;
using MasterChefApp.Controls.List;
using MasterChefApp.Models;
using MasterChefApp.Services;
using Model.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Viewmodel.ViewModel.MainView;
using Xamarin.Forms;

namespace MasterChefApp
{
    public partial class MainPage : ContentPage
    {
        MainViewModel viewModel;
        RabbitConnect rabbit;
        List<OrderDetail> ListNotifi = new List<OrderDetail>();
        bool isShowingAlert = false;
        IAudioNoti audio;
        public MainPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<HorizontalListItem>(this, "AddChef", (s) =>
            {
                
            });
            MessagingCenter.Subscribe<HorizontalListItem, string>(this, "DescriptionTap", async (s, mess) =>
            {
                await DisplayAlert("Ghi chú", mess, "Ok");
            });
            MessagingCenter.Subscribe<RabbitConnect, OrderDetail>(this, "AddDetailQuere", (s, e) => {
                var check = viewModel.ListWaiting.FirstOrDefault(x => x.OrderDetailId == e.OrderDetailId);
                if (check != null) return;
                ListNotifi.Add(e);
                //viewModel.InsertOrderDetail(e);
                //Device.BeginInvokeOnMainThread(() =>
                //{
                //    lsList.AddLast(e);
                //});
                //string mess = "Đã thêm: "+ e.Quantity + " món (" + e.Dish.LabName + ") vào danh sách chờ";
                //Messages.Add(mess);
                DisplayNotifyCation();
            });
            viewModel = new MainViewModel();
            BindingContext = viewModel;
           // Connect();
            GenerateData();
            audio = DependencyService.Get<IAudioNoti>();
            audio.playAudio();
        }

        private async void Connect()
        {
            await Task.Delay(200);
            BackgroundWorker wk = new BackgroundWorker();
            wk.DoWork += (s, e) =>
            {
                rabbit = new RabbitConnect();
                rabbit.ReceiveNotifyRabbitMQ();
            };
            wk.RunWorkerAsync();
            
        }

        private void DisplayNotifyCation()
        {
            if (!isShowingAlert)
            {
                ShowMessage();
            }
        }

        private async void ShowMessage()
        {
            var e = ListNotifi.First();
            string mess = "Đã thêm: " + e.Quantity + " món (" + e.Dish.LabName + ") vào danh sách chờ";
            string original = "Đang chờ: " + viewModel.ListWaiting?.Count + " món";
            Device.BeginInvokeOnMainThread(() =>
            {
                isShowingAlert = true;
                audio.playAudio();
                viewModel.InsertOrderDetail(e);
                lsList.AddLast(e);
                Notify.Text = mess;

            });
            await Task.Delay(3000);

            ListNotifi.Remove(e);
            Device.BeginInvokeOnMainThread(() =>
            {
                isShowingAlert = false;
                Notify.Text = original;
            });
            if(ListNotifi.Count != 0)
            {
                ShowMessage();
            }
        }

        private async void GenerateData()
        {
            await Task.Delay(500);
            BackgroundWorker wk = new BackgroundWorker();
            wk.DoWork += async (s, e) =>
            {
                await viewModel.LoadData();
                string original = "Đang chờ: " + viewModel.ListWaiting?.Count + " món";
                Device.BeginInvokeOnMainThread(() =>
                {
                    isShowingAlert = false;
                    Notify.Text = original;
                });
                Connect();
            };
            wk.RunWorkerAsync();
            //while (true)
            //{
            //    await Task.Delay(5000);
            //    viewModel.AutoAdd(new Food() { LabName = "test" });
            //    lsList.AddFirst(new Food() { LabName = "aaaa", ImagesSource = @"https://image.shutterstock.com/image-photo/beautiful-water-drop-on-dandelion-260nw-789676552.jpg" });
            //}
        }

        private void PkChef_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
