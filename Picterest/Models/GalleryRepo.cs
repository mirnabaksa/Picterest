using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Picterest.Data;

namespace Picterest.Models
{
    
    public class GalleryRepo : IGalleryRepo
    {
        private readonly GalleryDbContext _context;

        public GalleryRepo(GalleryDbContext context)
        {
            _context = context;
        }

        public Album GetAlbum(Guid albumId)
        {
           return  _context.Albums.Where(a => a.AlbumId.Equals(albumId)).Include(i => i.Images).FirstOrDefault();
        }

        public Image GetImage(Guid imageId)
        {
            return _context.Images.Where(a => a.ImageId.Equals(imageId)).Include(i => i.Albums).FirstOrDefault();
        }

        public void AddImage(Image image)
        {
            _context.Images.Add(image);
            _context.SaveChanges();
        }

        public void AddAlbum(Album album)
        {
            _context.Albums.Add(album);
            _context.SaveChanges();
        }

        public async Task<List<Album>> GetUserAlbums(string UserId)
        {
           return await _context.Albums
                .Where(i => i.ownerId.Equals(UserId))
                .Include(i => i.Images)
                .ToListAsync();
        }

        public async Task<List<Image>> GetAlbumImages(Guid albumId, string id)
        {
            List<Album> albums = await GetUserAlbums(id);
            Album album = albums?.Where(a => a.AlbumId.Equals(albumId)).FirstOrDefault();
            return album?.Images.ToList();
        }

        public void RemovePhotoFromAlbum(Guid imageId, Guid albumId)
        {
            Album album = GetAlbum(albumId);
            Image image = GetImage(imageId);
            album.Images.Remove(image);
            _context.SaveChanges();

        }
    }
}

