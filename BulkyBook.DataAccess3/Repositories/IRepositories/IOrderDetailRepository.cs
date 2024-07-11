using BulkyBook.Models3;

namespace BulkyBook.DataAccess3.Repositories.IRepositories
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        void Update(OrderDetail orderDetailData);
    }
}
