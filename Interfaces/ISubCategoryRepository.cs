namespace e_commerce_basic.Interfaces
{
    public interface ISubCategoryRepository
    {
        Task<bool> IsSubCategoryIdExist(int subCategoryId);
    }
}