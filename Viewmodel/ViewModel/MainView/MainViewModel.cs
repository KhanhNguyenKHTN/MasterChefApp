using Model.Model;
using Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Viewmodel.ViewModel.MainView
{
    public class MainViewModel: BaseViewModel
    {
        private ObservableCollection<OrderDetail> _ListWaiting;
        public ObservableCollection<OrderDetail> ListWaiting { get => _ListWaiting; set { _ListWaiting = value; OnPropertyChanged(); } }

        private bool _IsLoadingWaiting = true;
        public bool IsLoadingWaiting { get => _IsLoadingWaiting; set { _IsLoadingWaiting = value; OnPropertyChanged(); } }

        private bool _IsLoadingChef1 = true;
        public bool IsLoadingChef1 { get => _IsLoadingChef1; set { _IsLoadingChef1 = value; OnPropertyChanged(); } }

        private bool _IsLoadingChef2 = true;
        public bool IsLoadingChef2 { get => _IsLoadingChef2; set { _IsLoadingChef2 = value; OnPropertyChanged(); } }

        private bool _IsLoadingChef3 = true;
        public bool IsLoadingChef3 { get => _IsLoadingChef3; set { _IsLoadingChef3 = value; OnPropertyChanged(); } }

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

        MainService service;
        public MainViewModel()
        {
            service = new MainService();
        }
        public void LoadData()
        {
            LoadWaitingPage();
            LoadCompletePage();
            ListUser = new ObservableCollection<Pic>()
            {
                new Pic()
                {
                    EmployeeId = 10,
                    userInfo = new UserInfo()
                    {
                        DisplayName = "Đầu bếp 01"
                    },
                },
                new Pic()
                {
                    EmployeeId = 11,
                    userInfo = new UserInfo()
                    {
                        DisplayName = "Đầu bếp 02"
                    },
                },
                new Pic()
                {
                    EmployeeId = 12,
                    userInfo = new UserInfo()
                    {
                        DisplayName = "Đầu bếp 03"
                    },
                }
            };
           // LoadAllListCook();
        }

        public void LoadWaitingPage()
        {
            IsLoadingWaiting = true;
            BackgroundWorker wk = new BackgroundWorker();
            wk.DoWork +=async (s,e)=> {
                ListWaiting = await service.GetListOrderByStatus("ĐANG CHỜ");
            };
            wk.RunWorkerCompleted += (s, e) =>
            {
                IsLoadingWaiting = false;
            };
            wk.RunWorkerAsync();
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

        public void LoadAllListCook()
        {
            IsLoadingChef1 = true;
            BackgroundWorker wk1 = new BackgroundWorker();
            wk1.DoWork += async (s, e) => {
                ListCook1 = await service.GetListPicAndStatus(ListUser[0], "ĐANG THỰC HIỆN");
            };
            wk1.RunWorkerCompleted += (s, e) =>
            {
                IsLoadingChef1 = false;
            };
            wk1.RunWorkerAsync();

            IsLoadingChef2 = true;
            BackgroundWorker wk2 = new BackgroundWorker();
            wk2.DoWork += async (s, e) => {
                ListCook2 = await service.GetListPicAndStatus(ListUser[1], "ĐANG THỰC HIỆN");
            };
            wk2.RunWorkerCompleted += (s, e) =>
            {
                IsLoadingChef2 = false;
            };
            wk2.RunWorkerAsync();

            IsLoadingChef3 = true;
            BackgroundWorker wk3 = new BackgroundWorker();
            wk3.DoWork += async (s, e) => {
                ListCook3 = await service.GetListPicAndStatus(ListUser[2], "ĐANG THỰC HIỆN");
            };
            wk3.RunWorkerCompleted += (s, e) =>
            {
                IsLoadingChef3 = false;
            wk3.RunWorkerAsync();
            };
        }

        public void AutoAdd(Food n)
        {
            //ListFoods.Add(n);
        }
    }
}
