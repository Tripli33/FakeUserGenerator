﻿using Bogus;
using FakeUserGenerator.Exceptions;
using FakeUserGenerator.Models;
using FakeUserGenerator.Utilities.Settings;
using Microsoft.Extensions.Options;
using PhoneNumbers;

namespace FakeUserGenerator.Services;

public class UserGenerator(IOptions<RegionOptions> regionOptions) : IUserGenerator
{
    private readonly RegionOptions _regionOptions = regionOptions.Value;

    public IEnumerable<User> GenerateUsers(string region, int seed, int numberOfUsers, int page)
    {
        if (_regionOptions is null)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(regionOptions));
        }

        if (!_regionOptions.Regions.TryGetValue(region, out var regionConfig))
        {
            throw new InvalidRegionException(region);
        }

        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        var userFaker = new Faker<User>(regionConfig.Bogus)
            .UseSeed(seed + page) 
            .RuleFor(u => u.Number, f => f.IndexFaker + 1 + page * 10)
            .RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(u => u.FullName, f => f.Name.FullName())
            .RuleFor(u => u.Address, f => 
                $"{regionConfig.OriginalName}, {f.Address.City()}, {f.Address.StreetName()}, {f.Address.BuildingNumber()}")
            .RuleFor(u => u.PhoneNumber, f =>
            {
                var randomPhoneNumber = f.Random.ReplaceNumbers("##########");
                var parsedPhoneNumber = phoneNumberUtil.Parse(randomPhoneNumber, regionConfig.LibPhoneNumber);

                return phoneNumberUtil.Format(parsedPhoneNumber, PhoneNumberFormat.INTERNATIONAL);
            });

        return userFaker.Generate(numberOfUsers);
    }
}
