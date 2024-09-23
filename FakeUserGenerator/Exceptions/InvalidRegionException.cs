namespace FakeUserGenerator.Exceptions;

public class InvalidRegionException(string region) : Exception($"{region} - incorrect region")
{
}
