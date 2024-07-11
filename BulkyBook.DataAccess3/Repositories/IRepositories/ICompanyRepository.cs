using BulkyBook.Models3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess3.Repositories.IRepositories
{
    public interface ICompanyRepository: IRepository<Company>
    {
        void Update(Company companyDataToUpdate);
    }
}
