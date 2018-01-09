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
        Task<List<Image>> GetAlbumImages(Guid albumId, string id);
        void RemovePhotoFromAlbum(Guid imageId, Guid albumId);
    }
}
