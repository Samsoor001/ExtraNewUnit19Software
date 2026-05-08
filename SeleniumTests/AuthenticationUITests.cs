using Fantasy_EncyclopediaSelenium.Tests;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SeleniumTests
{
    public class AuthenticationUITests : SeleniumTestBase
    {
        [Fact]
        public void UI_Test11_LoggedOutUser_CannotSeeSaveControls()
        {
            UploadCsv();

            Assert.False(driver.FindElements(By.Id("saveSelectedBtn")).Any());

            var loginLink = driver.FindElements(By.LinkText("Log in"));
            Assert.True(loginLink.Any());
        }

        [Fact]
        public void UI_Test12_LoginPage_ValidUser_CanLogIn()
        {
            Login(NormalUserEmail, NormalUserPassword);

            Assert.DoesNotContain("/Identity/Account/Login", driver.Url, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void UI_Test13_LoggedInUser_CanSeeSaveButton()
        {
            Login(NormalUserEmail, NormalUserPassword);
            UploadCsv();

            var saveBtn = wait.Until(d => d.FindElement(By.Id("saveSelectedBtn")));
            Assert.True(saveBtn.Displayed);
        }

        [Fact]
        public void UI_Test15_SelectingComicRow_EnablesSaveButton()
        {
            Login(NormalUserEmail, NormalUserPassword);
            UploadCsv();

            var saveBtn = wait.Until(d => d.FindElement(By.Id("saveSelectedBtn")));
            Assert.False(saveBtn.Enabled);

            var firstRow = wait.Until(d => d.FindElements(By.CssSelector(".comic-row")).First());
            firstRow.Click();

            wait.Until(d => d.FindElement(By.Id("saveSelectedBtn")).Enabled);

            Assert.True(driver.FindElement(By.Id("saveSelectedBtn")).Enabled);
        }
    }
}
