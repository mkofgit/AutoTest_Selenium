using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TestProject1;


    public class Testchrome
    {
        [Test]
        public void Authorization()
        {
            var options = new ChromeOptions();
            options.AddArguments("--no-sandbox","--start-maximized","--disable-extensions");
            var driver = new ChromeDriver(options);
            
            driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");

            Thread.Sleep(3000);
            
            var login = driver.FindElement(By.Id("Username"));
            login.SendKeys("mkof@List.ru");
            
            var password = driver.FindElement(By.Name("Password"));
            password.SendKeys("Gang2020!!");
            
            Thread.Sleep(2000);

            var enter = driver.FindElement(By.Name("button"));
            enter.Click();
            
            
            var currentUrl = driver.Url;
            Assert.That(currentUrl =="https://staff-testing.testkontur.ru/news");
            
            driver.Quit();
        }
    }
