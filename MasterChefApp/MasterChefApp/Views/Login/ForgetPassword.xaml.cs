using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MasterChefApp.Views.Login
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ForgetPassword : ContentPage
	{
		public ForgetPassword ()
		{
			InitializeComponent ();
		}

        private async void btnSend_Clicked(object sender, EventArgs e)
        {
            Global.GlobalInfo.BaseUrl = txbMailOrPhone.Text;
            await Navigation.PopModalAsync();
        }
    }
}