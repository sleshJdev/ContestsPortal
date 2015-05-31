using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ContestsPortal.WebSite.Infrastructure.ActionAttributes
{
    public class AjaxOnlyAttribute: ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            if (!controllerContext.HttpContext.Request.IsAjaxRequest()) return false;
            return true;
        }
    }
}