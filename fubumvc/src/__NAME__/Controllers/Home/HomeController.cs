using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace __NAME__.Controllers.Home
{
    public class HomeController
    {
        public HomeViewModel Home(HomeInputModel model)
        {
            return new HomeViewModel {Text = "Hello World!"};
        }
    }

    public class HomeViewModel
    {
        public string Text { get; set; }
    }

    public class HomeInputModel
    {
        
    }
}
