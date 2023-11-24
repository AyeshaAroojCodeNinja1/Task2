using System.Collections.Generic;
using Task2.Models;

namespace Task2.Data
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ToDoDbContext _context;
        public ToDoRepository(ToDoDbContext context)
        {
            _context = context;
        }
        public IEnumerable<ToDo> GetAll()
        {
            return _context.ToDo.ToList();
        }

        public ToDo GetById(int id)
        {
            return _context.ToDo.FirstOrDefault(ToDo => ToDo.Id == id) ?? null;
        }

        public ToDo Create(ToDo item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(ToDo));
            }
            try
            {
                if (_context.ToDo != null)
                    _context.ToDo.Add(item);

                _context.SaveChanges();
                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(ToDo item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            try
            {
                if (_context.ToDo != null)
                {
                    var toDoItem = _context.ToDo.FirstOrDefault(ToDo => ToDo.Id == item.Id);
                    if (toDoItem != null)
                    {
                        toDoItem.Title = item.Title;
                        toDoItem.IsCompleted = item.IsCompleted;
                    }
                    _context.SaveChanges();
                }
                else
                    throw new ArgumentNullException(nameof(item));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var item = _context.ToDo.FirstOrDefault(ToDo => ToDo.Id == id);
                if (item != null)
                {
                    _context.ToDo.Remove(item);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
