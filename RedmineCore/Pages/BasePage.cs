using RedmineCore.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedmineCore.Pages
{
    public abstract class BasePage
    {
        public BasePage()
        {
            PageFactory.InitElements(Driver.Instance, this);
        }

        public static void Print(string outputStr)
        {
            System.Console.WriteLine(outputStr);
        }

        public static void PrintOnFrame(string outputStr, string header = null)
        {
            System.Console.WriteLine("\n*********************************************  " + header + "  *********************************************"
                + "\n" + outputStr
                + "\n" + "*********************************************************************************************************************");
        }
    }
}
