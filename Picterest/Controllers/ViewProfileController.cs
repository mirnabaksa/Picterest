using System;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
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



        public ViewProfileController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IGalleryRepo repository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repository = repository;
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

            Album album = new Album
            {
                Name = model.Name,
                Description = model.Description,
                ownerId = user.Id
            };

            foreach (HttpPostedFileBase file in model.Uploads)
            {
                if (file != null)
                {
                    //TODO check extensions
                    string pic = Path.GetFileName(file.FileName);
                    string path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/images"), pic);
                    // file is uploaded
                    file.SaveAs(path);

                    /*
                    // save the image path path to the database or you can send image 
                    // directly to database
                    // in-case if you want to store byte[] ie. for DB
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                    */
                    Image im = new Image()
                        {
                            ImageId = Guid.NewGuid(),
                            OwnerId = user.Id,
                            FilePath = path

                        };
                    im.Albums.Add(album);
                    album.Images.Add(im);
                    
                }


            }

            _repository.AddAlbum(album);
            return View("Index");
        }
    }

}

