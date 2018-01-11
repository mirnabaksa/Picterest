using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Picterest.Data;
using Picterest.Models;

namespace Picterest.Controllers
{
    public class HomeController : Controller
    {
        private IGalleryRepo _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IGalleryRepo repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> FilterAlbums(string filter)
        {
            List<Album> albums = await _repository.FilterAlbums(filter);
            return View("index",albums);
        }

        public async Task<IActionResult> FilterFavoriteAlbums(string filter)
        {
            var user = await _userManager.GetUserAsync(User);
            List<Album> albums =  user.FavoriteAlbums;
            return View("ViewFavorites", albums);
        }

        public async Task<IActionResult> ViewFavorites()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user.FavoriteAlbums);
        }
    }
}
