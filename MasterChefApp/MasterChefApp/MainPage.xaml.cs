using MasterChefApp.Controls.List;
using MasterChefApp.Models;
using MasterChefApp.Services;
using Model.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public MainPage()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            rabbit = new RabbitConnect();
            rabbit.ReceiveNotifyRabbitMQ();
            BindingContext = viewModel;
            MessagingCenter.Subscribe<HorizontalListItem>(this, "AddChef", (s) =>
            {
                
            });
            MessagingCenter.Subscribe<HorizontalListItem>(this, "DescriptionTap", (s) =>
            {
                
            });
            MessagingCenter.Subscribe<RabbitConnect, OrderDetail>(this, "DishQuere", (s, e) => {
                viewModel.InsertOrderDetail(e);
                Device.BeginInvokeOnMainThread(() =>
                {
                    lsList.AddFirst(e);
                });
            });
            viewModel.IsLoadingWaiting = false;
            //GenerateData();
        }

        private async void GenerateData()
        {
            await Task.Delay(2000);
            viewModel.LoadWaitingPage();
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
