// See https://aka.ms/new-console-template for more information

using System.Globalization;
using System.Text;
using CsvHelper;
using Serilog;

namespace ColorShapes;

public static class CSVHelper
{
    public static async Task<IEnumerable<ColorShape>?> GetCSV(string inputFilePath)
    {
        if (File.Exists(inputFilePath))
            using (var reader = new StreamReader(inputFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                try
                {
                    var records = csv.GetRecordsAsync<ColorShape>();
                    return await records.ToListAsync();
                }
                catch (Exception)
                {
                    Log.Error("CSV file is not valid");
                    return null;
                }
            }

        Log.Information($"CSV file does not exist on given path: {inputFilePath}");
        return null;
    }

    public static void WriteOutPutCsv(string outputFilePath, List<ColorShape> colors)
    {
        using (var writer = new StreamWriter(outputFilePath, false, Encoding.UTF8))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteHeader<ColorShape>();
            csv.NextRecord();

            csv.WriteRecords(colors);

            writer.Flush();
            Log.Information("output csv successfully written");
        }
    }
}