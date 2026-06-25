using webapiCourse.models;

namespace webapiCourse.Reposotiry
{
    public interface ICrudOperations<T>
    {

        Task<IEnumerable<T>> GetAll();

        Task<T> GetByID(int id);


        Task<T> Add(T category);
        Task Update(T category);

        Task Delete(int id);
    }
}
