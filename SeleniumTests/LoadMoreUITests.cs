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
    public class LoadMoreUITests : SeleniumTestBase
    {
        [Fact]
        public void UI_Test10_LoadMore_AppendsAdditionalRows()
        {
            UploadCsv();

            wait.Until(d => d.FindElements(By.CssSelector(".comic-row")).Any());
            var beforeCount = GetComicRows().Length;

            var loadMoreBtn = wait.Until(d => d.FindElement(By.Id("loadMoreBtn")));
            loadMoreBtn.Click();

            wait.Until(d => d.FindElements(By.CssSelector(".comic-row")).Count > beforeCount);

            var afterCount = GetComicRows().Length;
            Assert.True(afterCount > beforeCount);
        }
    }
}
