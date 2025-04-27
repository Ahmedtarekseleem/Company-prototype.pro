using Company.pro.BLL.Interfaces;
using Company.pro.DAL.Data.Contexts;
using Company.pro.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.pro.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {

        public DepartmentRepository(CompanyDbContext context) : base(context)
        {

        }

        
    }
}
