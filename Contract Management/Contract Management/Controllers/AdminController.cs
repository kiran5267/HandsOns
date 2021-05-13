using Contract_Management.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Contract_Management.Controllers
{
    public class AdminController : Controller
    {
        private readonly contractsdbEntities db = new contractsdbEntities();

        [Route("")]
        public ActionResult Login()
        {
            return View();
        }

        [Route("")]
        [HttpPost]
        public ActionResult Login(string userid,string pwd)
        {
            var user = db.admins.Where(x => x.userid==userid && x.pwd==pwd).FirstOrDefault();
            if (user != null)
            {
                Session["userid"] = "admin";
                return Redirect("/dashboard");
            }
            else
            {
                var ctr = db.contractors.Where(x => x.userid == userid && x.pwd == pwd).FirstOrDefault();
                if (ctr != null)
                {
                    Session["userid"] = ctr.userid;
                    Session["uname"] = string.Concat(ctr.fname, " ", ctr.lname);
                    return Redirect("/home");
                }
                else
                {
                    TempData["error"] = "Invalid username or password";
                    return Redirect("/");
                }
            }
        }

        [Route("dashboard")]
        public ActionResult Dashbord()
        {
            ViewBag.ccount = db.contractors.Count();
            ViewBag.tcount = db.tenders.Count();
            return View();
        }

        [Route("contractors")]
        public ActionResult Contractors(int? cat)
        {
            var categories = new List<ContractorTypes>();
            int i = 1;
            db.contractors.Select(x=>x.category).Distinct().ToList().ForEach((item)=>
            {
                categories.Add(new ContractorTypes { ContractorType = item, Id = i++ });
            }
            );
            ViewBag.cats = categories;
            IEnumerable<contractor> list;
            if (cat == null)
                list = db.contractors;
            else
            {
                var category = categories.Find(x => x.Id == cat).ContractorType;
                list = db.contractors.Where(x => x.category == category);
            }
            return View(list);
        }

        public ActionResult ContractorDetails(string id)
        {
            return View(db.contractors.Find(id));
        }

        [Route("addtendor")]
        public ActionResult CreateTender()
        {
            tender tt = new tender();
            tt.tid = db.tenders.Count()>0 ? Convert.ToInt32(db.tenders.Max(x=>x.tid)) + 1 : 101;
            tt.tdate = DateTime.Today;
            tt.duedate = DateTime.Today;
            return View(tt);
        }

        [Route("addtendor")]
        [HttpPost]
        public ActionResult CreateTender(tender t)
        {
            if (ModelState.IsValid)
            {
                t.status = "Running";
                db.tenders.Add(t);
                db.SaveChanges();
                TempData["msg"] = "Tender created successfully";
                return Redirect("/tendors");
            }
            return View(t);
        }

        [Route("tendors")]
        public ActionResult Tendors(string search)
        {
            if (search == null)
            {
                return View(db.tenders);
            }
            else
            {
                return View(db.tenders.Where(x => x.title.Contains(search)));
            }
        }

        public ActionResult TendorDetails(int id)
        {
            ViewBag.appliers = db.applytenders.Where(x => x.tid == id);
            return View(db.tenders.Find(id));
        }

        [Route("logout")]
        public ActionResult Logout()
        {
            Session.Abandon();
            return Redirect("/");
        }

        [Route("reports")]
        public ActionResult Reports(string fromdate,string todate)
        {
            if (fromdate != null)
            {
                var list = db.applytenders.ToList().Where(x => Convert.ToDateTime(x.applydate).Date >= DateTime.Parse(fromdate) && Convert.ToDateTime(x.applydate).Date <= DateTime.Parse(todate));
                return View(list);
            }
            return View(db.applytenders.ToList().Where(x=>Convert.ToDateTime(x.applydate).Date==DateTime.Today));
        }

        [Route("approve/{id:int:min(0)}")]
        public ActionResult ApproveTendor(int id)
        {
            var applytender = db.applytenders.Find(id);
            var tender = db.tenders.Find(applytender.tid);
            applytender.status = "Approved";
            applytender.approvaldate = DateTime.Now;
            tender.status = "Closed";
            db.Entry(applytender).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            db.Entry(tender).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            TempData["msg"] = "Tender Approved Successfully";
            return Redirect("/tendors");
        }

        [Route("export")]
        public ActionResult Export(string fromdate, string todate)
        {
            Response.AddHeader("content-disposition", "attachment;filename=Report1.xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            var list = db.applytenders.ToList().Where(x => Convert.ToDateTime(x.applydate).Date >= DateTime.Parse(fromdate) && Convert.ToDateTime(x.applydate).Date <= DateTime.Parse(todate));
            return View(list); ;
        }
    }
}