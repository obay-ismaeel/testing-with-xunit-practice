using UnitTesting.Projects.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTesting.Projects.Tests;

public class IssueTests
{
    // Constructor Tests
    [Fact]
    public void Constructor_IssueWithDescIsEmpty_ThrowsInvalidIssueDescriptionException()
    {
        Action sut = () => new Issue("", Priority.High, Category.Hardware, DateTime.Now);

        Assert.Throws<InvalidIssueDescriptionException>(() => sut());
    }

    [Fact]
    public void Constructor_IssueWithDescIsNull_ThrowsInvalidIssueDescriptionException()
    {
        Action sut = () => new Issue(null, Priority.High, Category.Hardware, DateTime.Now);

        Assert.Throws<InvalidIssueDescriptionException>(() => sut());
    }

    [Fact]
    public void Constructor_IssueNotProvidingCreatedAt_ReturnsCreatedAtAsCurrentTime()
    {
        var sut = new Issue("1# Issue", Priority.High, Category.Hardware);

        var actual = sut.CreatedAt;

        Assert.False(actual == default);
    }

    // GenerateKey() tests
    [Fact]
    public void GenerateKey_WithIssueValidProperties_Returns18CharsKey()
    {
        var sut = new Issue("Issue #1", Priority.High, Category.Hardware, new DateTime(2022, 10, 30, 1, 1, 1)); 

        MethodInfo methodInfo = typeof(Issue).GetMethod("GenerateKey", BindingFlags.Instance | BindingFlags.NonPublic);

        var actual = methodInfo.Invoke(sut, null).ToString();

        var expected = "HW-2022-H-1234ABCD";

        Assert.NotNull(actual);
        Assert.Equal(expected.Length, actual.Length);
    }

    [Theory]
    [InlineData("Issue #1", Priority.Urgent, Category.Hardware, "HW")]
    [InlineData("Issue #1", Priority.Urgent, Category.Software, "SW")]
    [InlineData("Issue #1", Priority.Urgent, Category.UnKnown, "NA")]
    public void GenerateKey_IssueWithCategory_ReturnsFirstSegmentMatchCategory(string description, Priority priority, Category category, string expected)
    {
        var sut = new Issue(description, priority, category, new DateTime(2022, 10, 30, 1, 1, 1));

        MethodInfo methodInfo = typeof(Issue).GetMethod("GenerateKey", BindingFlags.Instance | BindingFlags.NonPublic);

        var actual = methodInfo.Invoke(sut, null).ToString();

        Assert.NotNull(actual);
        Assert.Equal(expected, actual.Split("-")[0]);
    }

    [Theory]
    [InlineData("Issue #1", Priority.Urgent, Category.Hardware, "U")]
    [InlineData("Issue #1", Priority.Medium, Category.Software, "M")]
    [InlineData("Issue #1", Priority.High, Category.UnKnown, "H")]
    [InlineData("Issue #1", Priority.Low, Category.UnKnown, "L")]
    public void GenerateKey_IssueWithPriority_ReturnsThirdSegmentMatchPriority(string description, Priority priority, Category category, string expected)
    {
        var sut = new Issue(description, priority, category, new DateTime(2022, 10, 30, 1, 1, 1));

        MethodInfo methodInfo = typeof(Issue).GetMethod("GenerateKey", BindingFlags.Instance | BindingFlags.NonPublic);

        var actual = methodInfo.Invoke(sut, null).ToString();

        Assert.NotNull(actual);
        Assert.Equal(expected, actual.Split("-")[2]);
    }

    [Fact]
    public void GenerateKey_WithIssueCreatedAt_ReturnIssueKeySecondSegmentYYYY()
    {
        var sut = new Issue("Issue #2", Priority.High, Category.Hardware, new DateTime(2022, 10, 30, 1, 1, 1));

        MethodInfo methodInfo = typeof(Issue).GetMethod("GenerateKey", BindingFlags.Instance | BindingFlags.NonPublic);

        var actual = methodInfo.Invoke(sut, null).ToString();

        var expected = "2022";

        Assert.Equal(expected, actual.Split("-")[1]);
    }

    [Fact]
    public void GenerateKey_WithIssueValidProperies_ReturnIssueKeyFourthSegment8AlphaNumeric()
    {
        var sut = new Issue("Issue #1", Priority.Low, Category.Hardware,
            new DateTime(2022, 10, 11, 12, 30, 00));

        MethodInfo methodInfo = typeof(Issue).GetMethod("GenerateKey", BindingFlags.NonPublic | BindingFlags.Instance);

        var fourthSegment = methodInfo.Invoke(sut, null).ToString().Split("-")[3];
        var isAlphaNumeric = fourthSegment.All(x => char.IsLetterOrDigit(x));

        Assert.True(isAlphaNumeric);
    }

    [Theory]
    [InlineData("Issue #1", Priority.Urgent, Category.Hardware, "2001-8-12", "HW-2001-U-1234ABCD")]
    [InlineData("Issue #1", Priority.Urgent, Category.Software, "2000-10-10", "SW-2000-U-ABCD1234")]
    [InlineData("issue #1", Priority.Low, Category.Software, "2022-10-10", "SW-2022-L-ABCD1234")]
    [InlineData("issue #1", Priority.Low, Category.UnKnown, "2018-10-10", "NA-2018-L-ABCD1234")]
    [InlineData("issue #1", Priority.Low, Category.Hardware, "1992-10-10", "HW-1992-L-ABCD1234")]
    [InlineData("issue #1", Priority.Medium, Category.Hardware, "2003-10-10", "HW-2003-M-ABCD1234")]
    [InlineData("issue #1", Priority.High, Category.Hardware, "2015-10-10", "HW-2015-H-ABCD1234")]
    [InlineData("issue #1", Priority.Urgent, Category.Hardware, "1980-10-10", "HW-1980-U-ABCD1234")]
    public void GenerateKey_WithValidIssueProperties_ReturnsExpectedKey(string description, Priority priority, Category category, string date, string expected)
    {
        var sut = new Issue(description, priority, category, DateTime.Parse(date));

        MethodInfo methodInfo = typeof(Issue).GetMethod("GenerateKey", BindingFlags.Instance | BindingFlags.NonPublic);

        var actual = methodInfo.Invoke(sut, null).ToString();

        Assert.NotNull(actual);
        Assert.Equal(expected.Substring(0, 9), actual.Substring(0, 9));
    }
}
