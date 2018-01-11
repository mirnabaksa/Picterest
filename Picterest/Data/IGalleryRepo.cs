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
        void AddImagesToAlbum(Guid albumId, IEnumerable<Image> images);
        void Like(Guid imageId, Like like);
        List<Like> GetLikes(Guid imageId);
        void AddCommentToAlbum(Guid albumId, Comment comment);
        void AddCommentToImage(Guid imageid, Comment comment);
    }
}
