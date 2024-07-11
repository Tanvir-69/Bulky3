using BulkyBook.DataAccess3.Repositories.IRepositories;
using BulkyBook.Models3;
using BulkyBook.Utilities3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Web3.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> categoryListData = _unitOfWork.Category.GetAll().ToList();
            if(categoryListData == null)
            {
                return NotFound();
            }
            return View(categoryListData);
        }
        public IActionResult Upsert(int? id)
        {
            Category category = new Category();

            if (id == null || id == 0)
            {
                return View(category);
            }
            else
            {
                category = _unitOfWork.Category.Get(u => u.Id == id);
                return View(category);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Category categoryFormData)
        {
            if(categoryFormData == null)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                if(categoryFormData.Id != 0)
                {
                    _unitOfWork.Category.Update(categoryFormData);
                    TempData["success"] = "Category updated successfully";
                }
                else
                {
                    _unitOfWork.Category.Add(categoryFormData);
                    TempData["success"] = "Category created successfully";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "model state not valid. check category data!";
                return RedirectToAction("Index");
            }
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Category> categoryListData = _unitOfWork.Category.GetAll().ToList();
            if(categoryListData != null)
            {
                return Json(new { success = true, data = categoryListData });
            }
            else
            {
                return Json(new { success = false, message = "error getting data" });
            }

        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if(id != null){
                Category? categoryDataToDelete = _unitOfWork.Category.Get(u=>u.Id == id);
                if(categoryDataToDelete != null)
                {
                    _unitOfWork.Category.Remove(categoryDataToDelete);
                    _unitOfWork.Save();
                    return Json(new { success = true, message = "Item deleted successfully" });
                }
                else
                {
                    return Json(new { success = false, message = "Unsuccessfully attempt to delete" });
                }
            }
            else
            {
                return Json(new { success = false, message = "category Id not found" });
            }

        }
        #endregion
    }
}
