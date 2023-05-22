using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagneziaComplex.Classes
{
    internal class AppData
    {
        public static EF.Entities Context { get; } = new EF.Entities();
        public static EF.Employee myAccount { get; set; } = new EF.Employee();
        public static EF.Subscription clientNewSub { get; set; } = new EF.Subscription();
       

        public static bool isLogout = false;
    }
}
