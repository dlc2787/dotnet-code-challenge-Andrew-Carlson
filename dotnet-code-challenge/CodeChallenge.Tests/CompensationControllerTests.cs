using System;
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
    public class CompensationControllerTests
    {
        //borrowing the instance variables, Initialize, and cleanup from Employee Controller tests again
        private static HttpClient _httpClient;
        private static TestServer _testServer;
        private static DateTime _exampleDate;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
            _exampleDate = DateTime.Now;
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        /// <summary>
        /// A method to test post requests with new compensations work as intended
        /// </summary>
        [TestMethod]
        public void TestCompensationCreated_Returns_Created()
        {
            //retrieve an employee (John Lennon) to construct compensation, ideally this would be mocked but I don't see any mocking libraries in the project
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            /* used if compensations held employee objects
            var getRequestTask = _httpClient.GetAsync($"api/employee/{employeeId}");
            var foundEmployee = getRequestTask.Result.DeserializeContent<Employee>();
            */

            //create compensation
            Compensation compensation = new Compensation()
            {
                //employee = foundEmployee,
                employee = employeeId,
                salary = 70000,
                effectiveDate = _exampleDate,
            };

            //package json
            var requestJson = new JsonSerialization().ToJson(compensation);

            //make request
            var postTask = _httpClient.PostAsync("api/compensation", new StringContent(requestJson, Encoding.UTF8, "application/json"));
            var response = postTask.Result;

            //assertions
            var recievedCompensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.IsNotNull(recievedCompensation.CompensationId);
            //Assert.AreEqual(compensation.employee.EmployeeId, recievedCompensation.employee.EmployeeId);
            Assert.AreEqual(compensation.employee, recievedCompensation.employee);
            Assert.AreEqual(compensation.salary, recievedCompensation.salary);
            Assert.AreEqual(compensation.effectiveDate, recievedCompensation.effectiveDate);

        }

        /// <summary>
        /// A method to test that get requests for compensations work as intended
        /// </summary>
        [TestMethod]
        public void TestGetCompensationById_Returns_Ok()
        {
            //retrieving an employee to create a compensation for
            var employeeId = "b7839309-3348-463b-a7e3-5de1c168beb3";
            /* would use this for a Compensation object that stores an employee object rather than ID
            var getEmployeeTask = _httpClient.GetAsync($"api/employee/{employeeId}");
            var foundEmployee = getEmployeeTask.Result.DeserializeContent<Employee>();
            */

            //create compensation
            Compensation compensation = new Compensation()
            {
                //employee = foundEmployee;
                employee = employeeId,
                salary = 70000,
                effectiveDate = _exampleDate,
            };

            //post compensation
            var requestJson = new JsonSerialization().ToJson(compensation);
            var setupPostRequest = _httpClient.PostAsync("api/compensation", new StringContent(requestJson, Encoding.UTF8, "application/json"));
            var setupResponse = setupPostRequest.Result;
            var setupCompensation = setupResponse.DeserializeContent<Compensation>();

            //setup expected salary
            var expectedSalary = 70000;

            //make get request
            //var getRequestTask = _httpClient.GetAsync($"api/compensation/{setupCompensation.CompensationId}");
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{setupCompensation.employee}");
            var response = getRequestTask.Result;

            //assertions
            var recievedCompensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsNotNull(recievedCompensation);
            //Assert.AreEqual(employeeId, recievedCompensation.employee.EmployeeId);
            Assert.AreEqual(employeeId, recievedCompensation.employee);
            Assert.AreEqual(expectedSalary, recievedCompensation.salary);
            Assert.AreEqual(_exampleDate, recievedCompensation.effectiveDate);
        }

        /// <summary>
        /// A method to test that bad id's return a not found status
        /// </summary>
        [TestMethod]
        public void TestBadEmployeeId_Returns_NotFound()
        {
            //setup
            var employeeId = "magical numbers";

            //make request
            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;

            //assertions
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
