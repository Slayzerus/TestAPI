using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AlfavoxAPI.Models;

namespace AlfavoxAPI.Controllers
{
    public class ProductController : ApiController
    {
        
        private ProductContext db;
        private ProductService _service;

        


        public ProductController() 
        {

            _service = new ProductService(new ProductContext());
            db = new ProductContext();
        }

        public ProductController(ProductContext _db) 
        {
            db = _db;
        }

        public ProductController(ProductService service)
        {
            _service = service;
            db = new ProductContext();
        }


        // GET: api/Product
        public IQueryable<Product> GetProducts()
        {
            return db.Products;            
        }

        // GET: api/Product/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> GetProduct(long id)
        {
            //Product product = await db.Products.FindAsync(id);
            Product product = await _service.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }


        // GET: api/Products/
        [Route("api/Products")]
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> GetProducts([FromUri] string ids)
        {
            string[] keyss = ids.Split('-');
            List<long> keys = new List<long>();
            foreach (string k in keyss)
            {
                long sk = long.Parse(k);
                if (!keys.Contains(sk)) { keys.Add(sk); }
            }
            List<Product> list =  await db.Products.Where(t => keys.Contains(t.Id)).ToListAsync();
            if (list.Count > 0){ return Ok(list); }
            else { return NotFound(); }
        }


        // PUT: api/Product/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct(long id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            db.Entry(product).State = 
                EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Product
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // DELETE: api/Product/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(long id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(long id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}
 