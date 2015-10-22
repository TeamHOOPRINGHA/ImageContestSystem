namespace ImageContestSystem.Web.Controllers
{
    using Data;
    using Data.UnitOfWork;
    using System.Web.Mvc;

    public class BaseController : Controller
    {
        public BaseController()
            :this(new ImageContestData(new ImageContestSystemDbContext()))
        {          
        }

        public BaseController(IImageContestData data)
        {
            this.Data = data;
        }

        protected IImageContestData Data { get; set; }
    }
}