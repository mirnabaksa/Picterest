﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Picterest.Data;
using Picterest.Models;

namespace Picterest.Data
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
           return  _context.Albums.Where(a => a.AlbumId.Equals(albumId)).Include(i => i.Images).Include(i => i.Comments).FirstOrDefault();
        }

        public Image GetImage(Guid imageId)
        {
            return _context.Images.Where(a => a.ImageId.Equals(imageId)).Include(i => i.Albums).Include(i => i.Likes).Include(i => i.Comments).FirstOrDefault();
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

        public List<Image> GetAlbumImages(Guid albumId)
        {
            Album album =  GetAlbum(albumId);
            return album?.Images.ToList();
        }

        public void RemovePhotoFromAlbum(Guid imageId, Guid albumId)
        {
            Album album = GetAlbum(albumId);
            Image image = GetImage(imageId);
            album.Images.Remove(image);
            _context.SaveChanges();

        }

        public Task<List<Album>> FilterAlbums(string filter)
        {
            return _context.Albums.Where(a => a.Name.Trim().ToLower().Contains(filter.Trim().ToLower()) || a.Description.Trim().Contains(filter.Trim().ToLower())).Include(i=> i.Images).ToListAsync();
        }

        public void AddImagesToAlbum(Guid albumId, IEnumerable<Image> images)
        {
            Album album = GetAlbum(albumId);
            foreach(Image i in images) album.Images.Add(i);
            _context.SaveChanges();
        }

        public void Like(Guid imageId, Like like)
        {
            Image image = GetImage(imageId);
            image.Likes.Add(like);
            _context.SaveChanges();
        }

        public List<Like> GetLikes(Guid imageId)
        {
            Image image = GetImage(imageId);
            return image.Likes;
        }

        public void AddCommentToAlbum(Guid albumId, Comment Comment)
        {
            Album album = GetAlbum(albumId);
            album.Comments.Add(Comment);
            _context.SaveChanges();
        }

        public void AddCommentToImage(Guid imageid, Comment comment)
        {
            Image image = GetImage(imageid);
            image.Comments.Add(comment);
            _context.SaveChanges();
        }
    }
}
