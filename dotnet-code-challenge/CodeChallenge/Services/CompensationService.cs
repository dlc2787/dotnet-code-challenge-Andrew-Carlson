using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class CompensationService : ICompensationService
    {
        //instance vars
        private readonly ILogger<CompensationService> _logger;
        private readonly ICompensationRepository _compensationRepository;

        //constructor
        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository)
        {
            _compensationRepository = compensationRepository;
            _logger = logger;
        }

        /// <summary>
        /// A method to create a new compensation object in the databases
        /// </summary>
        /// <param name="compensation">The compensation to store</param>
        /// <returns>The stored compensation, or null if improperly provided</returns>
        public Compensation Create(Compensation compensation)
        {
            //if not null
            if (compensation != null)
            {
                //add the compensation to the database and then save
                _compensationRepository.Add(compensation);
                _compensationRepository.SaveAsync().Wait();
            }
            return compensation;
        }

        /// <summary>
        /// A method to retrieve a compensation by employee ID
        /// </summary>
        /// <param name="id">The employee id to retrieve a compensation for</param>
        /// <returns>The found compensation object, or null if no compensation was found for the given ID</returns>
        public Compensation GetCompById(string id)
        {
            //if not null
            if (!String.IsNullOrEmpty(id))
            {
                //get compensation by employee id
                return _compensationRepository.GetById(id);
            }
            return null;
        }
    }
}
