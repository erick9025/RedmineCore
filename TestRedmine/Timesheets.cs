using NUnit.Framework;
using RedmineCore.Pages;
using RedmineCore.Selenium;

namespace TestRedmine
{
    [TestFixture]
    [Category("RedmineErick")]
    public class Timesheets
    {
        #region TestInitialize
        [SetUp]
        public void Init()
        {
            Driver.Initialize();
        }
        #endregion TestInitialize

        ///Meeting
        ///Test Case Design 
        ///Testing
        ///Development
        ///Research (Non-Project Bucket)

        [Test]
        public void Automation1_Ayer()
        {
            RedminePage.LlenarHoras(1, 5, 1, 0, 1, "05/06/2021");
        }

        [Test]
        public void Automation3_Manana()
        {
            RedminePage.LlenarHoras(1, 5, 1, 0, 1, "05/10/2021");
        }

        [Test]
        public void Automation2_Hoy()
        {
            RedminePage.LlenarHoras(1, 5, 1, 0, 1);
        }

        #region CleanUp
        [TearDown]
        public void Cleanup()
        {
            Driver.Close();
        }
        #endregion CleanUp
    }
}
