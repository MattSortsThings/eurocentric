namespace Eurocentric.Domain.Rules;

public static class DomainConstants
{
    public static class CountryCode
    {
        public const int RequiredLengthInChars = 2;
    }

    public static class CountryName
    {
        public const int MaxPermittedLengthInChars = 200;
    }
}
