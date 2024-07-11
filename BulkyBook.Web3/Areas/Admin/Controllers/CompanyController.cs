using BulkyBook.DataAccess3.Repositories.IRepositories;
using BulkyBook.Models3;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Web3.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> companyListData = _unitOfWork.Company.GetAll().ToList();
            return View(companyListData);
        }

        public IActionResult Upsert(int? id)
        {
            Company company = new Company();

            if (id == null || id == 0)
            {
                return View(company);
            }
            else
            {
                company = _unitOfWork.Company.Get(u => u.Id == id);
                return View(company);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company companyFormData)
        {
            if (companyFormData == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (companyFormData.Id != 0)
                {
                    _unitOfWork.Company.Update(companyFormData);
                    TempData["success"] = "Company data updated successfully";
                }
                else
                {
                    _unitOfWork.Company.Add(companyFormData);
                    TempData["success"] = "Company created successfully";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "model state not valid. check company data!";
                return RedirectToAction("Index");
            }
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> companyListData = _unitOfWork.Company.GetAll().ToList();
            if (companyListData != null)
            {
                return Json(new { success = true, data = companyListData });
            }
            else
            {
                return Json(new { success = false, message = "error getting data" });
            }

        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                Company? companyDataToDelete = _unitOfWork.Company.Get(u => u.Id == id);
                if (companyDataToDelete != null)
                {
                    _unitOfWork.Company.Remove(companyDataToDelete);
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
                return Json(new { success = false, message = "company Id not found" });
            }

        }
        #endregion
    }
}
