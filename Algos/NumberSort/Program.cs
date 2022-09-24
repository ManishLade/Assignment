// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using NumberSort;

var path = Directory.GetCurrentDirectory();
var inputCsvPath = Path.Combine(path, "PhoneNumbers-8-digits.csv");

var data = CsvHelperUtility.GetDataFromInputCsv(inputCsvPath).ToArray();
if (data == null) return;

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

var outputFilePath = Path.Combine(path, "OUTPUT.csv");
CsvHelperUtility.WriteOutPutCsv(outputFilePath, sortedArray);