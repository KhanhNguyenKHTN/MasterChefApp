using Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MasterChefApp.Controls.List
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HorizontalListItem : ContentView
	{
		public HorizontalListItem ()
		{
			InitializeComponent ();
            frContent.BorderColor = BorderItemColor;
		}

        private Color _BorderItemColor;

        public Color BorderItemColor
        {
            get { return _BorderItemColor; }
            set { _BorderItemColor = value; frContent.BorderColor = _BorderItemColor; }
        }

        private void ItemTaped(object sender, EventArgs e)
        {
            MessagingCenter.Send<HorizontalListItem>(this, "AddChef");
            //TapedEvent?.Invoke(sender, e);
        }

        private void DescriptionTaped(object sender, EventArgs e)
        {
            var item = sender as StackLayout;
            var context = item.BindingContext as OrderDetail;
            if (!context.IsHasDescription) return;
            MessagingCenter.Send<HorizontalListItem, string>(this, "DescriptionTap",context.Description);
        }
    }
}