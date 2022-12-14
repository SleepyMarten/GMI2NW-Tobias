using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Tobias.RestInterface.Controllers;

namespace Tobias.RestInterface.Test.Controllers
{
    public class DonorControllerTests
    {
        [TestMethod]
        public void GetListOfDonors()
        {
            DonorController donorController = new DonorController(null, null);

            var jsonResult = donorController.GetDonor();
        }

        public JsonResult PostDonorWithGuid()
        {
            DonorController donorController = new DonorController(null, null);

            return donorController.GetDonor();
        }

        public void PostDonorWithoutGuid()
        {
            DonorController donorController = new DonorController(null, null);

            var jsonResult = donorController.GetDonor();
        }

        public static void DeleteDonor()
        {
            DonorController donorController = new DonorController(null, null);

            var jsonResult = donorController.GetDonor();
        }
    }
}
