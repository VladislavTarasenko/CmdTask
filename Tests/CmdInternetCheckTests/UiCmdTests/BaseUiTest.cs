using Common.Core.SeleniumWebDriver;

namespace Tests;

[TestFixture]
public class BaseUiTest
{
    [SetUp]
    public void SetUp()
    {
        DriverInstance.GetInstance();
    }

    [TearDown]
    public void TearDown()
    {
        DriverInstance.CloseBrowser();
    }
}
