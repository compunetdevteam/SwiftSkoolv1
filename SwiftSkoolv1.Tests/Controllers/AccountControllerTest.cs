using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SwiftSkoolv1.WebUI.Controllers;
using System.Web;

namespace SwiftSkoolv1.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {

        public void TestThatStudentsAreCreatedWhenUploadingFromExcel()
        {
            //Arrange
            var acctcontroller = new AccountController();
            var uploadedfile = Substitute.For<HttpPostedFileBase>();

            uploadedfile.ContentType.Returns("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            uploadedfile.ContentLength.Returns(47477);
            uploadedfile.FileName.Returns("gcmStudent.xlsx");

        }
    }
}
