using System;
using System.Web.Mvc;

namespace ContestsPortal.WebSite.Infrastructure.Binders
{
    public class TimeSpanModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            int totalMinutes;
            if (value != null && int.TryParse(value.AttemptedValue, out totalMinutes))
            {
                return TimeSpan.FromMinutes(totalMinutes);
            }
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}