using HardwareScrapper.Business.Abstractions;
using HardwareScrapper.DataAccess.Abstractions;
using HardwareScrapper.Domain.Entities;

namespace HardwareScrapper.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Category GetCategoryById(int id)
        {
            return _unitOfWork.CategoryRepository.GetById(id);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _unitOfWork.CategoryRepository.GetAll();
        }

        public Category AddCategory(Category category)
        {
            return _unitOfWork.CategoryRepository.Add(category);
        }

        public void UpdateCategory(Category category)
        {
            _unitOfWork.CategoryRepository.Update(category);
        }

        public void DeleteCategory(int id)
        {
            _unitOfWork.CategoryRepository.Delete(id);
        }
    }
}
