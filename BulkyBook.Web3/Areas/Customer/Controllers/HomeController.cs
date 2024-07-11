using BulkyBook.DataAccess3.Repositories.IRepositories;
using BulkyBook.Models3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBook.Web3.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork; 

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            return View(productList);
        }
        public IActionResult Details(int productId)
        {
            ShoppingCart shoppingCart = new()
            {
               Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category"),
               count = 1,
               ProductId = productId
            };
            return View(shoppingCart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCartData)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCartData.ApplicationId = userId;
            ShoppingCart cartFromDb = 
                _unitOfWork.ShoppingCart.Get(u => u.ApplicationId == userId && u.ProductId == shoppingCartData.ProductId);
            if(cartFromDb != null)
            {
                cartFromDb.count += shoppingCartData.count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                TempData["success"] = "product cart updated successfully";
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppingCartData);
                TempData["success"] = "product successfully added to cart";
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
