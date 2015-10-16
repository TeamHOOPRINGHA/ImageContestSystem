namespace ImageContestSystem.Data
{
    using Models;

    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;

    public class ImageContestSystemDbContext : IdentityDbContext<User>
    {
        public ImageContestSystemDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual IDbSet<Contest> Contests { get; set; }

        public virtual IDbSet<Picture> Pictures { get; set; }

        public virtual IDbSet<Vote> Votes { get; set; }

        public static ImageContestSystemDbContext Create()
        {
            return new ImageContestSystemDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // todo

            base.OnModelCreating(modelBuilder);
        }
    }
}
