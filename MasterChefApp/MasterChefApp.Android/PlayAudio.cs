using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MasterChefApp.Controls.Audio;

namespace MasterChefApp.Droid
{
    public class PlayAudio : IAudioNoti
    {
        public void playAudio()
        {
            GlobalAndroidClass.Player.Start();
        }
    }
}