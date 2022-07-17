using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        //instanbce variables
        private readonly ILogger<ICompensationRepository> _logger;
        private readonly CompensationContext _compensationContext;

        //constructor
        public CompensationRepository(ILogger<ICompensationRepository> logger, CompensationContext compContext)
        {
            _compensationContext = compContext;
            _logger = logger;
        }

        /// <summary>
        /// A method to add a new compensation to the compensation context
        /// </summary>
        /// <param name="compensation">The new compensation object to add</param>
        /// <returns>The added compensation object</returns>
        public Compensation Add(Compensation compensation)
        {
            //add the compensation to the context
            compensation.CompensationId = Guid.NewGuid().ToString();
            //temp relation
            _compensationContext.Compensations.Add(compensation);
            return compensation;
        }

        /// <summary>
        /// A method to find a compensation by employee ID
        /// </summary>
        /// <param name="id">The employee ID to search for</param>
        /// <returns>The compensation object for the employee who's ID was provided</returns>
        public Compensation GetById(string id)
        {
            //pass a function to filter for a compensation with an employee who's ID matches the provided ID to the DbSet in the context
            //var res = _compensationContext.Compensations.SingleOrDefault(e => e.CompensationId == id);
            return _compensationContext.Compensations.SingleOrDefault(e => e.employee == id); ;
        }

        /// <summary>
        /// Method to save changes to the context/database
        /// </summary>
        /// <returns>The asynchronous task to save changes</returns>
        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }
    }
}
