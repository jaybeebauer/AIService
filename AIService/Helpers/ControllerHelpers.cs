using AIService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AIService.Helpers
{
    public static class ControllerHelpers
    {
        public static void AddRuleViolations(this ModelStateDictionary modelState, IEnumerable<RuleViolation> errors)
        {
            foreach (RuleViolation issue in errors)
            {
                modelState.AddModelError(issue.PropertyName, issue.ErrorMessage);
            }
        }

        public static void CreateDropDownListViewBag(this ControllerBase controller)
        {
            using (var db = new AIServiceDataContext())
            {
                controller.ViewBag.Categories = db.Categories.Select(c => new SelectListItem
                {
                    Value = c.category_id.ToString(),
                    Text = c.category_description
                }).ToList();

                controller.ViewBag.Statuses = db.Status.Select(s => new SelectListItem
                {
                    Value = s.status_id.ToString(),
                    Text = s.status_description
                }).ToList();

                controller.ViewBag.Users = db.Users.Select(u => new SelectListItem
                {
                    Value = u.user_id.ToString(),
                    Text = u.user_firstname + " " + u.user_lastname
                }).ToList();

                controller.ViewBag.Technicians = db.Users.Where(t => t.user_istechnician == true).Select(u => new SelectListItem
                {
                    Value = u.user_id.ToString(),
                    Text = u.user_firstname + " " + u.user_lastname
                }).ToList();

                controller.ViewBag.Assets = db.Assets.Select(s => new SelectListItem
                {
                    Value = s.asset_id.ToString(),
                    Text = s.asset_name
                }).ToList();
            }
        }
    }
}