using BulkyBook.Models3;

namespace BulkyBook.DataAccess3.Repositories.IRepositories
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        void Update(ShoppingCart shoppingCartData);
    }
}
