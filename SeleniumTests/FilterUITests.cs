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
    public class FilterUITests : SeleniumTestBase
    {
        [Fact]
        public void UI_Test4_GenreFilter_ReturnsOnlySelectedGenre()
        {
            UploadCsv();

            var genreSelect = GetSelectByName("SelectedGenre");
            genreSelect.SelectByText("Fantasy");

            ApplyFilters();

            wait.Until(d => d.FindElements(By.CssSelector(".comic-row")).Any());

            var genres = GetGenresFromRows();
            Assert.All(genres, g => Assert.Equal("Fantasy", g));
        }

        [Fact]
        public void UI_Test5_PublicationYearFilter_ReturnsOnlySelectedYear()
        {
            UploadCsv();

            var yearSelect = GetSelectByName("SelectedYear");
            yearSelect.SelectByText("2020");

            ApplyFilters();

            wait.Until(d => d.FindElements(By.CssSelector(".comic-row")).Any());

            var dates = GetDatesFromRows();
            Assert.All(dates, d => Assert.Contains("2020", d));
        }

        [Fact]
        public void UI_Test6_LanguageFilter_ReturnsOnlySelectedLanguage()
        {
            UploadCsv();

            var languageSelect = GetSelectByName("SelectedLanguage");
            languageSelect.SelectByText("English");

            ApplyFilters();

            wait.Until(d => d.FindElements(By.CssSelector(".comic-row")).Any());

            var languages = GetLanguagesFromRows();
            Assert.All(languages, l => Assert.Contains("English", l, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void UI_Test7_NameTypeFilter_ReturnsOnlySelectedNameType()
        {
            UploadCsv();

            var firstOldRow = driver.FindElements(By.CssSelector(".comic-row")).FirstOrDefault();

            var nameTypeSelect = GetSelectByName("SelectedNameType");
            var targetOption = nameTypeSelect.Options.First(o => o.Text != "All");
            targetOption.Click();

            var selectedText = nameTypeSelect.SelectedOption.Text;

            ApplyFilters();

            if (firstOldRow != null)
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(firstOldRow));
            }

            wait.Until(d => d.FindElements(By.CssSelector(".comic-row")).Any());

            var nameTypes = GetNameTypesFromRows();

            Assert.NotEmpty(nameTypes);
            Assert.All(nameTypes, n => Assert.Equal(selectedText, n, ignoreCase: true));
        }

        [Fact]
        public void UI_Test9_GroupResults_ByYear_DisplaysYearHeadings()
        {
            UploadCsv();

            var groupSelect = GetSelectByName("GroupBy");
            groupSelect.SelectByValue("year");

            ApplyFilters();

            wait.Until(d => d.FindElements(By.TagName("h3")).Any());

            var headings = driver.FindElements(By.TagName("h3"))
                .Select(h => h.Text.Trim())
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .ToArray();

            Assert.NotEmpty(headings);
        }
    }
}
