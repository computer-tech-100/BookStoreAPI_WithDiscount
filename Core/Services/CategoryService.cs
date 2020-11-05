using System.Collections.Generic;//List
using System.Linq;//Select()
using System.Threading.Tasks;//Task
using Microsoft.EntityFrameworkCore;
using MyAPI.Core.Contexts;
using MyAPI.Core.Models.DbEntities;
using MyAPI.Core.Models.DTOs;

namespace MyAPI.Core.Services
{
    public class CategoryService : ICategoryService
    {
        //service always references context
        private readonly MyAppDbContext _context;

        public CategoryService(MyAppDbContext context)
        {
            _context = context;

        }

        public async Task CreateCategory(CategoryDTO cateogry)
        {
            Category newCategory = new Category()
            {
                //CategoryId is auto generated
                CategoryName = cateogry.CategoryName

            };

            _context.Categories.Add(newCategory);

            await _context.SaveChangesAsync();

            cateogry.CategoryId = newCategory.CategoryId;//Add CategoryId to DTO and return it
  
        }

        public async Task <List<CategoryDTO>> GetAllCategories()
        {
            //Translate the model to DTO by using Select()
            return (await _context.Categories.ToListAsync()).Select(i => new CategoryDTO
            {
                CategoryId = i.CategoryId,
                
                CategoryName = i.CategoryName

            }).ToList();

        }

        public async Task<CategoryDTO> GetOneCategory(int id)
        {
            return (await _context.Categories.ToListAsync()).Select(i => new CategoryDTO
            {
                CategoryId = i.CategoryId,
                
                CategoryName = i.CategoryName

            }).FirstOrDefault(x => x.CategoryId == id);

        }

        public async Task<CategoryDTO> EditCategory(CategoryDTO category)
        {
            Category chosenCategory = _context.Categories.Single(x => x.CategoryId == category.CategoryId);

            chosenCategory.CategoryName = category.CategoryName;//edit the name

            await _context.SaveChangesAsync();

            return category;

        }
        public async Task RemoveCategory(int? id)
        {
            var category = _context.Categories.Single(e => e.CategoryId == id);

            _context.Categories.Remove(category);//Remove category

            await _context.SaveChangesAsync();//Save the changes
           
        }
    }

    public interface ICategoryService
    {
        Task CreateCategory(CategoryDTO category);//post

        Task <List<CategoryDTO>> GetAllCategories();//Get

        Task <CategoryDTO> GetOneCategory(int id);//Get by id

        Task <CategoryDTO> EditCategory(CategoryDTO category);//Put

        Task RemoveCategory(int? id);//Delete

    }
}