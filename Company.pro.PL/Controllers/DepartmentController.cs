using AutoMapper;
using Company.pro.BLL.Interfaces;
using Company.pro.BLL.Repositories;
using Company.pro.DAL.Models;
using Company.pro.PL.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.pro.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        // Ask CLR Create Object From DepartmentRepository 
        public DepartmentController(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet] // GET : /Department/Index 
        public async Task<IActionResult> Index()
        {
            var depatment = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(depatment);
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // Server side validation
            {
                var department = _mapper.Map<Department>(model);
                await _unitOfWork.DepartmentRepository.AddAsync(department);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }

            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Details([FromRoute] int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id"); // 400

            var department = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            if (department is null) return NotFound(new { StatusCode = 404, message = $"Department With Id : {id} is not found" });

            var dto = _mapper.Map<CreateDepartmentDto>(department);

            return View(viewName, dto);
        }
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute]int? id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {

                var department = _mapper.Map<Department>(model);
                department.Id = id;

               _unitOfWork.DepartmentRepository.Update(department);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0) return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete( int? id)
        {

            //if (id is null) return BadRequest("Invalid Id"); // 400

            //var department = _departmentRepository.Get(id.Value);
            //if (department is null) return NotFound(new { StatusCode = 404, message = $"Department With Id : {id} is not found" });

            //return View(department);
            return await Details(id , "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                var department = _mapper.Map<Department>(model);
                department.Id=id;

                _unitOfWork.DepartmentRepository.Delete(department);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0) return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

    }
}
