using Company.pro.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.pro.BLL.Interfaces
{
    public interface IEmployeeRepository :IGenericRepository<Employee>
    {
        //IEnumerable<Employee> GetAll(); 
        //Employee? Get(int Id);
        //int Add(Employee model);
        //int Update(Employee model);
        //int Delete(Employee model);
        Task<List<Employee>> GetByNameAsync(string name);
    }
}
