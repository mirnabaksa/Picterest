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
            throw new NotImplementedException();
        }

        public Image GetImage(Guid imageId)
        {
            throw new NotImplementedException();
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
           return await _context.Albums.Where(i => i.ownerId.Equals(UserId)).ToListAsync();
        }
    }
}

