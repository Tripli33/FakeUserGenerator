using FakeUserGenerator.Models;

namespace FakeUserGenerator.Services;

public interface IExportCsvService
{
    MemoryStream ExportCsvStream(IEnumerable<User> users);
}
