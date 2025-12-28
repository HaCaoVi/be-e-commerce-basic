using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_commerce_basic.Database;
using e_commerce_basic.Dtos.Gallery;
using e_commerce_basic.Interfaces;
using e_commerce_basic.Models;

namespace e_commerce_basic.Repositories
{
    public class GalleryRepository : IGalleryRepository
    {
        private readonly ApplicationDBContext _context;
        public GalleryRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Gallery>> CreateGalleryAsync(List<Gallery> listGallery)
        {
            await _context.Galleries.AddRangeAsync(listGallery);
            await _context.SaveChangesAsync();
            return listGallery;
        }
    }
}