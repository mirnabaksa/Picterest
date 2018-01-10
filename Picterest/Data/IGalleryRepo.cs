using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Picterest.Models;

namespace Picterest.Data
{
    public interface IGalleryRepo
    {
        Album GetAlbum(Guid albumId);
        Image GetImage(Guid imageId);
        void AddImage(Image image);
        void AddAlbum(Album album);
        Task<List<Album>> GetUserAlbums(string UserId);
        List<Image> GetAlbumImages(Guid albumId);
        void RemovePhotoFromAlbum(Guid imageId, Guid albumId);
        Task<List<Album>> FilterAlbums(string filter);
    }
}
