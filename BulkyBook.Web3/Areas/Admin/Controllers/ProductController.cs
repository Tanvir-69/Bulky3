using BulkyBook.DataAccess3.Repositories.IRepositories;
using BulkyBook.Models3;
using BulkyBook.Utilities3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.Web3.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        internal readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> productListData = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            if(productListData == null)
            {
                return NotFound();
            }
            return View(productListData);
        }

        public IActionResult Upsert(int? id)
        {
            ProductViewModel productViewModel = new()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(u=> new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            if(id == null || id == 0)
            {
                return View(productViewModel);
            }
            else
            {
                productViewModel.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productViewModel);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductViewModel productVMData, IFormFile? file)
        {
            if(productVMData == null)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");
                    if(!string.IsNullOrEmpty(productVMData.Product.ImageUrl)) 
                    {
                        var oldImagePath = 
                            Path.Combine(wwwRootPath, productVMData.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using(var fileStream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVMData.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if(productVMData.Product.Id != 0)
                {
                    _unitOfWork.Product.Update(productVMData.Product);
                    TempData["success"] = "product successfully updated";
                }
                else
                {
                    _unitOfWork.Product.Add(productVMData.Product);
                    TempData["success"] = "product successfully created";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "product data not valid";
                return RedirectToAction("Index");
            }
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> productListData = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            if(productListData != null)
            {
                return Json(new { data = productListData });
            }
            else
            {
                return Json(new { success = false, message = "Failed to get product data" });
            }
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return Json(new { success = false, message = "product id not found" });
            }
            else
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                Product? productDataToDelete = _unitOfWork.Product.Get(u => u.Id == id);
                if(productDataToDelete != null)
                {
                    if (!string.IsNullOrEmpty(productDataToDelete.ImageUrl))
                    {
                        var oldImagePath =
                            Path.Combine(wwwRootPath, productDataToDelete.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    _unitOfWork.Product.Remove(productDataToDelete);
                    _unitOfWork.Save();
                    return Json(new { success = true, message = "Product deleted successfully" });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to delete product" });
                }
            }
        }
        #endregion
    }
}
