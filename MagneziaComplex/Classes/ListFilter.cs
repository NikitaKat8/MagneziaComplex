using MagneziaComplex.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MagneziaComplex.Classes
{
    internal class ListFilter
    {
        public List<Employee> myEmployeeFilter(int clubIndex, int roleIndex, ListView listView)
        {
            if(clubIndex == 0 && roleIndex == 0)
            {
                return AppData.Context.Employee.ToList();
            }
    
            if(clubIndex == 0)
            {
                var res1 = AppData.Context.Employee.Where(x => x.idRole == roleIndex).ToList();
                return res1;
            }
            if(roleIndex == 0)
            {
                var res2 = AppData.Context.Employee.Where(x => x.idClub == clubIndex ).ToList();
                return res2;
            }

            var finalList = AppData.Context.Employee.Where(x=> x.idRole== roleIndex && x.idClub == clubIndex).ToList();
            return finalList;
        }
    }
}
