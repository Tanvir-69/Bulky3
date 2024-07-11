using BulkyBook.DataAccess3.Data;
using BulkyBook.DataAccess3.Repositories.IRepositories;
using BulkyBook.Models3;


namespace BulkyBook.DataAccess3.Repositories
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
