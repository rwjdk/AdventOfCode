namespace Utils;

public static class DoubleArrayExtensions
{
    public static T[,] PadArray<T>(this T[,] originalArray, int padding, T fill)
    {
        int rows = originalArray.GetLength(0);
        int cols = originalArray.GetLength(1);

        // Calculate dimensions for the padded array
        int paddedRows = rows + 2 * padding;
        int paddedCols = cols + 2 * padding;

        // Create a new padded array
        T[,] paddedArray = new T[paddedRows, paddedCols];

        //Fill array with default
        for (int i = 0; i < paddedRows; i++)
        {
            for (int j = 0; j < paddedCols; j++)
            {
                paddedArray[i, j] = fill;
            }
        }

        // Copy values from the original array to the padded array
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                paddedArray[i + padding, j + padding] = originalArray[i, j];
            }
        }

        return paddedArray;
    }
}