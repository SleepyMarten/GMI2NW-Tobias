using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.IO;

namespace Tobias.RestInterface.Test.WebRequests
{
    [TestClass]
    public class DonorWebRequestTests
    {
        #region Constants
        private const string restInterfaceURI = "https://localhost:44357/donor";
        #endregion

        #region Initialize and Cleanup methods
        [TestInitialize]
        public void TestInitialize()
        {
            Console.WriteLine("Remember that the server first needs to be started separately.");
        }
        #endregion

        #region Tests
        [TestMethod]
        public void PostDonor()
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(restInterfaceURI);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json; charset=utf-8";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{ \"guid\" : \"\", "
                + "\"firstName\" : \"Pelle\", "
                + "\"lastName\" : \"Päron\", "
                + "\"socialSecurityNumber\": \"1888\", "
                + "\"bloodGroupRh\": \"Rh+\", "
                + "\"bloodGroupAB0\": \"AB\" }";

                streamWriter.Write(json);
                streamWriter.Flush();
            }
            WebResponse response = httpWebRequest.GetResponse();

            //What shhould we assert here?
            Assert.IsTrue(true);
        }


        [TestMethod]
        public void GetDonorList()
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(restInterfaceURI);
            httpWebRequest.Method = "GET";
            //httpWebRequest.ContentType = "application/json; charset=utf-8";

            WebResponse response = httpWebRequest.GetResponse();

            Assert.IsTrue(true);
        }
        #endregion
    }
}
