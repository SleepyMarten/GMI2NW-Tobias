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
    public class RecipientController : ControllerBase
    {
        private readonly ILogger<RecipientController> m_logger;
        private readonly IConfiguration m_configuration;
        private readonly DbAccess m_dbAccess;

        public RecipientController(IConfiguration configuration, ILogger<RecipientController> logger)
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
            }
            catch (Exception e) { 
                throw new ArgumentException("Configuration key \"ConnectionString\" is missing or invalid.", nameof(configuration), e);
            }
        }

        [HttpGet]
        [Route("[controller]")]
        public JsonResult GetRecipient(string guid = null)
        {
            JsonResult returnValue;

            if(guid is null)
            {
                IEnumerable<Recipient> recipients = m_dbAccess.GetRecipientList();
                returnValue = new JsonResult(recipients);
            }
            else
            {
                Guid guidAsGuid;
                if (Guid.TryParse(guid, out guidAsGuid))
                {
                    Recipient recipient = m_dbAccess.GetRecipient(guidAsGuid);
                    RecipientTransporter transporter = RecipientTransporter.FromRecipient(recipient);
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
        public JsonResult Post([FromBody] RecipientTransporter transporter)
        {
            JsonResult returnValue;

            Recipient recipient = RecipientTransporter.ToRecipient(transporter);

            if (m_dbAccess.RecipientExists(recipient.Guid))
            {
                recipient = m_dbAccess.UpdateRecipient(recipient);
            }
            else
            {
                recipient = m_dbAccess.CreateRecipient(recipient);
            }

            transporter = RecipientTransporter.FromRecipient(recipient);
            returnValue = new JsonResult(transporter);
            return returnValue;
        }

        [HttpDelete]
        [Route("[controller]")]
        public JsonResult Delete(Guid guid)
        {
            JsonResult returnValue;

            m_dbAccess.DeleteRecipient(guid);

            returnValue = new JsonResult("Deleted Successfully");

            return returnValue;
        }
    }
}
