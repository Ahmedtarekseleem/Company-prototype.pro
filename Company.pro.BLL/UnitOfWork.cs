using Company.pro.BLL.Interfaces;
using Company.pro.BLL.Repositories;
using Company.pro.DAL.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.pro.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyDbContext _context;

        public IDepartmentRepository DepartmentRepository { get; } // Null

        public IEmployeeRepository EmployeeRepository { get; } // Null
        public UnitOfWork(CompanyDbContext context)
        {
            _context = context;
            DepartmentRepository= new DepartmentRepository(_context);
            EmployeeRepository= new EmployeeRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
