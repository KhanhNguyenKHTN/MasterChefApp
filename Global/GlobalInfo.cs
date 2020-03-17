using Model.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Global
{
    public static class GlobalInfo
    {
        public static string BaseUrl = @"http://192.168.1.199:8081/";

        public static string CustomerName = @"";

        public static Customer User { get; set; }

        public static Stream BellRing { get; set; }
    }
}
