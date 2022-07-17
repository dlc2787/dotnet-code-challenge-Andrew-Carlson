using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        //need access to employee repositiory, as reporting structures aren't persistent
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<ReportingStructureService> _logger;

        //keeping consistent constructor format
        public ReportingStructureService(ILogger<ReportingStructureService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates and serves a new ReportingStructure object with the retrieved employee and the total number of reports under them
        /// </summary>
        /// <param name="id">Employee ID</param>
        /// <returns>The ReportingStructure object or null if no employee is found</returns>
        public ReportingStructure createReportingStructure(string id)
        {
            ReportingStructure res = new ReportingStructure();
            //retrieve employee
            if (!String.IsNullOrEmpty(id))
            {
                Employee employee = _employeeRepository.GetById(id);
                //check employee isn't null
                if (employee != null)
                {
                    //fill and calculate ReportingStructure
                    res.employee = employee;
                    res.numberOfReports = TotalReports(employee);

                    return res;
                }
            }
            //bad employee string or employee is null
            return null;
        }

        /// <summary>
        /// Recursively totals all reports under an employee
        /// </summary>
        /// <param name="employee">Employee to total reports for</param>
        ///<returns>Total reports for the provided employee</returns>
        private int TotalReports(Employee employee)
        {
            int currentTotal = 0;
            //get number of reports for this specific employee
            if (employee.DirectReports != null)
            {
                currentTotal = employee.DirectReports.Count;
                //for each employee under this employee, get their number of reports recursively
                foreach (Employee em in employee.DirectReports)
                {
                    currentTotal += TotalReports(em);
                }
            }
            return currentTotal;
        }
    }
}
