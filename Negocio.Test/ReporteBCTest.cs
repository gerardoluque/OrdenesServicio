using ZOE.OrdenesServicio.Negocio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using ZOE.OrdenesServicio.Model.Reportes;
using System.Collections.Generic;

namespace Negocio.Test
{
    
    
    /// <summary>
    ///This is a test class for ReporteBCTest and is intended
    ///to contain all ReporteBCTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ReporteBCTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ObtenerPolizas
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        //[HostType("ASP.NET")]
        //[AspNetDevelopmentServerHost("C:\\ZOE Proyectos\\OrdenesServicio\\OrdenesServicio", "/")]
        //[UrlToTest("http://localhost:13224/")]
        public void ObtenerPolizasTest()
        {
            ReporteBC target = new ReporteBC(); // TODO: Initialize to an appropriate value
            List<PolizaDatos> expected = null; // TODO: Initialize to an appropriate value
            List<PolizaDatos> actual;
            actual = target.ObtenerPolizas();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ObtenerTiposServicioPorAsesor
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\ZOE Proyectos\\OrdenesServicio\\OrdenesServicio", "/")]
        [UrlToTest("http://localhost:13224/")]
        public void ObtenerTiposServicioPorAsesorTest()
        {
            ReporteBC target = new ReporteBC(); // TODO: Initialize to an appropriate value
            List<TiposServicioAsesor> expected = null; // TODO: Initialize to an appropriate value
            List<TiposServicioAsesor> actual;
            actual = target.ObtenerTiposServicioPorAsesor();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ObtenerTiposServicioPorCliente
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\ZOE Proyectos\\OrdenesServicio\\OrdenesServicio", "/")]
        [UrlToTest("http://localhost:13224/")]
        public void ObtenerTiposServicioPorClienteTest()
        {
            ReporteBC target = new ReporteBC(); // TODO: Initialize to an appropriate value
            List<TiposServicioCliente> expected = null; // TODO: Initialize to an appropriate value
            List<TiposServicioCliente> actual;
            actual = target.ObtenerTiposServicioPorCliente();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ObtenerPolizaDetalleServiciosPorPoliza
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\ZOE Proyectos\\OrdenesServicio\\OrdenesServicio", "/")]
        [UrlToTest("http://localhost:13224/")]
        public void ObtenerPolizaDetalleServiciosPorPolizaTest()
        {
            ReporteBC target = new ReporteBC(); // TODO: Initialize to an appropriate value
            int polizaId = 0; // TODO: Initialize to an appropriate value
            List<PolizaSaldo> expected = null; // TODO: Initialize to an appropriate value
            List<PolizaSaldo> actual = null;
            //actual = target.ObtenerPolizaDetalleServiciosPorPoliza(polizaId);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ObtenerPolizaDetalleServicios
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("C:\\ZOE Proyectos\\OrdenesServicio\\OrdenesServicio", "/")]
        [UrlToTest("http://localhost:13224/")]
        public void ObtenerPolizaDetalleServiciosTest()
        {
            ReporteBC target = new ReporteBC(); // TODO: Initialize to an appropriate value
            List<PolizaSaldo> expected = null; // TODO: Initialize to an appropriate value
            List<PolizaSaldo> actual;
            //actual = target.ObtenerPolizaDetalleServicios();
            //Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ObtenerPolizasSaldoPorAgotar
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        //[HostType("ASP.NET")]
        //[AspNetDevelopmentServerHost("C:\\ZOE Proyectos\\OrdenesServicio\\OrdenesServicio", "/")]
        //[UrlToTest("http://localhost:13224/")]
        public void ObtenerPolizasSaldoPorAgotarTest()
        {
            ReporteBC target = new ReporteBC(); // TODO: Initialize to an appropriate value
            int saldoMin = 10; // TODO: Initialize to an appropriate value
            int saldoMax = 500; // TODO: Initialize to an appropriate value
            List<PolizaDatos> expected = null; // TODO: Initialize to an appropriate value
            List<PolizaDatos> actual;
            actual = target.ObtenerPolizasSaldoPorAgotar(saldoMin, saldoMax, true);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ObtenerTotalHorasAsesoresPorFecha
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        //[HostType("ASP.NET")]
        //[AspNetDevelopmentServerHost("C:\\ZOE Proyectos\\OrdenesServicio\\OrdenesServicio", "/")]
        //[UrlToTest("http://localhost:13224/")]
        public void ObtenerTotalHorasAsesoresPorFechaTest()
        {
            ReporteBC target = new ReporteBC(); // TODO: Initialize to an appropriate value
            DateTime fechaInicial = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime fechaFinal = new DateTime(); // TODO: Initialize to an appropriate value
            List<ActividadesAsesor> expected = null; // TODO: Initialize to an appropriate value
            List<ActividadesAsesor> actual;
            
            fechaInicial = System.DateTime.Now;
            fechaFinal = System.DateTime.Now;
            fechaInicial = fechaInicial.AddMonths(-10);
            actual = target.ObtenerTotalHorasAsesoresPorFecha(fechaInicial, fechaFinal);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ObtenerOrdenesServicioPorDia
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        //[HostType("ASP.NET")]
        //[AspNetDevelopmentServerHost("C:\\ZOE Proyectos\\OrdenesServicio\\OrdenesServicio", "/")]
        //[UrlToTest("http://localhost:13224/")]
        public void ObtenerOrdenesServicioPorDiaTest()
        {
            ReporteBC target = new ReporteBC(); // TODO: Initialize to an appropriate value
            DateTime fechaInicial = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime fechaFinal = new DateTime(); // TODO: Initialize to an appropriate value
            List<OrdenServicioAsesor> expected = null; // TODO: Initialize to an appropriate value
            List<OrdenServicioAsesor> actual;

            //fechaInicial = Convert.ToDateTime("2014-02-11");
            //fechaFinal = Convert.ToDateTime("2014-02-11");

            fechaInicial = Convert.ToDateTime("2014-03-03");
            fechaFinal = Convert.ToDateTime("2014-03-03");

            actual = target.ObtenerOrdenesServicioPorDia(fechaInicial, fechaFinal);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void ObtenerTicketsPrioridadTest()
        {
            ReporteBC target = new ReporteBC(); // TODO: Initialize to an appropriate value
            DateTime fechaInicial = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime fechaFinal = new DateTime(); // TODO: Initialize to an appropriate value
            int clienteId = 2;//295; // TODO: Initialize to an appropriate value  
            int proyectoId = 0; // TODO: Initialize to an appropriate value
            int asesorId = 0; // TODO: Initialize to an appropriate value
            int ticket = 13199; // TODO: Initialize to an appropriate value

            List<PolizaSaldo> expected = null; // TODO: Initialize to an appropriate value

            //fechaInicial = Convert.ToDateTime("2014-02-11");
            //fechaFinal = Convert.ToDateTime("2014-02-11");

            fechaInicial = Convert.ToDateTime("2014-03-03");
            fechaFinal = Convert.ToDateTime("2025-03-03");

            var actual = target.ObtenerTicketsDetallePrioridad(clienteId, proyectoId, asesorId, fechaInicial, fechaFinal, ticket, true);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
