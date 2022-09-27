using System.Diagnostics;
using CsvUtility;
using Serilog;

namespace ColorShapes;

public class App
{
    private const string InputCsv = "ColorShapes.csv";
    private const string OutputCsv = "ColorShapesOutput.csv";

    internal async Task RunAsync()
    {
        try
        {
            var path = Directory.GetCurrentDirectory();
            var inputfilePath = Path.Combine(path, InputCsv);
            var stopWatch = Stopwatch.StartNew();

            var colorShapes = await CsvUtilityHelper.GetDataFromInputCsv<ColorShape>(inputfilePath);
            if (colorShapes.Any())
            {
                Log.Information("Colorshapes CSV loaded in memory");
                var orderedColorShapes = colorShapes.EquidistantOrderByColor();

                var outputFilePath = Path.Combine(path, OutputCsv);
                CsvUtilityHelper.WriteOutPutCsv(outputFilePath, orderedColorShapes.ToArray());

                stopWatch.Stop();
                Log.Information("Console ran for {0}.", stopWatch.Elapsed);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }
    }
}