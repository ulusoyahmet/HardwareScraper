using HardwareScrapper.Domain.Entities;

namespace HardwareScrapper.Business.Abstractions
{
    public interface ICategoryService
    {
        Category GetCategoryById(int id);
        IEnumerable<Category> GetAllCategories();
        Category AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int id);
    }
}
