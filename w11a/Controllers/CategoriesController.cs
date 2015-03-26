using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using w11a.Models;

namespace w11a.Controllers
{
    public class CategoriesController : ApiController
    {
        private NorthwindEntities db = new NorthwindEntities();

        // GET: api/Categories
        public IQueryable<Category> GetCategories()
        {
            //return db.Categories;
            var query = from c in db.Categories
                        select new { c.CategoryID, c.CategoryName, c.Description };
            List<Category> categoryitems = new List<Category>();
            foreach (var item in query)
            {
                categoryitems.Add(new Category
                {
                    CategoryID = item.CategoryID,
                    CategoryName = item.CategoryName,
                    Description = item.Description
                });
            }
            return categoryitems.AsQueryable();
        }

        // GET: api/Categories/5
        [ResponseType(typeof(Category))]
        public IHttpActionResult GetCategory(int id)
        {
            Category category = db.Categories.Find(id);
            

            if (category == null)
            {
                return NotFound();
            }
            Category shortcategory = new Category
            {
                CategoryID = category.CategoryID,
                CategoryName = category.CategoryName,
                Description = category.Description
            };
            return Ok(shortcategory);
        }

        // PUT: api/Categories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.CategoryID)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Categories
        [ResponseType(typeof(Category))]
        public IHttpActionResult PostCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = category.CategoryID }, category);
        }

        // DELETE: api/Categories/5
        [ResponseType(typeof(Category))]
        public IHttpActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return Ok(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.CategoryID == id) > 0;
        }
    }
}