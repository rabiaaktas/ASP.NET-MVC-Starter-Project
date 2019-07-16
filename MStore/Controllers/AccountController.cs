using MStore.Models;
using MStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace MStore.Controllers
{
    
    public class AccountController : Controller
    {
        private formdbEntities db = new formdbEntities();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            MigrateShoppingCart(user.username);
            var userlog = db.Users.Where(x => x.username == user.username && x.password == user.password).FirstOrDefault();
            var forRolName = db.UserRoles.Where(y => y.userID == user.userId).FirstOrDefault();
           // var role = db.Rol.Where(z => z.roleID == forRolName.roleId).FirstOrDefault();
            //ViewBag.rol = forRolName.Rol.roleName.ToString();
            if (userlog != null)
            {
                FormsAuthentication.SetAuthCookie(userlog.username, false); //username , remember me -- true/false
                //if (role.roleName == "Admin")
                //{

                //}
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ViewBag.LogError = "Kullanıcı adı ve ya şifre hatalı.";
                return View(user);
            }
            
        }

        public ActionResult Register(RegisterViewModel register)
        {
            MembershipCreateStatus membershipCreateStatus = 0;
            Membership.CreateUser(register.userName, register.password, register.email);
            if(membershipCreateStatus == MembershipCreateStatus.Success)
            {
                MigrateShoppingCart(register.userName);
                FormsAuthentication.SetAuthCookie(register.userName, false);
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ModelState.AddModelError("", membershipCreateStatus.ToString());
            }
            return View(register);

        }

        public string[] GetUserRole(string username)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return null;
            }
            var userRoles = new string[] { };
            var selected = db.Users.Where(x => x.username == username).FirstOrDefault();
            var selectUsRole = db.UserRoles.Where(x => x.userID == selected.userId).ToArray();
            for(int i = 0 ; i < selectUsRole.Length; i++)
            {
                var role = db.Rol.Where(x => x.roleID == selectUsRole[i].roleId).FirstOrDefault();
                userRoles[i] = role.roleName;
            }
            return userRoles;
        }

        public bool isInRole(string username,string role)
        {
            var userRole = GetUserRole(username);
            return userRole.Contains(role);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //public ActionResult ActivationAccount()
        //{
        //    bool accountStatus = false;

        //}
        [NonAction]
        public void ActivationMail(string email,string activationCode)
        {
            var url = string.Format("/Account/ActivationMail/{0}", activationCode);
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, url);
            var fromMail = new MailAddress("rabia@digitalpartners.com.tr","MStore Örnek"); //username,title
            var toMail = new MailAddress(email);
            SmtpClient sm = new SmtpClient();
            sm.Port = 587;
            sm.Host = "smtp.gmail.com";
            sm.EnableSsl = false;
            sm.Credentials = new NetworkCredential("rabia@digitalpartners.com.tr", "password");
            var message = new MailMessage();
            message.Subject = "Activation Mail";
            message.IsBodyHtml = true;
            message.Body = "<br/> Please click on the following link in order to activate your account" + "<br/><a href='" + link + "'> Activation Account ! </a>";
            sm.Send(message);
            sm.Dispose();
            message.Dispose();
        }
        private void MigrateShoppingCart(string userName)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.MigrateCart(userName);
            Session[ShoppingCart.cartSessionKey] = userName;

        }
    }

    
}