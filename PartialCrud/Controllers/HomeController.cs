using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PartialCrud.Data;
using PartialCrud.Models;
using System.Linq.Dynamic;
using PartialCrud.Service;

namespace PartialCrud.Controllers
{
    public class HomeController : Controller
    {

        public IEmployee ES = new EmployeeService();

        //public HomeController (EmployeeService ES)
        //{
        //    this.ES = ES;
        //}

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
        public ActionResult AddEdit(Int64? id)
        {
            if (id == null)
            {
                return PartialView("AddEdit");
            }
            else
            {
                return PartialView("AddEdit", ES.AddEdit(id));
            }
        }
        [HttpPost]
        public ActionResult AddEdit(EmployeeModel empm)
        {
            string msg = null;
            if (ES.Save(empm, out msg))
            {
                return Json(new { success = true, msg }, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteData(int? id)
        {
            bool data = ES.Delete(id);
            if (data)
                return Json(new { success = true, message = "Successfully Delete..!" }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = true, message = "Not Deleted..!" }, JsonRequestBehavior.AllowGet);
        }
    }
}