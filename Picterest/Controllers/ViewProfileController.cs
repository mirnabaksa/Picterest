using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Picterest.Data;
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

        [Authorize(Roles = "User")]
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
                List<Image> images = await ProcessImages(model.Photos, user, album);
                foreach (Image i in images) album.Images.Add(i);

            }


            _repository.AddAlbum(album);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> ViewAlbumImages(Guid albumId)
        {
            var user = await _userManager.GetUserAsync(User);
            List<Image> images = _repository.GetAlbumImages(albumId);
            Album album = _repository.GetAlbum(albumId);
            ViewAlbumImagesModel model = new ViewAlbumImagesModel
            {
                Images = images,
                Album = album,
                IsOwner = album.ownerId.Equals(user.Id),
                CurrentUserId = user.Id

            };

            foreach (Image i in model.Images)
            {
                i.Likes = _repository.GetLikes(i.ImageId);
                i.Comments = _repository.getComments(i.ImageId);
            }


            return View("Album", model);

        }

        [HttpGet]
        public ActionResult RenderImage(Guid id)
        {
            Image image = _repository.GetImage(id);

            var dir = System.Web.HttpContext.Current.Server.MapPath("/Uploads");
            var path = Path.Combine(dir, image.Path + ".jpg"); //validate the path for security or use other means to generate the path.
            return base.File(path, "image/jpeg");

        }

        public async Task<IActionResult> ViewSingle(Guid imageId)
        {
            var user = await _userManager.GetUserAsync(User);

            ImageViewModel image = new ImageViewModel
            {
                Image = _repository.GetImage(imageId),
                CurrentUserId = user.Id
            };
            image.IsOwner = image.Image.OwnerId.Equals(user.Id);
            return View("ViewSingle",image);

        }

        public Task<IActionResult> RemovePhotoFromAlbum(Guid imageId, Guid albumId)
        {
            _repository.RemovePhotoFromAlbum(imageId, albumId);
            return ViewAlbumImages(albumId);
        }

        public async Task<IActionResult> AddImages(Guid albumId, List<IFormFile> Photos)
        {
            Album album = _repository.GetAlbum(albumId);
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                List<Image> images = await ProcessImages(Photos, user, album);
                _repository.AddImagesToAlbum(albumId, images);
            }
            return await ViewAlbumImages(albumId);
        }

        private async Task<List<Image>> ProcessImages(IEnumerable<IFormFile> Photos, ApplicationUser user, Album album)
        {
            List<Image> images = new List<Image>();
            foreach (IFormFile file in Photos)
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
                images.Add(imageEntity);
                imageEntity.Albums.Add(album);
            }

            return images;
        }

        public async Task<IActionResult> Like(Guid imageId, Guid albumid)
        {
            var user = await _userManager.GetUserAsync(User);
            Like like = new Like
            {
                LikeId = Guid.NewGuid(),
                UserId = user.Id,
                UserName = user.UserName
            };
            _repository.Like(imageId, like);
            return await ViewAlbumImages(albumid);
        }

        public async Task<IActionResult> Dislike(Guid imageId, Guid albumid)
        {
            var user = await _userManager.GetUserAsync(User);
            _repository.Dislike(imageId, user.Id);
            return await ViewAlbumImages(albumid);
        }

        public async Task<IActionResult> LikeSingle(Guid imageId)
        {
            var user = await _userManager.GetUserAsync(User);
            Like like = new Like
            {
                LikeId = Guid.NewGuid(),
                UserId = user.Id,
                UserName = user.UserName,
            };
            _repository.Like(imageId, like);
            return await ViewSingle(imageId);
        }

        public async Task<IActionResult> DislikeSingle(Guid imageId)
        {
            var user = await _userManager.GetUserAsync(User);
            _repository.Dislike(imageId, user.Id);
            return await ViewSingle(imageId);
        }

        /*
        public async Task<IActionResult> ViewLikes(Guid imageId, Guid albumId)
        {
            var user = await _userManager.GetUserAsync(User);
            List<Like> likes = _repository.GetLikes(imageId);
            return await ViewAlbumImages(albumId);
        }*/

        [HttpPost]
        public async Task<IActionResult> AddCommentToAlbum(Guid albumId, string Comment)
        {
            var user = await _userManager.GetUserAsync(User);
           
            Comment comment = new Comment()
            {
                CommentId = Guid.NewGuid(),
                Content = Comment,
                UserId = user.Id,
                UserName = user.UserName

            };
            _repository.AddCommentToAlbum(albumId, comment);
            return await ViewAlbumImages(albumId);
        }

        [HttpPost]
        public async Task<IActionResult> AddCommentToImage(Guid imageid, string Comment)
        {
            var user = await _userManager.GetUserAsync(User);

            Comment comment = new Comment()
            {
                CommentId = Guid.NewGuid(),
                Content = Comment,
                UserId = user.Id,
                UserName = user.UserName

            };
            _repository.AddCommentToImage(imageid, comment);
            return await ViewSingle(imageid);
        }

        public async Task<IActionResult> AddToFavorites(Guid albumId)
        {
            var user = await _userManager.GetUserAsync(User);
            _repository.AddFavorite(user, albumId);
            return await ViewAlbumImages(albumId);
        }
    }

}

