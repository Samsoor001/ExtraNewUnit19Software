using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Xunit;
using Fantasy_EncyclopediaSelenium.Tests;

namespace SeleniumTests
{
    public class SortingUITests : SeleniumTestBase
    {
        [Fact]
        public void UI_Test8_Sorting_TitleZa_DisplaysReverseAlphabeticalOrder()
        {
            UploadCsv();

            var firstOldRow = driver.FindElements(By.CssSelector(".comic-row")).FirstOrDefault();

            var sortSelect = GetSelectByName("SortTitle");
            sortSelect.SelectByValue("za");

            ApplyFilters();

            if (firstOldRow != null)
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(firstOldRow));
            }

            wait.Until(d => d.FindElements(By.CssSelector(".comic-row")).Any());

            var titles = GetTitlesFromRows();

            Assert.NotEmpty(titles);

            var comparer = System.Globalization.CultureInfo.CurrentCulture.CompareInfo;

            for (int i = 0; i < titles.Length - 1; i++)
            {
                var comparison = comparer.Compare(
                    titles[i],
                    titles[i + 1],
                    System.Globalization.CompareOptions.IgnoreCase);

                Assert.True(
                    comparison >= 0,
                    $"Titles are not in descending order at index {i}: '{titles[i]}' came before '{titles[i + 1]}'.");
            }
        }
    }
}
