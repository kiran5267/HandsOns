using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Contract_Management.Controllers
{
    public class ContractorController : Controller
    {
        private readonly contractsdbEntities db=new contractsdbEntities();

        [Route("register")]
        public ActionResult Register()
        {
            ViewBag.questions = new SelectList(db.securityques,"id","qdesc");
            contractor c = new contractor();
            c.userid = "CTR" + (db.contractors.Count()+1).ToString("D4");
            return View(c);
        }

        [Route("register")]
        [HttpPost]
        public ActionResult Register(contractor c)
        {
            if (ModelState.IsValid)
            {
                db.contractors.Add(c);
                db.SaveChanges();
                TempData["msg"] = "New User Created Successfully";
                return Redirect("/");
            }
            ViewBag.questions = new SelectList(db.securityques, "id", "qdesc");
            return View(c);
        }

        [Route("home")]
        public ActionResult HomePage()
        {
            string userid = Session["userid"].ToString();
            var ctr = db.contractors.Find(userid);
            return View(ctr);
        }

        public ActionResult TendorDetails(int id)
        {
            return View(db.tenders.Find(id));
        }

        [HttpPost]
        [Route("applytendor")]
        public ActionResult ApplyTendor(applytender apt,HttpPostedFileBase docs)
        {
            apt.applydate = DateTime.Now;
            apt.status = "Applied";
            apt.document = "/TenderDocs/" + docs.FileName;
            apt.cid = Session["userid"].ToString();
            db.applytenders.Add(apt);
            db.SaveChanges();
            docs.SaveAs(Server.MapPath("/TenderDocs/" + docs.FileName));
            TempData["msg"] = "Applied successfully";
            return Redirect("/mytendors");
        }

        [Route("mytendors")]
        public ActionResult MyTendors()
        {
            var userid = Session["userid"].ToString().Trim();
            var list = db.applytenders.Where(x=>x.cid==userid);
            return View(list);
        }

        [Route("forgotuserid")]
        public ActionResult ForgotUserID()
        {
            return View();
        }

        [Route("forgotuserid")]
        [HttpPost]
        public ActionResult ForgotUserID(string phone,string email)
        {
            var ctr = db.contractors.Where(x => x.phone == phone && x.email == email).FirstOrDefault();
            if (ctr == null)
                TempData["error"] = "Incorrect email or phone";
            else
                TempData["userid"] = ctr.userid;
            return Redirect("/forgotuserid");
        }


        [Route("forgotpwd")]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [Route("secques")]
        public ActionResult SecurityQuestion(string userid)
        {
            return View(db.contractors.Find(userid));
        }

        [Route("secques")]
        [HttpPost]
        public ActionResult SecurityQuestion(string userid,string ans1,string ans2,string ans3)
        {
            var ctr = db.contractors.Where(x => x.userid == userid && x.ans1 == ans1 && x.ans2 == ans2 && x.ans3 == ans3).FirstOrDefault();
            if (ctr != null)
            {
                TempData["userid"] = userid;
                return Redirect("/resetpwd");
            }
            else
            {
                TempData["error"] = "Incorrect details provided";
                return Redirect("/forgotpwd");
            }
            return View();
        }

        [Route("resetpwd")]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [Route("resetpwd")]
        [HttpPost]
        public ActionResult ResetPassword(string userid,string pwd)
        {
            var ctr = db.contractors.Find(userid);
            ctr.pwd = pwd;
            db.Entry(ctr).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            TempData["msg"] = "Password reset successfully";
            return Redirect("/");
        }

        [Route("forgotpwd")]
        [HttpPost]
        public ActionResult ForgotPassword(string userid,string email,string phone)
        {
            var ctr = db.contractors.Where(x => x.phone == phone && x.email == email && x.userid==userid).FirstOrDefault();
            if (ctr == null)
            {
                TempData["error"] = "Incorrect email or phone";
                return Redirect("/forgotpwd");
            }
            else
            {
                TempData["userid"] = ctr.userid;
                return Redirect("/secques?userid="+ctr.userid);
            }
            
        }
    }
}