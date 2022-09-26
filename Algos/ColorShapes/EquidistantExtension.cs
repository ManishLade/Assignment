// See https://aka.ms/new-console-template for more information

namespace ColorShapes;

public static class EquidistantExtension
{
    /// <summary>
    ///     spreding items in collection evenly.
    /// </summary>
    /// <param name="colorShapes"></param>
    /// <returns></returns>
    public static List<ColorShape> EquidistantOrderByColor(this IEnumerable<ColorShape> colorShapes)
    {
        var colors = new List<ColorShape>();
        var colorGroup = colorShapes.GroupBy(x => x.Color).OrderByDescending(x => x.Count());

        foreach (var (item, index) in colorGroup.Select((item, i) => (item, i)))
            if (index == 0)
                foreach (var y in item)
                    colors.Add(y);
            else
            {
                var arrList = item.ToArray();
                var i = index;
                do
                {
                    colors.Insert(i, arrList[(i - index) / (index + 1)]);
                    i += index + 1;
                } while ((i - 1) / (index + 1) < arrList.Length);
            }

        return colors;
    }
}