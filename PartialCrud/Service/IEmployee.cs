using PartialCrud.Data;
using PartialCrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartialCrud.Service
{
    public interface IEmployee
    {
        EmployeeModel AddEdit(Int64? id);

        bool Save(EmployeeModel em,out string msg);

        bool Delete(Int64? id);

        //List<EmployeeModel> GetData();
    }
}
