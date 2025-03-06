using Eurocentric.AdminApi.V1.Countries.CreateCountry;
using Eurocentric.AdminApi.V1.Models;

namespace Eurocentric.AdminApi.Tests.Utils.V1.Assertions;

public static class CountryExtensions
{
    private static bool EqualValues(Country first, Country second) => first.Id == second.Id
                                                                      && first.CountryCode == second.CountryCode
                                                                      && first.CountryName == second.CountryName
                                                                      && first.CountryType == second.CountryType
                                                                      && first.ContestIds.SequenceEqual(second.ContestIds);

    public static void ShouldBeCorrectlyCreatedFrom(this Country subject, CreateCountryCommand command)
    {
        Assert.Equal(command.CountryCode, subject.CountryCode);
        Assert.Equal(command.CountryName, subject.CountryName);
        Assert.Equal(command.CountryType, subject.CountryType);
        Assert.Empty(subject.ContestIds);
    }

    public static void ShouldBe(this Country subject, Country expected) => Assert.Equal(expected, subject, EqualValues);

    public static void ShouldBe(this IEnumerable<Country> subject, IEnumerable<Country> expected) =>
        Assert.Equal(expected, subject, EqualValues);
}
