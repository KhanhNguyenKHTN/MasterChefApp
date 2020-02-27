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
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
            btnLogin.Clicked += BtnLogin_Clicked;
		}

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            gridWaiting.IsVisible = true;
            await Navigation.PushModalAsync(new MainPage());
            gridWaiting.IsVisible = false;
        }

        private async void BtnForgotPass_Clicked(object sender, EventArgs e)
        {
            gridWaiting.IsVisible = true;
            await Navigation.PushModalAsync(new ForgetPassword());
            gridWaiting.IsVisible = false;
        }
    }
}