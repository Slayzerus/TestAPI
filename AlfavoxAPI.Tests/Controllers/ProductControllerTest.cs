using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlfavoxAPI;
using AlfavoxAPI.Controllers;
using System.Diagnostics;
using AlfavoxAPI.Models;
using Moq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;

namespace AlfavoxAPI.Tests.Controllers
{
    [TestClass]
    public class ProductControllerTest
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }


        [TestMethod]
        public void CheckMockDb()
        {
            ProductContext ctx = GetMockContext();

            Assert.IsNotNull(ctx);
            Assert.IsTrue(ctx.Products.ToList().Count > 0);
        }






        [TestMethod]
        public void GetAllProducts()
        {
            //Arrange
            ProductContext ctx = GetMockContext();
            ProductController pc = new ProductController(ctx);

            //Act
            IQueryable<Product> result = pc.GetProducts() as IQueryable<Product>;            

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ToList().Count > 0);
        }


        [TestMethod]
        public async Task GetSomeProducts()
        {
            //Arrange
            ProductContext ctx = GetMockContext();
            ProductController pc = new ProductController(ctx);

            //Act
            var result = await pc.GetProducts("1-2-3");
            
            //Assert
            Assert.IsNotNull(result);          
        }


        [TestMethod]
        public async Task GetSingleProduct()
        {
            //Arrange
            ProductContext ctx = GetMockContext();
            ProductController pc = new ProductController(ctx);
            ProductService service = GetMockService();
            Product pr = ctx.Products.ToList().FirstOrDefault();                        
            
            TestContext.WriteLine("Checking Id:"+ pr.Id.ToString());
            TestContext.WriteLine("Checking Name:" + pr.ProductName);

            //Act
            var response = await service.FindAsync(1);            

            TestContext.WriteLine("Response:"+response.ToString());

            


            //Assert
            Assert.IsNotNull(response);
        }





        #region Test Data and Mock Context

        public ProductContext GetMockContext() 
        {
            List<Product> tps = GetTestProducts();
            Mock<ProductContext> db = new Mock<ProductContext>();
            Mock<DbSet<Product>> mockDbSet = GetMockDbSet<Product>(tps);            
            db.Setup(c => c.Set<Product>()).Returns(mockDbSet.Object);            
            return db.Object;
        }

        public ProductService GetMockService()
        {
            ProductContext mockCtx = GetMockContext();
            Mock<ProductService> service = new Mock<ProductService>(mockCtx);
            service.Setup(m => m.FindAsync(1)).ReturnsAsync(new Product() { Id = 1, ProductName = "Pen" });
            return service.Object;
        }
        
        public List<Product> GetTestProducts() 
        {
            List<Product> context = new List<Product>();            
            context.Add(new Product() { Id = 1, ProductName = "Pen" });
            context.Add(new Product() { Id = 2, ProductName = "Pencil" });
            context.Add(new Product() { Id = 3, ProductName = "Notebook" });
            context.Add(new Product() { Id = 4, ProductName = "Clipboard" });
            return context;
        }
        
        internal static Mock<DbSet<T>> GetMockDbSet<T>(ICollection<T> entities) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entities.AsQueryable().Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.AsQueryable().Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.AsQueryable().ElementType);            
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.AsQueryable().GetEnumerator());            
            mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(entities.Add);
            return mockSet;
        }

        #endregion
    }
}
