using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PartialCrud.Data;
using PartialCrud.Models;
using System.Linq.Dynamic;

namespace PartialCrud.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.IsStatusOnly = false;
            return View();
        }
        public ActionResult GetGridDataPartial(string status)
        {
            ViewBag.Status = status;
            return PartialView("_GetGridDataPartial");
        }
        [HttpPost]
        public ActionResult LoadData(string status)
        {
            try
            {
                using (masterDBEntities obj = new masterDBEntities())
                {
                    var draw = Request.Form.GetValues("draw").FirstOrDefault();
                    var start = Request.Form.GetValues("start").FirstOrDefault();
                    var length = Request.Form.GetValues("length").FirstOrDefault();
                    var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                    var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                    var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

                    //Paging Size (10,20,50,100)    
                    int pageSize = length != null ? Convert.ToInt32(length) : 0;
                    int skip = start != null ? Convert.ToInt32(start) : 0;
                    int recordsTotal = 0;

                    // Getting all  data   
                    var v = (from m in obj.Employee select m).Where(m => m.Status == status);

                    //Sorting    
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        v = v.OrderBy(sortColumn + " " + sortColumnDir);
                    }
                    //Search    
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        v = v.Where(a => a.Name.Contains(searchValue) || a.Email.Contains(searchValue) || a.Designation.Contains(searchValue) || a.Salary.ToString().Contains(searchValue) || a.DateOfJoining.ToString().Contains(searchValue));
                    }

                    //total number of rows count     
                    recordsTotal = v.Count();
                    //Paging     
                    var data = v.Skip(skip).Take(pageSize).ToList();
                    // Returning Json Data
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public ActionResult AddEdit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return PartialView("AddEdit");
                }
                else
                {
                    using (masterDBEntities obj = new masterDBEntities())
                    {
                        EmployeeModel emp = new EmployeeModel();

                        emp = obj.Employee.Where(x => x.EmployeeId == id).Select(x => new EmployeeModel
                        {
                            EmployeeId = x.EmployeeId,
                            Name = x.Name,
                            Email = x.Email,
                            Designation = x.Designation,
                            Salary = x.Salary,
                            DateOfJoining = x.DateOfJoining,
                            Status = x.Status
                        }).FirstOrDefault();
                        return PartialView("AddEdit", emp);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult AddEdit(EmployeeModel empm)
        {
            try
            {
                using (masterDBEntities obj = new masterDBEntities())
                {
                    Employee emp = new Employee();
                    if (empm.EmployeeId == 0)
                    {
                        emp.Name = empm.Name;
                        emp.Email = empm.Email;
                        emp.Designation = empm.Designation;
                        emp.Salary = empm.Salary;
                        emp.DateOfJoining = empm.DateOfJoining;
                        emp.Status = empm.Status;
                        obj.Employee.Add(emp);
                        obj.SaveChanges();
                        return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var empp = obj.Employee.Find(empm.EmployeeId);
                        empp.Name = empm.Name;
                        empp.Email = empm.Email;
                        empp.Designation = empm.Designation;
                        empp.Salary = empm.Salary;
                        empp.DateOfJoining = empm.DateOfJoining;
                        empp.Status = empm.Status;
                        obj.SaveChanges();
                        return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult DeleteData(int? id)
        {
            try
            {
                using (masterDBEntities obj = new masterDBEntities())
                {
                    var v = obj.Employee.Find(id);
                    if (id == null)
                        return Json(new { success = true, message = "Not Deleted..!" }, JsonRequestBehavior.AllowGet);
                    obj.Employee.Remove(v);
                    obj.SaveChanges();
                    return Json(new { success = true, message = "Successfully Delete..!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}