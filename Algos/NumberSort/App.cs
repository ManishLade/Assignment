using System.Diagnostics;
using CsvUtility;
using System.Linq;

namespace NumberSort;
public class App
{
    private const string InputCsv = "PhoneNumbers-8-digits.csv";
    private const string OutputCsv = "PhoneNumbers-8-digitsOutput.csv";

    public async Task RunAsync()
    {
        var path = Directory.GetCurrentDirectory();
        var inputCsvPath = Path.Combine(path, InputCsv);

        var data = await CsvUtilityHelper.GetDataFromInputCsv<PhoneNumberCsv>(inputCsvPath);
        var numbers = data.Select(x => x.PhoneNumber).ToArray();
        
        var stopWatch = Stopwatch.StartNew();
        //calling extension method to sort array 
        var sortedArray = numbers.SortArray(0, numbers.Length - 1);
        stopWatch.Stop();
        Console.WriteLine($"Manual Quick sort execution time:{stopWatch.Elapsed.TotalSeconds}");

        var stopWatch2 = Stopwatch.StartNew();
        //calling built in to sort array 
        Array.Sort(numbers);
        stopWatch2.Stop();
        Console.WriteLine($"Built in Quick sort execution time:{stopWatch.Elapsed.TotalSeconds}");

        var outputFilePath = Path.Combine(path, OutputCsv);
        CsvUtilityHelper.WriteOutPutCsv<int>(outputFilePath, sortedArray);
    }
}