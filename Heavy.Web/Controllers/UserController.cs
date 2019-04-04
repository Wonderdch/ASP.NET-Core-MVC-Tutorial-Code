using System.Linq;
using System.Threading.Tasks;
using Heavy.Web.Data;
using Heavy.Web.Models;
using Heavy.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Heavy.Web.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            return View(users);
        }

        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserCreateViewModel userAddViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userAddViewModel);
            }

            var user = new ApplicationUser
            {
                UserName = userAddViewModel.UserName,
                Email = userAddViewModel.Email,
                IdCard = userAddViewModel.IdCard,
                BirthDate = userAddViewModel.BirthDate
            };

            var result = await _userManager.CreateAsync(user, userAddViewModel.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(userAddViewModel);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            var claims = await _userManager.GetClaimsAsync(user);

            var userEditViewModel = new UserEditViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                IdCard = user.IdCard,
                BirthDate = user.BirthDate,
                Claims = claims.Select(c => c.Value).ToList()
            };
            return View(userEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(string id, UserEditViewModel userEditViewModel)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            user.UserName = userEditViewModel.UserName;
            user.Email = userEditViewModel.Email;
            user.IdCard = userEditViewModel.IdCard;
            user.BirthDate = userEditViewModel.BirthDate;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "更新用户信息时发生错误");
            return View(userEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, "删除用户时发生错误");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "用户找不到");
            }

            return View("Index", await _userManager.Users.ToListAsync());
        }

        public async Task<IActionResult> ManageClaims(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            var vm = new ManageClaimsViewModel
            {
                UserId = id,
                AllClaims = ClaimTypes.AllClaimTypeList
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ManageClaims(ManageClaimsViewModel vm)
        {
            var user = await _userManager.FindByIdAsync(vm.UserId);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            var claim = new IdentityUserClaim<string>
            {
                ClaimType = vm.ClaimId,
                ClaimValue = vm.ClaimId
            };

            user.Claims.Add(claim);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // 代码刷新登录状态，让 Claim 的修改立即生效
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                if (currentUser.Id == vm.UserId)
                {
                    await _signInManager.RefreshSignInAsync(currentUser);
                }

                return RedirectToAction("EditUser", new { id = vm.UserId });
            }

            ModelState.AddModelError(string.Empty, "编辑用户 Claims 时发生错误");
            return View(vm);
        }
    }
}