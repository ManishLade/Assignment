using Microsoft.Extensions.Options;
using Moq;

namespace Lookup.Service.Test;

public class LookupServiceTest
{
    private Mock<IOptions<AppSettings>> appSettings;
    private LookupService targetService;

    [SetUp]
    public void Setup()
    {
        appSettings = new Mock<IOptions<AppSettings>>();
        appSettings.Setup(x => x.Value).Returns(
            new AppSettings
            {
                LookupUrl = "http://production.shippingapis.com/ShippingAPI.dll?API=CityStateLookup",
                Username = "340HEXAW6349"
            }
        );
        targetService = new LookupService(appSettings.Object);
    }

    [TestCase("90210", "CA")]
    [TestCase("10008", "NY")]
    public void GetStateByZipCode_ValidZipCode_ReturnsState(string zipCode, string expectedState)
    {
        var result = targetService.GetStateByZipCode(zipCode);

        Assert.That(result.State, Is.EqualTo(expectedState));
    }

    [TestCase("99999", "Invalid Zip Code.")]
    [TestCase("00000", "Invalid Zip Code.")]
    public void GetStateByZipCode_InvalidZipCode_ReturnsErrorMessage(string zipCode, string expectedState)
    {
        var result = targetService.GetStateByZipCode(zipCode);

        Assert.That(result.ErrorMessage, Is.EqualTo(expectedState));
    }

    [TestCase("99999", "InternalServerError")]
    public void GetStateByZipCode_InvalidLookupUrl_ReturnsErrorMessage(string zipCode, string expectedState)
    {
        appSettings.Setup(x => x.Value).Returns(
            new AppSettings
            {
                LookupUrl = "http://xyz.com"
            }
        );
        targetService = new LookupService(appSettings.Object);

        var result = targetService.GetStateByZipCode(zipCode);

        Assert.That(result.StatusCode.ToString(), Is.EqualTo(expectedState));
    }


    [TestCase("", "Invalid Zip Code.")]
    [TestCase(null, "Invalid Zip Code.")]
    public void GetStateByZipCode_EmptyOrNullZipCode_ReturnsErrorMessage(string zipCode, string expectedState)
    {
        var result = targetService.GetStateByZipCode(zipCode);

        Assert.That(result.ErrorMessage, Is.EqualTo(expectedState));
    }

    [TestCase("902108", "ZIPCode must be 5 characters")]
    [TestCase("100088", "ZIPCode must be 5 characters")]
    [TestCase("1000", "ZIPCode must be 5 characters")]
    [TestCase("9999", "ZIPCode must be 5 characters")]
    public void GetStateByZipCode_ZipCodeMoreOrLessthan5Char_ReturnsErrorMessage(string zipCode, string expectedState)
    {
        var result = targetService.GetStateByZipCode(zipCode);
        Assert.That(result.ErrorMessage, Is.EqualTo(expectedState));
    }
}