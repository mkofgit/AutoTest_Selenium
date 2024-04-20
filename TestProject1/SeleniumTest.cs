using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace SeleniumTestForPractics_Lobashov;

public class SeleniumTest
{
    public ChromeDriver driver;

    [SetUp]
    public void SetUp()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox","--disable-extensions"); 
        options.AddArguments("--headless"); //Вынес отдельно,чтобы была возможность быстро скрыть браузер
        driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        Authorization();
    }

    [Test] //Тест на авторизацию с валидными логином и паролем
    public void Authorization1()
    {
        driver.Url.Should().Be("https://staff-testing.testkontur.ru/news");
    }
    
    [Test] //Проверка наличия сообщества на странице, проверка урла
    public void NavigationTest()
    {
        SidebarMenu();
        
        var community = driver.FindElements(By.CssSelector("[data-tid='Community']")).First(element => element.Displayed);
        community.Click();
        
        var communityTitle = driver.FindElement(By.CssSelector("[data-tid='Title']"));   
        Assert.That(communityTitle.Text == "Сообщества", "Заголовок должен быть Сообщества");
    }
    
    [Test] //Тест на поисковую строку по фамилии
    public void TestSearch()
    {
        var search = driver.FindElement(By.CssSelector("[data-tid='SearchBar']"));
        search.Click();
        
        string lastName = "Лобашов";
        
        var input = driver.FindElement(By.CssSelector("[placeholder='Поиск сотрудника, подразделения, сообщества, мероприятия']"));
        input.SendKeys(lastName);
        
        var namesearch = driver.FindElement(By.XPath("//*[contains(text(),'Лобашов')]")).Text;
        namesearch.Should().Contain(lastName);
    }

    [Test] //Тест на изменение дополнительного Email
    public void EditAdditionalEmail()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/profile/settings/edit");
        
        var additionalEmail = driver.FindElement(By.CssSelector("[data-tid='AdditionalEmail'] input"));
        
        additionalEmail.SendKeys(Keys.Control + "a");
        additionalEmail.SendKeys(Keys.Backspace);
        additionalEmail.SendKeys("ffffaaa@mail.ru");
        
        var saveButton = driver.FindElement(By.CssSelector("[data-tid='PageHeader'] button"));
        saveButton.Click();
        
        var contactCard = driver.FindElement(By.CssSelector("[data-tid='ContactCard']"));
        contactCard.Text.Should().Contain("fffffaa@mail.ru");
    }

    [Test] //Создание сообщества, проверка и удаление
    public void CreateCommunity()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities");

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='PageHeader']")));

        var newCommunity = driver.FindElement(By.XPath("//*[contains(text(),'СОЗДАТЬ')]"));
        newCommunity.Click();

        var createName = driver.FindElement(By.CssSelector("[placeholder='Название сообщества']"));
        createName.SendKeys("testcommunity");

        var button = driver.FindElement(By.CssSelector("[data-tid='CreateButton']"));
        button.Click();
        
        string namesearch = driver.FindElement(By.XPath("//*[contains(text(),'Управление сообществом')]")).Text;
        namesearch.Should().Contain("Управление сообществом");
        
        var delbatton = driver.FindElement(By.CssSelector("[data-tid='DeleteButton']"));
        delbatton.Click();

        var delclick = driver.FindElement(By.CssSelector("[data-tid='ModalPageFooter'] button"));
        delclick.Click();
    }

    public void Authorization()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        var login = driver.FindElement(By.Id("Username")); 
        login.SendKeys("mkof@list.ru");
        var password = driver.FindElement(By.Name("Password")); 
        password.SendKeys("Gang2020!!");
        var enter = driver.FindElement(By.Name("button")); 
        enter.Click();
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/news"));
    }
    public void SidebarMenu()
    {
        var sidebarMenu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        sidebarMenu.Click();
    }
    
        [TearDown] 
    public void TearDown() 
    { 
        driver.Close();
        driver.Quit(); 
    } 
}


