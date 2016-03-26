using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OrganicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrganicStore.Controllers
{
    public class RolesController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: Roles
        public ActionResult Index()
        {
            var roles = context.Roles.ToList();

            return View(roles);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                context.SaveChanges();
                TempData["ResultMessage"] = "saved";

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddtoUser(string UserName, string RoleName)
        {
            try
            {
                User currentUser = context.Users.Where(p => p.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                var roles = context.Roles.OrderBy(p => p.Name).ToList().Select(m => new SelectListItem { Value = m.Name, Text = m.Name });
                ViewBag.RoleName = roles;

                var Users = context.Users.OrderBy(n => n.UserName).ToList().Select(rr => new SelectListItem { Value = rr.UserName, Text = rr.UserName });
                ViewBag.UserName = Users;

                var UserManager = new UserManager<User>(new UserStore<User>(context));

                UserManager.AddToRole(currentUser.Id, RoleName);
                context.SaveChanges();

                ViewData["ResultMessage"] = "saved";
                return View("ManageUserRoles");
            }
            catch
            {
                ViewData["ResultMessage"] = "error";
                return View("ManageUserRoles");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                User currentUser = context.Users.Where(p => p.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                var UserManager = new UserManager<User>(new UserStore<User>(context));
                ViewBag.RolesForThisUser = UserManager.GetRoles(currentUser.Id);
                var roles = context.Roles.OrderBy(p => p.Name).ToList().Select(m => new SelectListItem { Value = m.Name, Text = m.Name });
                ViewBag.RoleName = roles;

                var Users = context.Users.OrderBy(n => n.UserName).ToList().Select(rr => new SelectListItem { Value = rr.UserName, Text = rr.UserName });
                ViewBag.UserName = Users;
            }
            return View("ManageUserRoles");
        }

        public ActionResult ManageUserRoles()
        {
            try
            {
                var roles = context.Roles.OrderBy(p => p.Name).ToList().Select(m => new SelectListItem { Value = m.Name, Text = m.Name });
                ViewBag.RoleName = roles;

                var Users = context.Users.OrderBy(n => n.UserName).ToList().Select(rr => new SelectListItem { Value = rr.UserName, Text = rr.UserName });
                ViewBag.UserName = Users;

                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(string roleName)
        {
            try
            {
                var role = context.Roles.Where(p => p.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                context.Roles.Remove(role);
                context.SaveChanges();
                TempData["ResultMessage"] = "deleted";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult DeleteUserRole()
        {
            var Users = context.Users.OrderBy(n => n.UserName).ToList().Select(rr => new SelectListItem { Value = rr.UserName, Text = rr.UserName });
            SelectListItem NewListItem = new SelectListItem { Text = "--Select--", Value = "--Select--" };
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(NewListItem);
            foreach (var user in Users)
            {
                listItems.Add(user);
            }

            ViewBag.UserName = listItems;

            return View("DeleteUserRole");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string UserRoles)
        {
            // var account = new AccountController();
            try
            {
                var Users = context.Users.OrderBy(n => n.UserName).ToList().Select(rr => new SelectListItem { Value = rr.UserName, Text = rr.UserName });
                SelectListItem NewListItem = new SelectListItem { Text = "--Select--", Value = "--Select--" };
                List<SelectListItem> listItems = new List<SelectListItem>();
                listItems.Add(NewListItem);
                foreach (var user1 in Users)
                {
                    listItems.Add(user1);
                }

                //List<SelectListItem> roleNameList = new List<SelectListItem>();
                //ViewBag.RoleName = roleNameList;
                ViewBag.UserName = listItems;
                var UserManager = new UserManager<User>(new UserStore<User>(context));
                User user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (user != null && string.IsNullOrEmpty(UserRoles) == false)
                {
                    if (UserManager.IsInRole(user.Id, UserRoles))
                    {
                        UserManager.RemoveFromRole(user.Id, UserRoles);
                        TempData["ResultMessage"] = "deleted";
                    }
                    else
                    {
                        TempData["ResultMessage"] = "error";
                    }
                    // prepopulat roles for the view dropdown
                    var list = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                    ViewBag.Roles = list;
                }
                else
                {
                    throw new Exception();
                }

                return RedirectToAction("DeleteUserRole");
            }
            catch
            {
                TempData["ResultMessage"] = "error";
                return RedirectToAction("DeleteUserRole");
            }
        }

        public ActionResult GetUserRoles(string userName)
        {
            if (userName != "--Select--")
            {
                User currnetuser = context.Users.Where(p => p.UserName.Equals(userName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                UserManager<User> userManager = new UserManager<Models.User>(new UserStore<User>(context));
                List<string> roles = userManager.GetRoles(currnetuser.Id).ToList();
                List<SelectListItem> listItems = new List<SelectListItem>();

                foreach (var role in roles)
                {
                    SelectListItem item = new SelectListItem() { Text = role, Value = role };
                    listItems.Add(item);
                }

                // ViewBag.UserAppliedRoles = listItems;
                return Json(listItems);
            }
            return Json(string.Empty);
        }

        public ActionResult Edit(string roleName)
        {
            var thisRole = context.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            return View(thisRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IdentityRole role)
        {
            try
            {
                context.Entry(role).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}