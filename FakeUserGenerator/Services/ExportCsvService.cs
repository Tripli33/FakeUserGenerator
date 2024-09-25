using CsvHelper;
using FakeUserGenerator.Models;
using System.Formats.Asn1;
using System.Globalization;

namespace FakeUserGenerator.Services;

public class ExportCsvService : IExportCsvService
{
    public MemoryStream ExportCsvStream(IEnumerable<User> users)
    {
        var memoryStream = new MemoryStream();
        var streamWriter = new StreamWriter(memoryStream);
        var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

        csvWriter.WriteRecords(users);
        streamWriter.Flush();
        memoryStream.Position = 0;

        return memoryStream;
    }
}
