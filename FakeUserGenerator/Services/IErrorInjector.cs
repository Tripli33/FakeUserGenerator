using FakeUserGenerator.Models;

namespace FakeUserGenerator.Services;

public interface IErrorInjector
{
    IEnumerable<User> InjectErrors(IEnumerable<User> users, double errorCount, string region);
}
