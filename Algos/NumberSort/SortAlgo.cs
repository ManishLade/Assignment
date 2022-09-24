namespace NumberSort;

public static class SortAlgo
{
    public static int[] SortArray(this int[] array, int leftIndex, int rightIndex)
    {
        if (array == null)
            return null;

        if (array.Length == 0)
            return new int[0];

        if (leftIndex < 0 || leftIndex >= array.Length)
            throw new IndexOutOfRangeException();

        if (rightIndex < 0 || rightIndex >= array.Length)
            throw new IndexOutOfRangeException();

        var i = leftIndex;
        var j = rightIndex;
        var pivot = array[leftIndex];
        while (i <= j)
        {
            while (array[i] < pivot) i++;

            while (array[j] > pivot) j--;
            if (i <= j)
            {
                var temp = array[i];
                array[i] = array[j];
                array[j] = temp;
                i++;
                j--;
            }
        }

        if (leftIndex < j)
            SortArray(array, leftIndex, j);
        if (i < rightIndex)
            SortArray(array, i, rightIndex);
        return array;
    }
}