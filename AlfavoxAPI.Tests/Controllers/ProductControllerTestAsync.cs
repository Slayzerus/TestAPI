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
    public abstract class BaseProductControllerTests
    {
        protected readonly List<Product> products;
        protected readonly Mock<IProductService> mockService;
        protected readonly ProductController controllerUnderTest;

        protected BaseProductControllerTests(List<Product> items)
        { 
            products = items;
            mockService = new Mock<IProductService>();
            mockService.Setup(svc => svc.FindAsync(1)).ReturnsAsync(new Product() { Id=1, ProductName = "Pen" });
            mockService.Setup(svc => svc.FindAsync("1-2-3")).ReturnsAsync(new List<Product> { new Product() { Id = 1, ProductName = "Pen" }, new Product() { Id = 2, ProductName = "Pencil" } });
            controllerUnderTest = new ProductController(mockService.Object);
        }
    }

    [TestClass]
    public class ProductControllerTestAsync : BaseProductControllerTests
    {
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        private static readonly Product product_first = new Product() { Id = 1, ProductName = "Pen" };
        private static readonly Product product_second = new Product() { Id = 2, ProductName = "Pencil" };

        public ProductControllerTestAsync() : base(new List<Product>() { product_first, product_second })
        { 
        
        }

        [TestMethod]
        public async Task GetSingleProduct()
        {
            var result = await controllerUnderTest.GetProduct(1);            
            var res = result as System.Web.Http.Results.OkNegotiatedContentResult<Product>;
            if (res != null)
            {
                Product pr = res.Content as Product;
                Assert.IsTrue(pr.Id == 1);
            }
                
        }


        
        [TestMethod]
        public async Task GetSomeProducts()
        {                        
            var result = await controllerUnderTest.GetProducts("1-2-3");
            
            var res = result as System.Web.Http.Results.OkNegotiatedContentResult<List<Product>>;
            if (res != null)
            {
                List<Product> pr = res.Content as List<Product>;                
                Assert.IsTrue(pr.Count > 0);
            }
            else { Assert.IsTrue(false); }
            
            
        }


        
    }
}
