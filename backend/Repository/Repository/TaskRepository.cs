namespace Repository.Repository
{
    using Contracts;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _Context;

        public TaskRepository(ApplicationDbContext Context) => _Context = Context;

        public IEnumerable<Task> All()
        {
            return _Context.Tasks.ToList();
        }

        public bool Create(Task entity)
        {
            _Context.Tasks.Add(entity);
            
            return this.Save();
        }

        public bool Delete(Task entity)
        {
            _Context.Tasks.Remove(entity);

            return this.Save();
        }

        public bool Exists(int id)
        {
            return _Context.Tasks.Any(e => e.id == id);
        }

        public Task Find(int id)
        {
            return _Context.Tasks.Find(id);
        }

        public bool Save()
        {
            var changes = this._Context.SaveChanges();
            return changes > 0;
        }

        public bool Update(Task entity)
        {
            this._Context.Tasks.Update(entity);
            return this.Save();
        }
    }
}
