using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PartialCrud.Data;
using PartialCrud.Models;

namespace PartialCrud.Service
{
    public class EmployeeService : IEmployee
    {
        public EmployeeModel AddEdit(Int64? id)
        {
            try
            {
                using (masterDBEntities obj = new masterDBEntities())
                {
                    return obj.Employee.Where(x => x.EmployeeId == id).Select(x => new EmployeeModel
                    {
                        EmployeeId = x.EmployeeId,
                        Name = x.Name,
                        Email = x.Email,
                        Designation = x.Designation,
                        Salary = x.Salary,
                        DateOfJoining = x.DateOfJoining,
                        Status = x.Status
                    }).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public bool Save(EmployeeModel em, out string msg)
        {
            try
            {
                using (masterDBEntities obj = new masterDBEntities())
                {
                    var temp = obj.Employee.Find(em.EmployeeId);
                    Employee emp = temp == null ? new Employee() : temp;

                    emp.Name = em.Name;
                    emp.Email = em.Email;
                    emp.Designation = em.Designation;
                    emp.Salary = em.Salary;
                    emp.DateOfJoining = em.DateOfJoining;
                    emp.Status = em.Status;

                    if (temp == null)
                    {
                        obj.Employee.Add(emp);
                        msg = "Saved Successfully";
                    }
                    else
                    {
                        msg = "Updated Successfully";
                    }
                    obj.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool Delete(Int64? id)
        {
            try
            {
                using (masterDBEntities obj = new masterDBEntities())
                {
                    var v = obj.Employee.Find(id);
                    if (v == null)
                        return false;
                    obj.Employee.Remove(v);
                    obj.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //public List<EmployeeModel> GetData()
        //{
        //    using (masterDBEntities obj = new masterDBEntities())
        //    {
        //       return (from m in obj.Employee
        //                              join cn in obj.Country on m.CountryId equals cn.CountryId
        //                              join st in obj.State on m.StateId equals st.StateId
        //                              join ct in obj.City on m.CityId equals ct.CityId
        //                              select new
        //                              {
        //                                  m.Name,
        //                                  m.Email,
        //                                  m.Designation,
        //                                  m.Salary,
        //                                  m.DateOfJoining,
        //                                  m.Status,
        //                                  cn.CountryName,
        //                                  st.StateName,
        //                                  ct.CityName
        //                              }).ToList();
        //    }
        //}
    }
}