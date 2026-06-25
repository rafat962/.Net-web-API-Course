using Microsoft.EntityFrameworkCore;
using webapiCourse.Data;
using webapiCourse.models;

namespace webapiCourse.Reposotiry
{
    public class CategoryRepo : ICategoryRepo
    {
         
        public CategoryRepo(DataDBContext dataDBContext)
        {
            DataDBContext = dataDBContext;
        }

        public DataDBContext DataDBContext { get; }

        public async Task<Category> Add(Category category)
        {

            await DataDBContext.Categories.AddAsync(category);

            await DataDBContext.SaveChangesAsync();

            return category;

        }

        public async Task Delete(int id)
        {
            var categoryRepo = await GetByID(id);

            if (categoryRepo == null)
            {
                throw new Exception($"There is no Category With id = ${id}");
            }

            DataDBContext.Remove(categoryRepo);
            await DataDBContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await DataDBContext.Categories.ToListAsync();
        }

        public async Task<Category> GetByID(int id)
        {
            return await DataDBContext.Categories.FirstOrDefaultAsync(cate => cate.Id == id);
        }

        public async Task Update(Category category)
        {
            DataDBContext.Categories.Update(category);
            await DataDBContext.SaveChangesAsync();
             
        }
    }
}
