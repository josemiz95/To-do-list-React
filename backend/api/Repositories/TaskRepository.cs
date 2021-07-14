using api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = api.Models.Task;

namespace api.Repositories
{
    public class TaskRepository : ITasks<Task, int>
    {
        private readonly ApplicationDbContext context;

        public TaskRepository(ApplicationDbContext context) => this.context = context;

        public IEnumerable<Task> GetAll()
        {
            return context.Tasks.ToList();
        }

        public Task GetById(int id)
        {
            return context.Tasks.Find(id);
        }

        public Task Insert(Task entity)
        {
            context.Tasks.Add(entity);
            this.Save();

            return entity;
        }

        public Task Update(Task entity)
        {
            // task.date = !task.pending? DateTime.Now : null;  For newer version

            var old_task = this.GetById(entity.id);

            if (entity.pending) // Set time stamp
            {
                entity.date = null;
            }
            else if(old_task.date == null)
            {
                entity.date = DateTime.Now;
            }

            context.Entry(old_task).State = EntityState.Detached; // Detach
            context.Entry(entity).State = EntityState.Modified; // Modify Task

            this.Save();

            return entity;
        }

        public Boolean Delete(Task entity)
        {
            context.Tasks.Remove(entity);
            this.Save();

            return true;
        }

        public Boolean Any(int id)
        {
            return context.Tasks.Any(e => e.id == id);
        }

        public Boolean Save()
        {
            context.SaveChanges();
            return true;
        }
    }
}
