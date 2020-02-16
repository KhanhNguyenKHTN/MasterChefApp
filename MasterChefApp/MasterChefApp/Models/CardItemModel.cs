using System;
using System.Collections.Generic;
using System.Text;

namespace MasterChefApp.Models
{
    public class CardItemModel : TabMenuItemModel
    {
        public string Description { get; set; }

        public int Likes { get; set; }

        public string ImagesSource { get; set; }

        public string Cost { get { return RealCost.ToString("C0").Remove(RealCost.ToString("C0").Length - 2, 2) + " " + Unit; } set { } } // temp for food menu

        private int _RealCost;

        public int RealCost
        {
            get { return _RealCost; }
            set { _RealCost = value; OnPropertyChanged(); }
        }

        private string _Unit;

        public string Unit
        {
            get { return _Unit; }
            set { _Unit = value; OnPropertyChanged(); }
        }
        private int _OrderQuantity = 1;

        public int OrderQuantity
        {
            get { return _OrderQuantity; }
            set { _OrderQuantity = value; OnPropertyChanged(); }
        }

        private int _Quantity;

        public int Quantity
        {
            get { return _Quantity; }
            set { _Quantity = value; OnPropertyChanged(); }
        }

        public int TotalQuantity { get { return OrderQuantity * RealCost; } }

        private bool _IsSoldOut;
        public bool IsSoldOut { get => _IsSoldOut; set { _IsSoldOut = value; OnPropertyChanged(); } }

    }

}
