using AutoMapper;
using Company.pro.BLL;
using Company.pro.BLL.Interfaces;
using Company.pro.DAL.Models;
using Company.pro.PL.Dtos;
using Company.pro.PL.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.pro.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        
        private readonly IMapper _mapper;

        public EmployeeController(
           IUnitOfWork unitOfWork,
            IMapper mapper
            ) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }else
            {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
            }
                return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            //var departments = _departmentRepository.GetAll();
            //ViewData["departments"] = departments;
            return View(); 
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image is not null)
                {
                    model.ImageName =  DocumentSettings.UploadFile(model.Image, "Images");
                }
                
                var employee = _mapper.Map<Employee>(model);
                await _unitOfWork.EmployeeRepository.AddAsync(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id ,string viewName= "Details")
        {
            if (id is null) return BadRequest("Invalid Id"); // 400
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (employee == null) return NotFound(new { statusCode = 404, message = $"Employee With Id : {id} is not Found "});
            var dto = _mapper.Map<CreateEmployeeDto>(employee);
            
            return View(viewName,dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //var departments = _departmentRepository.Get(id.Value);
            //ViewData["departments"] = departments;
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageName is not null && model.Image is not null )
                {
                    DocumentSettings.DeleteFile(model.ImageName, "Images");
                } 
                if (model.ImageName is not null)
                {
                   model.ImageName = DocumentSettings.UploadFile(model.Image, "Images");
                }
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                _unitOfWork.EmployeeRepository.Update(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));

                }
            }
                return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;   
               _unitOfWork.EmployeeRepository.Delete(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    if (model.ImageName is not null)
                    {
                        DocumentSettings.DeleteFile(model.ImageName, "Images");
                    }
                    return RedirectToAction(nameof(Index));

                }
            }
            return View(model);
        }

        public async Task<IActionResult> Search(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
            }

            return PartialView("EmployeePartialView/EmployeeTablePartialView", employees);

        }

    }
}
