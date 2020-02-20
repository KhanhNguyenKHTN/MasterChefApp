using Model.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Global
{
    public static class GlobalInfo
    {
        public static string BaseUrl = @"http://10.248.60.150:8081/";

        public static string CustomerName = @"";

        public static Customer User { get; set; }

        public static Stream BellRing { get; set; }
    }
}
