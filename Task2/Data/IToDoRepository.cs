using Task2.Models;

namespace Task2.Data
{
    public interface IToDoRepository
    {
        IEnumerable<ToDo> GetAll();
        ToDo GetById(int id);
        ToDo Create(ToDo item);
        void Update(ToDo item);
        void Delete(int id);
    }
}
