using BulkyBook.DataAccess3.Data;
using BulkyBook.DataAccess3.Repositories.IRepositories;
using BulkyBook.Models3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess3.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product productDataToUpdate)
        {
            var productData = _db.Product.FirstOrDefault(u=>u.Id == productDataToUpdate.Id);

            if (productData != null)
            {
                productData.Title = productDataToUpdate.Title;
                productData.ISBN = productDataToUpdate.ISBN;
                productData.Description = productDataToUpdate.Description;
                productData.Author = productDataToUpdate.Author;
                productData.ListPrice = productDataToUpdate.ListPrice;
                productData.Price = productDataToUpdate.Price;
                productData.Price50 = productDataToUpdate.Price50;
                productData.Price100 = productDataToUpdate.Price100;
                productData.CategoryId = productDataToUpdate.CategoryId;
                if (productDataToUpdate.ImageUrl != null)
                {
                    productData.ImageUrl = productDataToUpdate.ImageUrl;
                }
            }
        }
    }
}
