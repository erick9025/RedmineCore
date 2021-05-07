using NUnit.Framework;
using OpenQA.Selenium;
using RedmineCore.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RedmineCore.Pages
{
    public class RedminePage : BasePage
    {
        //Configuration booleans
        static bool skipSpecialProject = false;
        static bool isDateOnWebAsyyyymmdd = false;

        public static void LlenarHoras(int hoursMeeting, int hoursDesign, int hoursTesting, int hoursDevelopment, int hoursOther, string myDate = null)
        {
            bool forceTo8 = false;
            int myiterator = 0;
            int sumHours = hoursMeeting + hoursDesign + hoursTesting + hoursDevelopment + hoursOther;
            IWebElement ddlActivity = null;
            OpenQA.Selenium.Support.UI.SelectElement selectElement;

            if (forceTo8) Assert.IsTrue(sumHours == 8, "Hours should be 8");

            List<int> listHours = new List<int>
            {
                hoursMeeting,
                hoursDesign,
                hoursTesting,
                hoursDevelopment,
                hoursOther
            };

            List<string> listComments = new List<string>
            {
                Constants.CommentsMeeting,
                Constants.CommentsDesign,
                Constants.CommentsTesting,
                Constants.CommentsDevelopment1, // <---- change this
                Constants.CommentsOther
            };

            List<string> listActivities = new List<string>
            {
                "Meeting",
                "Test Case Design",
                "Testing",
                "Development",
                "Research"
            };

            IrAPagina(Constants.BaseURL);
            IniciarSesion(Constants.Username, Constants.Password);
            IrAProyecto(Constants.TaskURLAutomation);

            foreach (var item in listHours)
            {
                //Si estamos en el non-project cambiamos de url
                if (myiterator == 4 && !skipSpecialProject)
                {
                    IrAProyecto(Constants.TaskURLNonProject, false);
                }

                if (myiterator == 4 && skipSpecialProject)
                {
                    break;
                }

                SeleccionarFecha(myDate);

                if (item > 0)
                {
                    //Meter horas
                    Driver.Instance.FindElement(By.Id("time_entry_hours")).SendKeys($"{item}"); //conversión de entero a string                                                                                                
                    Driver.Instance.FindElement(By.Id("time_entry_comments")).SendKeys(listComments[myiterator]); //Meter comentarios

                    //Seleccionar actividad del drop down list
                    ddlActivity = Driver.Instance.FindElement(By.Id("time_entry_activity_id"));
                    selectElement = new OpenQA.Selenium.Support.UI.SelectElement(ddlActivity);
                    selectElement.SelectByText(listActivities[myiterator]);

                    Driver.Instance.FindElement(By.Name("continue")).Click();//confirmar horas
                    Thread.Sleep(600);

                    Print(listActivities[myiterator] + "//" + $"{item}" + "//" + listComments[myiterator]);
                }
                myiterator++;
            }
        }

        private static void IrAPagina(string url)
        {
            Driver.Instance.Manage().Window.Maximize();
            Driver.Instance.Navigate().GoToUrl(url);
        }

        private static void IniciarSesion(string username, string password)
        {
            Driver.Instance.FindElement(By.Id("username")).SendKeys(username);
            Driver.Instance.FindElement(By.Id("password")).SendKeys(password);
            Console.WriteLine("Entered user and password");

            Driver.Instance.FindElement(By.Id("login-submit")).Click();
            Console.WriteLine("Login");
        }

        private static void IrAProyecto(string url, bool isIssueView = true)
        {
            Thread.Sleep(1000);
            Driver.Instance.Navigate().GoToUrl(url);
            Thread.Sleep(1000);

            if (isIssueView)
            {
                //Click link "Log time" upper right corner
                Driver.Instance.FindElement(By.XPath("//div[@id='content']//div[1]//a[2]")).Click();
            }
            else
            {
                //Click link "Log time" lower left corner below "Spent time" label
                Driver.Instance.FindElement(By.XPath("//h3[contains(@class,'icon-time')]/following::a[contains(text(),'Log time')]")).Click();
            }


            Thread.Sleep(1000);
        }

        private static void SeleccionarFecha(string myDate = null)
        {
            IWebElement boxDate = null;
            string boxDateString = string.Empty;
            string formattedDate = string.Empty;
            string wantedDay = string.Empty;
            string wantedMonth = string.Empty;
            string wantedYear = string.Empty;

            if (myDate != null) //se especificó algo
            {
                /*
                    Date on UI is not same as date wanted
                      String lengths are both 10. Strings differ at index 6.
                      Expected: “2021-02-03”
                      But was:  “2021-03-02"
                */

                if (isDateOnWebAsyyyymmdd) //Happening on Mac
                {
                    DateTime dateObject = DateTime.Parse(myDate);

                    Console.WriteLine("Fecha ya formateada disque: " + formattedDate);

                    wantedDay = dateObject.Day.ToString("00");
                    wantedMonth = dateObject.Month.ToString("00");
                    wantedYear = dateObject.Year.ToString();

                    formattedDate = wantedYear + "-" + wantedMonth + "-" + wantedDay;
                    Console.WriteLine("Fecha ya formateada a la malagueña: " + formattedDate);

                    //Seleccionar el día
                    boxDate = Driver.Instance.FindElement(By.Id("time_entry_spent_on"));

                    boxDate.SendKeys(wantedDay);
                    TeclazoDerecha(boxDate);
                    boxDate.SendKeys(wantedMonth);
                    TeclazoDerecha(boxDate);
                    boxDate.SendKeys(wantedYear);

                    //Validación día se introdujo correctamente
                    boxDateString = boxDate.GetAttribute("value");
                    Console.WriteLine(boxDateString);

                    //Validate that formatted date is correct
                    Assert.AreEqual(formattedDate, boxDateString, "Date on UI is not same as date wanted");
                }
                else //As regular on VDI (Windows)
                {
                    //myDate = mm/dd/yyyy
                    //position 0123456789
                    formattedDate = DateTime.Parse(myDate).ToString("yyyy-mm-dd");
                    Console.WriteLine("Fecha ya formateada disque: " + formattedDate);
                    wantedDay = myDate.Substring(3, 2);
                    wantedMonth = myDate.Substring(0, 2);
                    wantedYear = myDate.Substring(6, 4);
                    formattedDate = wantedYear + "-" + wantedMonth + "-" + wantedDay;
                    Console.WriteLine("Fecha ya formateada a la malagueña: " + formattedDate);

                    //Seleccionar el día
                    boxDate = Driver.Instance.FindElement(By.Id("time_entry_spent_on"));
                    //boxDate.SendKeys(myDate);
                    boxDate.SendKeys(wantedMonth);
                    Thread.Sleep(100);
                    boxDate.SendKeys(wantedDay);
                    Thread.Sleep(100);
                    boxDate.SendKeys(wantedYear);

                    //Validación día se introdujo correctamente
                    boxDateString = boxDate.GetAttribute("value");
                    Console.WriteLine(boxDateString);

                    //Validate that formatted date is correct
                    Assert.AreEqual(formattedDate, boxDateString, "Date on UI is not same as date wanted");
                }
            }
        }

        private static void TeclazoDerecha(IWebElement element)
        {
            Thread.Sleep(100);
            element.SendKeys(Keys.ArrowRight);
            Thread.Sleep(100);
        }
    }
}
