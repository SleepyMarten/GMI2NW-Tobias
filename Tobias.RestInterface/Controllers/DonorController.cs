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

namespace Tobias.RestInterface.Controllers
{
    [ApiController]
    [EnableCors("AllowAnyOriginMethodHeader")]
    public class DonorController : ControllerBase
    {
        private readonly ILogger<DonorController> m_logger;
        private readonly IConfiguration m_configuration;
        private readonly DbAccess m_dbAccess;

        public DonorController(IConfiguration configuration, ILogger<DonorController> logger)
        {
            m_logger = logger;
            m_configuration = configuration;
            string connectionString = String.Empty;

            try
            {
                connectionString = m_configuration.GetValue<string>("ConnectionString");
            } 
            catch (Exception e) 
            { 
                throw new ArgumentException("Configuration key \"ConnectionString\" is missing or invalid.", nameof(configuration), e);
            }

            //Replace 'variables' with actual contents
            string curDir = Path.Combine(Directory.GetCurrentDirectory(), "bin\\Debug\\net7.0");
            connectionString = connectionString.Replace("$(CurDir)", curDir);

            m_dbAccess = new DbAccess(connectionString);
        }

        [HttpGet]
        [Route("[controller]")]
        public JsonResult GetDonor(string guid = null)
        {
            JsonResult returnValue;

            if(guid is null)
            {
                IEnumerable<Donor> donors = m_dbAccess.GetDonorList();
                returnValue = new JsonResult(donors);
            }
            else
            {
                Guid guidAsGuid;
                if (Guid.TryParse(guid, out guidAsGuid))
                {
                    Donor donor = m_dbAccess.GetDonor(guidAsGuid);
                    DonorTransporter transporter = DonorTransporter.FromDonor(donor);
                    returnValue = new JsonResult(transporter);
                }
                else
                {
                    string errorMessage = String.Format("Parameter '{0}' has invalid format, should be a valid GUID string. Parameter is now '{1}'.", nameof(guid), guid);
                    throw new ArgumentException(errorMessage);
                }
            }
            return returnValue;
        }

        [HttpPost]
        [Route("[controller]")]
        public JsonResult Post([FromBody] DonorTransporter transporter)
        {
            JsonResult returnValue;

            Donor donor = DonorTransporter.ToDonor(transporter);

            if (m_dbAccess.DonorExists(donor.Guid))
            {
                donor = m_dbAccess.UpdateDonor(donor);
            }
            else
            {
                donor = m_dbAccess.CreateDonor(donor);
            }

            transporter = DonorTransporter.FromDonor(donor);
            returnValue = new JsonResult(transporter);
            return returnValue;
        }

        [HttpDelete]
        [Route("[controller]")]
        public JsonResult Delete(Guid guid)
        {
            JsonResult returnValue;

            m_dbAccess.DeleteDonor(guid);

            returnValue = new JsonResult("Deleted Successfully");

            return returnValue;
        }
    }
}
