using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Picterest.Models;

namespace Picterest.Controllers
{
    public class ViewProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IGalleryRepo _repository;
        private IHostingEnvironment _hostingEnvironment;


        public ViewProfileController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IGalleryRepo repository, IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new ProfileViewModel
            {
                Username = user.UserName,
                Albums = await _repository.GetUserAlbums(user.Id)
            };


            return View(model);
        }

        public IActionResult AddAlbum()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Album(AddAlbumModel model)
        {
            var user = await _userManager.GetUserAsync(User);

            Album album = new Album()
            {
                AlbumId = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                ownerId = user.Id
            };

            if (ModelState.IsValid)
            {

                foreach (IFormFile file in model.Photos)
                {
                    Image imageEntity = new Image()
                    {
                        ImageId = Guid.NewGuid(),
                        OwnerId = user.Id
                    };
                    
                    var parsedContentDisposition =
                        ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                    var filename = Path.Combine(_hostingEnvironment.WebRootPath,
                        "Uploads", parsedContentDisposition.FileName.Trim('"'));
                    using (var stream = System.IO.File.OpenWrite(filename))
                    {
                        await file.CopyToAsync(stream);
                    }

                    imageEntity.Path = file.FileName;
                    album.Images.Add(imageEntity);
                    imageEntity.Albums.Add(album);
                }
               
            }

          
            _repository.AddAlbum(album);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> ViewAlbumImages(Guid albumId)
        {
            var user = await _userManager.GetUserAsync(User);
            List<Image> images = await _repository.GetAlbumImages(albumId, user.Id);
            ViewAlbumImagesModel model = new ViewAlbumImagesModel()
            {
                Images = images,
                AlbumId = albumId
            };
            return View("Album",model);

        }

        [HttpGet]
        public ActionResult RenderImage(Guid id)
        {
            Image image = _repository.GetImage(id);
           
            var dir = System.Web.HttpContext.Current.Server.MapPath("/Uploads");
            var path = Path.Combine(dir, image.Path + ".jpg"); //validate the path for security or use other means to generate the path.
            return base.File(path, "image/jpeg");

        }

        public void ViewSingle()
        {
            
        }

        public Task<IActionResult> RemovePhotoFromAlbum(Guid imageId, Guid albumId)
        {
            _repository.RemovePhotoFromAlbum(imageId, albumId);
            return ViewAlbumImages(albumId);
        }

        
    }

}

