using Company.pro.DAL.Models;
using Company.pro.PL.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Company.pro.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<RoleToReturnDto> roles;
            if (string.IsNullOrEmpty(SearchInput))
            {
                roles = _roleManager.Roles.Select(U => new RoleToReturnDto()
                {
                    Id = U.Id,
                    Name = U.Name
                });
            }
            else
            {
                roles = _roleManager.Roles.Select(U => new RoleToReturnDto()
                {
                    Id = U.Id,
                    Name = U.Name,
                   
                }).Where(U => U.Name.ToLower().Contains(SearchInput.ToLower()));
            }
            return View(roles);
        }
        [HttpGet]
        public IActionResult Create()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByNameAsync(model.Name);
                if (role is null)
                {
                    role = new IdentityRole()
                    {
                        Name = model.Name,
                    };
                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }

            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id"); // 400
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound(new { statusCode = 404, message = $"User With Id : {id} is not Found " });
            var dto = new RoleToReturnDto()
            {
                Id= role.Id,
                Name = role.Name
            };
            return View(viewName, dto);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id)
                {
                    return BadRequest("Invalid Operation !!");
                }
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null)
                {
                    return BadRequest("Invalid Operation !!");
                }
              
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                
                ModelState.AddModelError("", "Invalid Operation !");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id)
                {
                    return BadRequest("Invalid Operation !!");
                }
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null)
                {
                    return BadRequest("Invalid Operation !!");
                }
                

                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUser(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
            {
                return NotFound();
            }
            ViewData["RoleId"]= roleId;
            var usersInRole = new List<UsersInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var userInRole = new UsersInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName =user.UserName,
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected= false; 
                }
                usersInRole.Add(userInRole);

            }
            return View(usersInRole);

        }
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUser(string roleId, List<UsersInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if (appUser is not null)
                    {
                        if (user.IsSelected && ! await _userManager.IsInRoleAsync(appUser,role.Name ))
                        {
                            await _userManager.AddToRoleAsync(appUser,role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser,role.Name);

                        } 
                    }

                }
                return RedirectToAction(nameof(Edit),new {id = roleId});
            }
            return View();
        }
    }
}
