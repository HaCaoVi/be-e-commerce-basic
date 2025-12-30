using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_commerce_basic.Database;
using e_commerce_basic.Dtos.Gallery;
using e_commerce_basic.Interfaces;
using e_commerce_basic.Models;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_basic.Repositories
{
    public class GalleryRepository : IGalleryRepository
    {
        private readonly ApplicationDBContext _context;
        public GalleryRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Gallery>> CreateAsync(List<Gallery> listGallery)
        {
            await _context.Galleries.AddRangeAsync(listGallery);
            return listGallery;
        }

        public async Task<List<Gallery>> GetListByProductIdAsync(int productId)
        {
            return await _context.Galleries.Where(p => p.ProductId == productId).ToListAsync();
        }

        public async Task<bool> UpdateAsync(int productId, List<Gallery> galleries)
        {
            await _context.Galleries.Where(g => g.ProductId == productId).ExecuteDeleteAsync();
            _context.Galleries.AddRange(galleries);
            return true;
        }
    }
}