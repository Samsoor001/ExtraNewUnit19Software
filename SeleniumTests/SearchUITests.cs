using Fantasy_EncyclopediaSelenium.Tests;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumTests
{
    public class SearchUITests : SeleniumTestBase
    {
        [Fact]
        public void UI_Test2_TitleSearch_ReturnsMatchingResults()
        {
            UploadCsv();

            var titleBox = wait.Until(d => d.FindElement(By.Name("TitleQuery")));
            titleBox.Clear();
            titleBox.SendKeys("Secret invasion");

            ApplyFilters();

            wait.Until(d => d.FindElements(By.CssSelector(".comic-row")).Any());

            var titles = GetTitlesFromRows();
            Assert.All(titles, t => Assert.Contains("Secret invasion", t, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void UI_Test3_AuthorSearch_ReturnsRelevantResults()
        {
            UploadCsv();

            var beforeCount = GetComicRows().Length;
            var firstOldRow = driver.FindElements(By.CssSelector(".comic-row")).FirstOrDefault();

            var authorBox = wait.Until(d => d.FindElement(By.Name("AuthorQuery")));
            authorBox.Clear();
            authorBox.SendKeys("Andrews");

            ApplyFilters();

            if (firstOldRow != null)
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(firstOldRow));
            }

            wait.Until(d =>
                d.Url.Contains("AuthorQuery=Andrews") &&
                d.FindElements(By.CssSelector(".comic-row")).Any());

            var authors = GetAuthorsFromRows();
            var afterCount = authors.Length;

            Assert.NotEmpty(authors);
            Assert.True(afterCount < beforeCount);
            Assert.Contains(authors, a => a.Contains("Andrews", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void UI_Test17_NoResultsSearch_ShowsNoResultsMessage()
        {
            UploadCsv();

            var titleBox = wait.Until(d => d.FindElement(By.Name("TitleQuery")));
            titleBox.Clear();
            titleBox.SendKeys("zzzzzzzzz");

            ApplyFilters();

            wait.Until(d => d.PageSource.Contains("No results found."));
            Assert.Contains("No results found.", driver.PageSource);
        }
    }
}
