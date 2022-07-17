using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;

using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeChallenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
    {
        //borrowing test class setup from the Employee Controller test
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        /// <summary>
        /// A test method to ensure the return reporting structure has the proper employee and proper report count
        /// </summary>
        [TestMethod]
        public void GetReportStructure_Returns_Ok()
        {
            //checking for John Lennon, who should have 4 reports
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedFirstName = "John";
            var expectedLastName = "Lennon";
            var expectedReports = 4;

            //retrieve from server
            var GETRequestTask = _httpClient.GetAsync($"api/reportingstructure/{employeeId}");
            var res = GETRequestTask.Result;

            //assertions
            var structure = res.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
            Assert.AreEqual(expectedFirstName, structure.employee.FirstName);
            Assert.AreEqual(expectedLastName, structure.employee.LastName);
            Assert.AreEqual(expectedReports, structure.numberOfReports);
        }

        /// <summary>
        /// A test method to ensure a bad employee string returns the proper status code
        /// </summary>
        [TestMethod]
        public void BadEmployeeID_Returns_NotFound()
        {
            //set up and recieve response from server
            var employeeId = "nonsense string";

            var GETRequestTask = _httpClient.GetAsync($"api/reportingstructure/{employeeId}");
            var res = GETRequestTask.Result;

            //assertions
            Assert.AreEqual(HttpStatusCode.NotFound, res.StatusCode);
        }
    }
}
