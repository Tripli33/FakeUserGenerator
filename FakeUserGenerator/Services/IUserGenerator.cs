using FakeUserGenerator.Models;

namespace FakeUserGenerator.Services;

public interface IUserGenerator
{
    IEnumerable<User> GenerateUsers(string region, int seed, int numberOfUsers);
}
