using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using BusLib.Transaction;
using System.Data;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {        
        BORegistration BOReg = new BORegistration();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        #region Registration
        public ActionResult RegistrationMaster()
        {
            return View();
        }

        public JsonResult InsertUser(string Id, string Name, string Email, string Phone, string Address, string StateId, string CityId)
        {

            bool Result = false;
            Result = BOReg.Save_User(Convert.ToInt32(Id), Name, Email, Phone, Address, Convert.ToInt32(StateId), Convert.ToInt32(CityId));
            return Json(Result);
        }

        public JsonResult DeleteUser(int Id)
        {
            bool Result = false;
            if (Id != 0)
            {
                Result = BOReg.DeleteUser(Id);
            }
            return Json(Result);
        }


        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> rowelement;
        [HttpGet]
        public JsonResult GetUser(int Id)
        {
            DataSet DS = new DataSet();
            DataTable dt = new DataTable();
            DS = BOReg.GetUser(Id);
            if (DS != null && DS.Tables.Count > 0)
            {
                dt = DS.Tables[0];
            }

            if (dt.Rows.Count > 0) //if data is there in dt(dataTable)  
            {
                foreach (DataRow dr in dt.Rows)
                {
                    rowelement = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        rowelement.Add(col.ColumnName, dr[col]); //adding columnn  
                    }
                    rows.Add(rowelement);
                }
            }
            return Json(rows, JsonRequestBehavior.AllowGet);
        }


        List<Dictionary<string, object>> rowsstate = new List<Dictionary<string, object>>();
        Dictionary<string, object> rowelementstate;
        [HttpGet]
        public JsonResult GetState()
        {
            DataSet DS = new DataSet();
            DataTable dt = new DataTable();
            DS = BOReg.GetState();
            if (DS != null && DS.Tables.Count > 0)
            {
                dt = DS.Tables[0];
            }

            if (dt.Rows.Count > 0) //if data is there in dt(dataTable)  
            {
                foreach (DataRow dr in dt.Rows)
                {
                    rowelementstate = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        rowelementstate.Add(col.ColumnName, dr[col]); //adding columnn  
                    }
                    rowsstate.Add(rowelementstate);
                }
            }
            return Json(rowsstate, JsonRequestBehavior.AllowGet);
        }

        List<Dictionary<string, object>> rowsCity = new List<Dictionary<string, object>>();
        Dictionary<string, object> rowelementCity;
        [HttpGet]
        public JsonResult GetCityFromStateId(int StateId)
        {
            DataSet DS = new DataSet();
            DataTable dt = new DataTable();
            DS = BOReg.GetCityFromStateId(StateId);
            if (DS != null && DS.Tables.Count > 0)
            {
                dt = DS.Tables[0];
            }

            if (dt.Rows.Count > 0) //if data is there in dt(dataTable)  
            {
                foreach (DataRow dr in dt.Rows)
                {
                    rowelementCity = new Dictionary<string, object>();
                    foreach (DataColumn col in dt.Columns)
                    {
                        rowelementCity.Add(col.ColumnName, dr[col]); //adding columnn  
                    }
                    rowsCity.Add(rowelementCity);
                }
            }
            return Json(rowsCity, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}