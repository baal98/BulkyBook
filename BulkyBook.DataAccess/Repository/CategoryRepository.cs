using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Category category)
        {
            _db.Categories.Update(category);
            //var objFromDb = _db.Categories.FirstOrDefault(s => s.Id == category.Id);
            //if (objFromDb != null)
            //{
            //    objFromDb.Name = category.Name;
            //    objFromDb.DisplayOrder = category.DisplayOrder;

            //}
        }
    }
}
