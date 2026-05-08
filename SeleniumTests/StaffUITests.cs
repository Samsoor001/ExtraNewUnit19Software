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
    public class StaffUITests : SeleniumTestBase
    {
        [Fact]
        public void UI_Test14_StaffUser_CanSeeStaffOnlyControls()
        {
            Login(StaffEmail, StaffPassword);
            UploadCsv();

            var flagBtn = wait.Until(d => d.FindElement(By.Id("flagSelectedBtn")));
            Assert.True(flagBtn.Displayed);

            var flaggedLink = driver.FindElements(By.LinkText("View Flagged Comics"));
            Assert.True(flaggedLink.Any());
        }

        [Fact]
        public void UI_Test16_SelectingComicRow_EnablesFlagButtonForStaff()
        {
            Login(StaffEmail, StaffPassword);
            UploadCsv();

            var flagBtn = wait.Until(d => d.FindElement(By.Id("flagSelectedBtn")));
            Assert.False(flagBtn.Enabled);

            var firstRow = wait.Until(d => d.FindElements(By.CssSelector(".comic-row")).First());
            firstRow.Click();

            wait.Until(d => d.FindElement(By.Id("flagSelectedBtn")).Enabled);

            Assert.True(driver.FindElement(By.Id("flagSelectedBtn")).Enabled);
        }
    }
}
