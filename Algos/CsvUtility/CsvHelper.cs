using System.Globalization;
using System.Text;
using CsvHelper;

namespace CsvUtility;

public static class CsvUtilityHelper
{
    /// <summary>
    ///     Gets the list from input CSV.
    /// </summary>
    /// <param name="inputFilePath">The input file path.</param>
    /// <returns></returns>
    public static async Task<IEnumerable<T>> GetDataFromInputCsv<T>(string inputFilePath) where T : new()
    {
        var result = new List<T>();
        if (File.Exists(inputFilePath))
            using (var reader = new StreamReader(inputFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                try
                {
                    var records = csv.GetRecordsAsync<T>();
                    result = await records.ToListAsync();
                }
                catch (Exception)
                {
                    Console.WriteLine("CSV file is not valid");
                }
            }

        Console.WriteLine($"CSV file does not exist on given path: {inputFilePath}");
        return result;
    }

    /// <summary>
    ///     Write the output to the csv file
    /// </summary>
    /// <param name="outputFilePath"></param>
    /// <param name="csvObjects"></param>
    /// <typeparam name="T"></typeparam>
    public static void WriteOutPutCsv<T>(string outputFilePath, T[] csvObjects)
    {
        using (var writer = new StreamWriter(outputFilePath, false, Encoding.UTF8))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteHeader<T>();
            csv.NextRecord();

            csv.WriteRecords(csvObjects);

            writer.Flush();
            Console.WriteLine("output csv successfully written");
        }
    }
}