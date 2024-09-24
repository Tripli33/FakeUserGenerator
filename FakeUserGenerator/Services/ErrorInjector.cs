using FakeUserGenerator.Exceptions;
using FakeUserGenerator.Models;
using FakeUserGenerator.Utilities.Settings;
using Microsoft.Extensions.Options;

namespace FakeUserGenerator.Services;

public class ErrorInjector(IOptions<RegionOptions> regionOptions) : IErrorInjector
{
    private readonly Random _random = new();
    private readonly RegionOptions _regionOptions = regionOptions.Value;

    public IEnumerable<User> InjectErrors(IEnumerable<User> users, double errorCount, string region)
    {
        if (errorCount < 0)
        {
            return users;
        }

        var totalErrors = (int)Math.Floor(errorCount);
        var fractionalError = errorCount - totalErrors;

        if (_random.NextDouble() < fractionalError)
        {
            totalErrors++;
        }

        for (int i = 0; i < totalErrors; i++)
        {
            foreach (var user in users)
            {
                var action = _random.Next(3);

                switch (action)
                {
                    case 0:
                        user.FullName = ApplyRandomError(user.FullName!, region);
                        break;
                    case 1:
                        user.Address = ApplyRandomError(user.Address!, region);
                        break;
                    default:
                        user.PhoneNumber = ApplyRandomError(user.PhoneNumber!, region);
                        break;
                }
            }
        }

        return users;
    }

    private string ApplyRandomError(string field, string region)
    {
        if (string.IsNullOrEmpty(field)) 
        {
            return field;
        } 

        var action = _random.Next(3);

        return action switch
        {
            0 => RemoveRandomCharacter(field),
            1 => AddRandomCharacter(field, region),
            _ => SwapAdjacentCharacters(field)
        };
    }

    private string RemoveRandomCharacter(string field)
    {
        if (field.Length == 1)
        {
            return field;
        }

        var randomPos = _random.Next(field.Length);

        return field.Remove(randomPos, 1);
    }

    private string SwapAdjacentCharacters(string field)
    {
        if (field.Length < 2) 
        {
            return field;
        } 

        var randomPos = _random.Next(field.Length - 1);
        var chars = field.ToCharArray();
        (chars[randomPos], chars[randomPos + 1]) = (chars[randomPos + 1], chars[randomPos]);

        return new string(chars);
    }

    private string AddRandomCharacter(string field, string region)
    {
        if (_regionOptions is null)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(regionOptions));
        }

        if (!_regionOptions.Regions.TryGetValue(region, out var regionConfig))
        {
            throw new InvalidRegionException(region);
        }

        var randomPos = _random.Next(field.Length + 1);
        var alphabet = regionConfig.Alphabet;
        var randomChar = alphabet[_random.Next(alphabet.Length)];

        return field.Insert(randomPos, randomChar.ToString());
    }
}
