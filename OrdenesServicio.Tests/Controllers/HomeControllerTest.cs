using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrdenesServicio.Controllers;

namespace OrdenesServicio.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            //ViewResult result = controller.Index() as ViewResult;

            // Assert
            //Assert.AreEqual("Welcome to ASP.NET MVC!", result.ViewBag.Message);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
           // ViewResult result = controller.About() as ViewResult;

            // Assert
            //Assert.IsNotNull(result);
        }
    }
}
