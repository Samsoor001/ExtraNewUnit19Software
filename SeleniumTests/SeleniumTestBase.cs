using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Fantasy_EncyclopediaSelenium.Tests
{
    public abstract class SeleniumTestBase : IDisposable
    {
        protected readonly IWebDriver driver;
        protected readonly WebDriverWait wait;

        protected const string BaseUrl = "http://localhost:5034";
        protected const string UploadPage = BaseUrl + "/CsvImporter/Index";
        protected const string LoginPage = BaseUrl + "/Identity/Account/Login";

        protected const string CsvPath = @"C:\Users\Sam\Downloads\names.csv";
        protected const string NormalUserEmail = "Noctis00011@outlook.com";
        protected const string NormalUserPassword = "4zaW:vkPNLCD:WB";
        protected const string StaffEmail = "staff2@fbz.com";
        protected const string StaffPassword = "FbZStaff123!";

        protected SeleniumTestBase()
        {
            var options = new ChromeOptions();
            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        protected void GoToUploadPage()
        {
            driver.Navigate().GoToUrl(UploadPage);
        }

        protected void GoToLoginPage()
        {
            driver.Navigate().GoToUrl(LoginPage);
        }

        protected void UploadCsv()
        {
            GoToUploadPage();

            var fileInput = wait.Until(d => d.FindElement(By.Name("csvFile")));
            fileInput.SendKeys(CsvPath);

            driver.FindElement(By.XPath("//button[normalize-space()='Upload']")).Click();

            wait.Until(d =>
                d.PageSource.Contains("Search / Filter") ||
                d.PageSource.Contains("No results found.") ||
                d.FindElements(By.Name("TitleQuery")).Any());
        }

        protected void Login(string email, string password)
        {
            GoToLoginPage();

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Input_Email"))).SendKeys(email);
            driver.FindElement(By.Id("Input_Password")).SendKeys(password);
            driver.FindElement(By.Id("login-submit")).Click();

            wait.Until(d => !d.Url.Contains("/Identity/Account/Login"));
        }

        protected void ApplyFilters()
        {
            driver.FindElement(By.XPath("//button[normalize-space()='Apply']")).Click();
        }

        protected SelectElement GetSelectByName(string name)
        {
            return new SelectElement(wait.Until(d => d.FindElement(By.Name(name))));
        }

        protected IWebElement[] GetComicRows()
        {
            return driver.FindElements(By.CssSelector(".comic-row")).ToArray();
        }

        protected string[] GetTitlesFromRows()
        {
            var rows = GetComicRows();
            return rows.Select(r => r.FindElements(By.TagName("td"))[0].Text.Trim()).ToArray();
        }

        protected string[] GetAuthorsFromRows()
        {
            var rows = GetComicRows();
            return rows.Select(r => r.FindElements(By.TagName("td"))[2].Text.Trim()).ToArray();
        }

        protected string[] GetGenresFromRows()
        {
            var rows = GetComicRows();
            return rows.Select(r => r.FindElements(By.TagName("td"))[1].Text.Trim()).ToArray();
        }

        protected string[] GetDatesFromRows()
        {
            var rows = GetComicRows();
            return rows.Select(r => r.FindElements(By.TagName("td"))[5].Text.Trim()).ToArray();
        }

        protected string[] GetLanguagesFromRows()
        {
            var rows = GetComicRows();
            return rows.Select(r => r.FindElements(By.TagName("td"))[6].Text.Trim()).ToArray();
        }

        protected string[] GetNameTypesFromRows()
        {
            var rows = GetComicRows();
            return rows.Select(r => r.FindElements(By.TagName("td"))[7].Text.Trim()).ToArray();
        }

        public void Dispose()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}