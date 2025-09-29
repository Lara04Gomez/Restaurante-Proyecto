using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.ICategory.Repository;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Command
{
    public class CategoryCommand : ICategoryCommand
    {
        private readonly AppDbContext _context;
        public CategoryCommand(AppDbContext context)
        {
            _context = context;
        }
        public  Task InsertCategory(Category category)
        {
            _context.Categories.Add(category);
            return Task.CompletedTask;
        }

        public  Task RemoveCategory(Category category)
        {
            _context.Categories.Remove(category);
            return Task.CompletedTask;
        }

        public Task UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            return Task.CompletedTask;
        }
    }
}