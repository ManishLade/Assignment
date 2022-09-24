using System.Globalization;
using System.Text;
using CsvHelper;

namespace NumberSort;

public class CsvHelperUtility
{
    /// <summary>
    ///     /// Gets the patient list from input CSV.
    /// </summary>
    /// <param name="inputFilePath">The input file path.</param>
    /// <returns></returns>
    public static IEnumerable<PhoneNumbers> GetDataFromInputCsv(string inputFilePath)
    {
        if (File.Exists(inputFilePath))
            using (var reader = new StreamReader(inputFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                try
                {
                    var records = csv.GetRecords<PhoneNumbers>();
                    var result = records.ToList();
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("CSV file is not valid");
                    return null;
                }
            }

        Console.WriteLine($"CSV file does not exist on given path: {inputFilePath}");
        return null;
    }

    public static void WriteOutPutCsv(string outputFilePath, int[] numbers)
    {
        using (var writer = new StreamWriter(outputFilePath, false, Encoding.UTF8))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteHeader<int>();
            csv.NextRecord();

            csv.WriteRecords(numbers);

            writer.Flush();
            Console.WriteLine("output csv successfully written");
        }
    }
}