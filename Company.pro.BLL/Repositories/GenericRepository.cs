 using Company.pro.BLL.Interfaces;
using Company.pro.DAL.Data.Contexts;
using Company.pro.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.pro.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly CompanyDbContext _context;
        public GenericRepository(CompanyDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T model)
        {
            await _context.Set<T>().AddAsync(model);
        }

        public void Delete(T model)
        {
            _context.Set<T>().Remove(model);
        }

        public async Task<T?> GetAsync(int Id)
        {
            if (typeof(T) == typeof(Employee))
            {
                return await _context.Employees.Include(E => E.Department).FirstOrDefaultAsync(E => E.Id == Id) as T;
            }
            return _context.Set<T>().Find(Id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee))
            {
                return  (IEnumerable<T>)await _context.Employees.Include(E => E.Department).ToListAsync();
            }
           return await _context.Set<T>().ToListAsync();
        }

        public void Update(T model)
        {
            _context.Set<T>().Update(model);
        }
    }
}
