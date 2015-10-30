using System.Web.Mvc;

namespace ImageContestSystem.Web.Areas.Administration
{
    public class AdministrationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Administration";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Administration_default",
                "Administration/{controller}/{action}/{id}",
                new { Controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "ImageContestSystem.Web.Areas.Administration.Controllers" }
            );
        }
    }
}