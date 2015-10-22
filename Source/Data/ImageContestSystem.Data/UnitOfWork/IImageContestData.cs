namespace ImageContestSystem.Data.UnitOfWork
{
    using Models;
    using Repositories;

    public interface IImageContestData
    {
        IRepository<Contest> Contests { get; }

        IRepository<Picture> Pictures { get; }

        IRepository<User> Users { get; }

        IRepository<Vote> Votes { get; }

        int SaveChanges();
    }
}