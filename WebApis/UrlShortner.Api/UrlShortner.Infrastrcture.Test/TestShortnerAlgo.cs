using Moq;
using UrlShortner.Infrastructure;

namespace UrlShortner.Infrastrcture.Test;

public class TestLongToShortURL
{
    private Mock<ILongUrlContext> context;
    private UrlShortnerService targetService;

    [SetUp]
    public void Setup()
    {
        context = new Mock<ILongUrlContext>();
        targetService = new UrlShortnerService(context.Object);
    }

    [TestCase]
    public void TestInvalidShortUrl()
    {
        //Arrange
        var id = 5;

        //Act
        var result = targetService.IdToShortUrl(id);

        //Assert
        Assert.AreEqual("ZZZZZf", result);
    }

    [TestCase]
    public void TestInvalidShortUrlLength()
    {
        //Arrange
        var shorturl = "ZZZZZf";

        //Act
        var result = targetService.ShortUrlToId(shorturl);

        //Assert
        Assert.True(result == 5);
    }
}