using BulkyBook.DataAccess3.Data;
using BulkyBook.DataAccess3.Repositories.IRepositories;
using BulkyBook.Models3;


namespace BulkyBook.DataAccess3.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Category categoryData)
        {
            _db.Update(categoryData);
        }
    }
}
