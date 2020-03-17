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
        int timeWait = 3000;
        public MainPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<HorizontalListItem, OrderDetail>(this, "AddChef", (s, detail) =>
            {
                viewModel.SelectedDetail = detail;
                gridSelectedChef.IsVisible = true;
            });
            MessagingCenter.Subscribe<HorizontalListItem, string>(this, "DescriptionTap", async (s, mess) =>
            {
                await DisplayAlert("Ghi chú", mess, "Ok");
            });

            MessagingCenter.Subscribe<HorizontalListItem, OrderDetail>(this, "ChangeStatus", (s, mess) =>
            {
                ChangeStatus(mess);
            });
            MessagingCenter.Subscribe<RabbitConnect, OrderDetail>(this, "AddDetailQuere", (s, e) => {
                
                ListNotifi.Add(e);
                DisplayNotifyCation();
            });
            viewModel = new MainViewModel();
            BindingContext = viewModel;
            GenerateData();
            audio = DependencyService.Get<IAudioNoti>();
            //audio.playAudio();
        }

        private void ChangeStatus(OrderDetail mess)
        {
            viewModel.ChangeStatus(mess);
            var item = viewModel.ListComplete.FirstOrDefault(x=>x.OrderDetailId == mess.OrderDetailId);
            viewModel.ListComplete.Remove(item);
            lsComplete.Remove(item);
            if (item.Status == "XONG") return;
            item.Status = "ĐANG CHỜ";
            viewModel.ListWaiting.Insert(0, item);
            lsList.AddFirst(item);
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
            try
            {

                var e = ListNotifi.First();
                if (e.Status == "ĐANG THỰC HIỆN")
                {
                    var check = viewModel.ListWaiting.FirstOrDefault(x => x.OrderDetailId == e.OrderDetailId);
                    string mess = e.Pic.UserInfo.DisplayName + " bắt đầu làm món " + e.Dish.LabName;
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        isShowingAlert = true;
                        audio.playAudio();
                        check.Status = "ĐANG THỰC HIỆN";
                        viewModel.ChangeStatusToDoing(check);
                        lsList.Remove(check);
                        switch (check.Pic.EmployeeId)
                        {
                            case 10:
                                await lsChef1.AddLast(check);
                                break;
                            case 11:
                                await lsChef2.AddLast(check);
                                break;
                            case 12:
                                await lsChef3.AddLast(check);
                                break;
                            default:
                                break;
                        }

                        Notify.Text = mess;
                        Notify.TextColor = Color.White;
                    });
                    await Task.Delay(timeWait);
                }
                else if (e.Status == "HOÀN TẤT")
                {
                    OrderDetail check = null;
                    string mess = e.Pic.UserInfo.DisplayName + " đã hoàn thành món " + e.Dish.LabName;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        isShowingAlert = true;
                        audio.playAudio();
                        switch (e.Pic.EmployeeId)
                        {
                            case 10:
                                check = viewModel.ListCook1.FirstOrDefault(x => x.OrderDetailId == e.OrderDetailId);
                                lsChef1.Remove(check);
                                viewModel.ListCook1.Remove(check);
                                break;
                            case 11:
                                check = viewModel.ListCook2.FirstOrDefault(x => x.OrderDetailId == e.OrderDetailId);
                                lsChef2.Remove(check);
                                viewModel.ListCook2.Remove(check);
                                break;
                            case 12:
                                check = viewModel.ListCook3.FirstOrDefault(x => x.OrderDetailId == e.OrderDetailId);
                                lsChef3.Remove(check);
                                viewModel.ListCook3.Remove(check);
                                break;
                            default:
                                break;
                        }
                        if (check != null)
                        {
                            check.Status = "HOÀN TẤT";
                            viewModel.ChangeStatusToComplete(check);
                            lsComplete.AddFirst(check);
                        }
                        Notify.Text = mess;
                        Notify.TextColor = Color.LightGreen;
                    });
                    await Task.Delay(timeWait);
                }
                else if(e.Status == "ĐANG CHỜ")
                {
                    var check = viewModel.ListWaiting.FirstOrDefault(x => x.OrderDetailId == e.OrderDetailId);
                    if(check == null)
                    {
                        string mess = "Đã thêm: " + e.Quantity + " món (" + e.Dish.LabName + ") vào danh sách chờ";

                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            isShowingAlert = true;
                            audio.playAudio();
                            viewModel.ListWaiting.Add(e);
                            await lsList.AddLast(e);
                            Notify.Text = mess;
                            Notify.TextColor = Color.White;
                        });
                        await Task.Delay(timeWait);
                    }
                    else
                    {
                        string mess = "Đã cập nhật món " + e.Dish.LabName + " lên " + e.Quantity;

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            isShowingAlert = true;
                            audio.playAudio();
                            check.Quantity = e.Quantity;
                            Notify.Text = mess;
                            Notify.TextColor = Color.LightPink;
                        });
                        await Task.Delay(timeWait);
                    }
                }
                ListNotifi.Remove(e);
                Device.BeginInvokeOnMainThread(() =>
                {
                    isShowingAlert = false;
                    string original = "Đang chờ: " + viewModel.ListWaiting?.Count + " món";
                    Notify.Text = original;
                    Notify.TextColor = Color.White;
                });

                if(ListNotifi.Count != 0)
                {
                    ShowMessage();
                }
            }
            catch (Exception)
            {
            }
        }

        private async void GenerateData()
        {
            await Task.Delay(500);
            BackgroundWorker wk = new BackgroundWorker();
            wk.DoWork += async (s, e) =>
            {
                await viewModel.LoadData();
                Device.StartTimer(TimeSpan.FromMinutes(75), () =>
                {
                    viewModel.UpdateOrder();
                    return true;
                });

                string original = "Đang chờ: " + (viewModel.ListWaiting == null? 0 : viewModel.ListWaiting.Count) + " món";
                Device.BeginInvokeOnMainThread(() =>
                {
                    isShowingAlert = false;
                    Notify.Text = original;
                    Notify.TextColor = Color.White;
                });
                await Task.Delay(100);
                Connect();
            };
            wk.RunWorkerAsync();
        }

        private void PkChef_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void ItemTaped(object sender, EventArgs e)
        {
            var frame = sender as Frame;
            Device.BeginInvokeOnMainThread(async() => {
                await frame.FadeTo(0.5, 50);
                await frame.FadeTo(1, 50);
                //gridPicWaiting.IsVisible = true;
            });
            var pic = frame.BindingContext as Pic;

            var res = await viewModel.AssignChef(pic);
            if(res == false)
            {
                await DisplayAlert("Thông báo", "Không thể assign người này!", "Ok");
            }
            else
            {
                //gridPicWaiting.IsVisible = false;
            }
            var check = viewModel.ListWaiting.FirstOrDefault(x => x.OrderDetailId == viewModel.SelectedDetail.OrderDetailId);
            check.Pic = pic;
            lsList.UpdateItem(check);
            viewModel.SelectedDetail = null;
            gridSelectedChef.IsVisible = false;

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            gridSelectedChef.IsVisible = false;
        }
    }
}
