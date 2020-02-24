using Model.Model;
using Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Viewmodel.ViewModel.MainView
{
    public class MainViewModel: BaseViewModel
    {
        private ObservableCollection<OrderDetail> _ListWaiting;
        public ObservableCollection<OrderDetail> ListWaiting { get => _ListWaiting; set { _ListWaiting = value; OnPropertyChanged(); } }

        private bool _IsLoadingWaiting = false;
        public bool IsLoadingWaiting { get => _IsLoadingWaiting; set { _IsLoadingWaiting = value; OnPropertyChanged(); } }

        private bool _IsLoadingChef1 = true;
        public bool IsLoadingChef1 { get => _IsLoadingChef1; set { _IsLoadingChef1 = value; OnPropertyChanged(); } }

        private bool _IsLoadingChef2 = true;
        public bool IsLoadingChef2 { get => _IsLoadingChef2; set { _IsLoadingChef2 = value; OnPropertyChanged(); } }

        private bool _IsLoadingChef3 = true;
        public bool IsLoadingChef3 { get => _IsLoadingChef3; set { _IsLoadingChef3 = value; OnPropertyChanged(); } }

        public void InsertOrderDetail(OrderDetail e)
        {
            ListWaiting.Add(e);
        }

        private bool _IsLoadingComplete = true;
        public bool IsLoadingComplete { get => _IsLoadingComplete; set { _IsLoadingComplete = value; OnPropertyChanged(); } }


        private ObservableCollection<OrderDetail> _ListCook1;
        public ObservableCollection<OrderDetail> ListCook1 { get => _ListCook1; set { _ListCook1 = value; OnPropertyChanged(); } }

        private ObservableCollection<OrderDetail> _ListCook2;
        public ObservableCollection<OrderDetail> ListCook2 { get => _ListCook2; set { _ListCook2 = value; OnPropertyChanged(); } }

        private ObservableCollection<OrderDetail> _ListCook3;
        public ObservableCollection<OrderDetail> ListCook3 { get => _ListCook3; set { _ListCook3 = value; OnPropertyChanged(); } }

        private ObservableCollection<OrderDetail> _ListComplete;
        public ObservableCollection<OrderDetail> ListComplete { get => _ListComplete; set { _ListComplete = value; OnPropertyChanged(); } }


        private ObservableCollection<Pic> _ListUser;
        public ObservableCollection<Pic> ListUser { get => _ListUser; set { _ListUser = value; OnPropertyChanged(); } }

        private ObservableCollection<string> _ListStringUser;
        public ObservableCollection<string> ListStringUser { get => _ListStringUser; set { _ListStringUser = value; OnPropertyChanged(); } }

        public OrderDetail SelectedDetail { get; set; }

        MainService service;
        public MainViewModel()
        {
            service = new MainService();
        }
        public async Task<bool> LoadData()
        {
            await LoadWaitingPage();
            LoadCompletePage();
            ListUser = new ObservableCollection<Pic>()
            {
                new Pic()
                {
                    EmployeeId = 10,
                    UserInfo = new UserInfo()
                    {
                        DisplayName = "Đầu bếp 01",
                        UserId = 15,
                        
                    },
                },
                new Pic()
                {
                    EmployeeId = 11,
                    UserInfo = new UserInfo()
                    {
                        DisplayName = "Đầu bếp 02",
                        UserId = 16
                    },
                },
                new Pic()
                {
                    EmployeeId = 12,
                    UserInfo = new UserInfo()
                    {
                        DisplayName = "Đầu bếp 03",
                        UserId = 17
                    },
                }
            };

            LoadAllListCook();
            return true;
           // LoadAllListCook();
        }
        public void ChangeStatusToComplete(OrderDetail check)
        {
            ListComplete.Insert(0, check);
        }
        public void ChangeStatusToDoing(OrderDetail check)
        {
            ListWaiting.Remove(check);
            switch (check.Pic.EmployeeId)
            {
                case 10:
                    ListCook1.Add(check);
                    break;
                case 11:
                    ListCook2.Add(check);
                    break;
                case 12:
                    ListCook3.Add(check);
                    break;
                default:
                    break;
            }
        }

        public async Task<bool> LoadWaitingPage()
        {
            IsLoadingWaiting = true;
            ListWaiting = await service.GetListOrderByStatus("ĐANG CHỜ");
            IsLoadingWaiting = false;
            return true;
        }

        public void LoadCompletePage()
        {
            IsLoadingComplete = true;
            BackgroundWorker wk = new BackgroundWorker();
            wk.DoWork += async (s, e) => {
                ListComplete = await service.GetListOrderByStatus("HOÀN TẤT");
            };
            wk.RunWorkerCompleted += (s, e) =>
            {
                IsLoadingComplete = false;
            };
            wk.RunWorkerAsync();

        }

        public async void LoadAllListCook()
        {
            IsLoadingChef1 = true;
            ListCook1 = await service.GetListPicAndStatus(ListUser[0], "ĐANG THỰC HIỆN");
            IsLoadingChef1 = false;


            IsLoadingChef2 = true;
            ListCook2 = await service.GetListPicAndStatus(ListUser[1], "ĐANG THỰC HIỆN");
            IsLoadingChef2 = false;
           

            IsLoadingChef3 = true;
            ListCook3 = await service.GetListPicAndStatus(ListUser[2], "ĐANG THỰC HIỆN");
            IsLoadingChef3 = false;

        }

        public async Task<bool> AssignChef(Pic pic)
        {
            if (SelectedDetail == null) return true;
            return await service.AssignChef(pic, SelectedDetail);
        }
    }
}
