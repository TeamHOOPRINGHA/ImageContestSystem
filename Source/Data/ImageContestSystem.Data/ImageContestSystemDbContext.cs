namespace ImageContestSystem.Data
{
    using Models;

    using Microsoft.AspNet.Identity.EntityFramework;

    public class ImageContestSystemDbContext : IdentityDbContext<User>
    {
        public ImageContestSystemDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ImageContestSystemDbContext Create()
        {
            return new ImageContestSystemDbContext();
        }
    }
}
