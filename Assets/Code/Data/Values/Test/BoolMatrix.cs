[System.Serializable]
public class BoolMatrix
{
    public BoolArray[] Matrix;

    public BoolMatrix(int rows, int cols)
    {
        Matrix = new BoolArray[rows];
        for (int i = 0; i < rows; i++)
        {
            Matrix[i] = new BoolArray(cols);
        }
    }

    public bool GetValue(int row, int col)
    {
        return Matrix[row].Array[col];
    }

    public void SetValue(int row, int col, bool value)
    {
        Matrix[row].Array[col] = value;
    }
}