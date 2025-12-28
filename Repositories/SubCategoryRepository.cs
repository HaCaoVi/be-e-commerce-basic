using e_commerce_basic.Database;
using e_commerce_basic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace e_commerce_basic.Repositories
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly ApplicationDBContext _context;
        public SubCategoryRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> IsSubCategoryIdExist(int subCategoryId)
        {
            return await _context.SubCategories.AnyAsync(s => s.Id == subCategoryId);
        }
    }
}