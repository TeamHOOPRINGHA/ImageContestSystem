namespace ImageContestSystem.Web.Controllers
{
    using Data;
    using Data.UnitOfWork;
    using System.Web.Mvc;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;

    public class BaseController : Controller
    {
        protected static CloudBlobContainer imagesContainer;

        public BaseController()
            :this(new ImageContestData(new ImageContestSystemDbContext()))
        {          
        }

        public BaseController(IImageContestData data)
        {
            this.Data = data;
            InitStorage();
        }

        protected IImageContestData Data { get; set; }

        private static void InitStorage()
        {
            var credentials = new StorageCredentials(resources.AccountName, resources.AccountKey);
            var storageAccount = new CloudStorageAccount(credentials, true);
            var blobClient = storageAccount.CreateCloudBlobClient();
            imagesContainer = blobClient.GetContainerReference("images");
        }
    }
}