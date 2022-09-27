using System;
using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using UrlShortner.Infrastructure;
using Assert = NUnit.Framework.Assert;

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

    public TestLongToShortURL()
    {
    }

    [TestCase]
    public void TestInvalidShortUrl()
    {
        //Arrange
        int id = 5;

        //Act
        var result = targetService.IdToShortUrl(id);

        //Assert
        Assert.AreEqual("ZZZZZf", result);
    }

    [TestCase]
    public void TestInvalidShortUrlLength()
    {
        //Arrange
        string shorturl = "ZZZZZf";

        //Act
        var result = targetService.ShortUrlToId(shorturl);
        
        //Assert
        Assert.True(result == 5);
    }
}