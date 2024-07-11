using BulkyBook.DataAccess3.Data;
using BulkyBook.DataAccess3.Repositories.IRepositories;
using BulkyBook.Models3;


namespace BulkyBook.DataAccess3.Repositories
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(OrderDetail orderDetailData)
        {
            _db.Update(orderDetailData);
        }
    }
}
