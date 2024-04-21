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
        //Предусловия
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox","--disable-extensions"); 
        //options.AddArguments("--headless"); //Вынес отдельно,чтобы была возможность быстро скрыть браузер
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
        
        //Поиск элемента "Сообщества" и клик по нему
        var community = driver.FindElements(By.CssSelector("[data-tid='Community']")).First(element => element.Displayed);
        community.Click();
        //Проверка наличия заголовка "Сообщества"
        var communityTitle = driver.FindElement(By.CssSelector("[data-tid='Title']"));   
        Assert.That(communityTitle.Text == "Сообщества", "Заголовок должен быть Сообщества");
    }
    
    [Test] //Тест на поисковую строку по фамилии
    public void TestSearch()
    {
        //Поиск элемента поисковой строки и клик
        var search = driver.FindElement(By.CssSelector("[data-tid='SearchBar']"));
        search.Click();
        
        string lastName = "Лобашов";
        //После клика появляется плейсхолдер, вписываем фамилию
        var input = driver.FindElement(By.CssSelector("[placeholder='Поиск сотрудника, подразделения, сообщества, мероприятия']"));
        input.SendKeys(lastName);
        
        //Явное ожидание, пока прогрузится поисковая строка
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='ComboBoxMenu__item']")));
        
        //Поиск фамилии из выборки
        var namesearch = driver.FindElement(By.CssSelector("[data-tid='ScrollContainer__inner']")).Text;
        namesearch.Should().Contain(lastName);
    }

    [Test] //Тест на изменение дополнительного Email
    public void EditAdditionalEmail()
    {
        //Переход по урлу в настройки профиля и поиск поля доп.почты
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/profile/settings/edit");
        var additionalEmail = driver.FindElement(By.CssSelector("[data-tid='AdditionalEmail'] input"));
        
        //Стираю всё, что есть в поле и ввожу свою почту
        additionalEmail.SendKeys(Keys.Control + "a");
        additionalEmail.SendKeys(Keys.Backspace);
        additionalEmail.SendKeys("ffffaaa@mail.ru");
        
        //Ищу кнопку "Сохранить" и нажимаю на неё
        var saveButton = driver.FindElement(By.CssSelector("[data-tid='PageHeader'] button"));
        saveButton.Click();
        
        //Проверяю, что у меня получилось изменить почту
        var contactCard = driver.FindElement(By.CssSelector("[data-tid='ContactCard']"));
        contactCard.Text.Should().Contain("ffffaaa@mail.ru");
    }

    [Test] //Создание сообщества, проверка и удаление
    public void CreateCommunity()
    {
        //Переход по урлу в сообщества
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities");

        //Явное ожидание, чтобы можно было нажать на кнопку
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='PageHeader']")));
        
        //Ищу кнопку создать и кликаю по ней. Ищу поле с названием, вписываю текст. Метод и локатор выбраны специально, чтобы показать, что не только за data-tid могу зацепиться
        var newCommunity = driver.FindElement(By.XPath("//*[contains(text(),'СОЗДАТЬ')]"));
        newCommunity.Click();
        var createName = driver.FindElement(By.CssSelector("[placeholder='Название сообщества']"));
        createName.SendKeys("testcommunity");
        
        //Ищу кнопку "Создать" и кликаю по ней
        var button = driver.FindElement(By.CssSelector("[data-tid='CreateButton']"));
        button.Click();
        
        //Если сообщество создано, появится текст "Управление сообществом". Ищу его для проверки
        string namesearch = driver.FindElement(By.XPath("//*[contains(text(),'Управление сообществом')]")).Text;
        namesearch.Should().Contain("Управление сообществом");
        
        //Ищу кнопку удалить и нажимаю на неё
        var delbatton = driver.FindElement(By.CssSelector("[data-tid='DeleteButton']"));
        delbatton.Click();

        //В подтверждающем окне так же нахожу кнопку удаления и нажимаю её
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
        //Ищу кнопку боковой панели и кликаю на неё
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


