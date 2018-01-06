using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Picterest.Models
{
    public interface IGalleryRepo
    {
        Album GetAlbum(Guid albumId);
        Image GetImage(Guid imageId);
        void AddImage(Image image);
        void AddAlbum(Album album);
        Task<List<Album>> GetUserAlbums(string UserId);
    }
}
