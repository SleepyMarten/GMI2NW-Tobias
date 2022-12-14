using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Tobias.Core;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Tobias.RestInterface.Transporters;
using System.IO;
using Tobias.Core.Compatibility;

namespace Tobias.RestInterface.Controllers
{
    [ApiController]
    [EnableCors("AllowAnyOriginMethodHeader")]
    public class CompatibilityController : ControllerBase
    {
        private readonly ILogger<DonorController> m_logger;
        private readonly IConfiguration m_configuration;
        private readonly DbAccess m_dbAccess;


        public CompatibilityController(IConfiguration configuration, ILogger<DonorController> logger)
        {
            m_logger = logger;
            m_configuration = configuration;
                      
            try
            {
                var connectionString = m_configuration.GetValue<string>("ConnectionString");
                
                //Replace 'variables' with actual contents
                string curDir = Path.Combine(Directory.GetCurrentDirectory(),
                    m_configuration.GetValue<string>("DotNetPathVersion"));
                connectionString = connectionString.Replace("$(CurDir)", curDir);

                m_dbAccess = new DbAccess(connectionString);
            } catch (Exception e) { 
                throw new ArgumentException("Configuration key \"ConnectionString\" is missing or invalid.", nameof(configuration), e);
            }
        }

        [HttpGet]
        [Route("[controller]")]
        public JsonResult GetCompatibility(string donorGuid = "", string recipientGuid = "")
        {
            JsonResult returnValue;

            Donor donor = m_dbAccess.GetDonor(Guid.Parse(donorGuid));
            Recipient recipient = m_dbAccess.GetRecipient(Guid.Parse(recipientGuid));

            CompatibilityResult compatibilityResult = CompatibilityCalculator.ComputeCompatibilityScore(donor, recipient);

            returnValue = new JsonResult(compatibilityResult);

            return returnValue;
        }
    }
}
