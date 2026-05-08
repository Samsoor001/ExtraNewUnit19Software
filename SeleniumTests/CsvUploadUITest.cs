using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using OpenQA.Selenium;
using Xunit;
using Fantasy_EncyclopediaSelenium.Tests;

namespace SeleniumTests
{
    public class CsvUploadUITest : SeleniumTestBase
    {
        [Fact]
        public void UI_Test1_CsvUploadPage_ValidCsvFile_UploadsSuccessfully()
        {
            GoToUploadPage();

            var fileInput = wait.Until(d => d.FindElement(By.Name("csvFile")));
            fileInput.SendKeys(CsvPath);

            var uploadButton = driver.FindElement(By.XPath("//button[normalize-space()='Upload']"));
            uploadButton.Click();

            wait.Until(d =>
                d.FindElements(By.Name("TitleQuery")).Any() ||
                d.PageSource.Contains("Search / Filter"));

            Assert.True(driver.FindElements(By.Name("TitleQuery")).Any());
        }
    }
}
