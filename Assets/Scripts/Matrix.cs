using System;

public class Matrix {
    public int rows;
    public int cols;
    double[,] data;
    static Random random = new Random();

    public Matrix(int rows_, int cols_) {
        // constructor for Matrix class
        rows = rows_;
        cols = cols_;

        data = new double[rows, cols];
    }

    public Matrix Copy(){
        // copy matrix
        Matrix result = new Matrix(rows, cols);
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                result.data[i,j] = data[i,j];
            }
        }

        return result;
    }

    public static Matrix FromArray(double[] arr) {
        // load matrix from array
        Matrix result = new Matrix(arr.Length, 1);
        for (int i = 0; i < result.rows; i++) {
            result.data[i,0] = arr[i];
        }

        return result;
    }

    public static double[] ToArray(Matrix m) {
        // convert matrix to array
        double[] result = new double[m.rows * m.cols];
        for (int i = 0; i < m.rows; i++) {
            for (int j = 0; j < m.cols; j++) {
                result[i * m.cols + j] = m.data[i,j];
            }
            
        }

        return result;
    }

    public void Randomize() {
        // randomize all values in matrix
        Map(0, 0, "random");
    }

    public void Mutate(double frequency, double learningRate)
    {
        // mutate some values
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                // slightly change value by random number with slight chance determined by the program
                if (random.NextDouble() < frequency)
                {
                    data[i,j] += random.NextDouble() * learningRate * 2 - learningRate;
                }
            }
        }
    }

    public static Matrix Transpose(Matrix m) {
        // static transposing
        Matrix result = new Matrix(m.cols, m.rows);
        
        for (int i = 0; i < m.rows; i++) {
            for (int j = 0; j < m.cols; j++) {
                result.data[j,i] = m.data[i,j];
            }
        }

        return result;
    }

    static double Function(double x, double m, double b, String f) {
        // function for mapping
        switch(f) {
            case "linear":
                return m * x + b;
            case "sigmoid":
                return 1 / (1 + Math.Exp(-x));
            case "dsigmoid":
                return x * (1 - x);
            case "tanh":
                return Math.Tanh(x);
            case "dtanh":
                return 1 - x * x;
            case "random":
                return random.NextDouble() * 2 - 1;
            default:
                throw new InvalidOperationException("Operation is not an available operation.");
        }
    }


    public void Map(double m, double b, string f) {
        // mapping a function to each element
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                data[i,j] = Function(data[i,j], m, b, f);
            }
        }
    }

    public static Matrix Map(Matrix m, double n, double b, String f) {
        // mapping a function to each element (static)
        Matrix result = new Matrix(m.rows, m.cols);

        for (int i = 0; i < m.rows; i++) {
            for (int j = 0; j < m.cols; j++) {
                result.data[i,j] = Function(m.data[i,j], n, b, f);
            }
        }

        return result;
    }

    public void Multiply(double n) {
        // Scalar multiplication
        Map(n, 0, "linear");
    }

    public void Add(double n) {
        // Scalar addition
        Map(1, n, "linear");
    }

    public static Matrix Subtract(Matrix a, Matrix b) {
         // Element-wise subtraction
         if (a.rows != b.rows || a.cols != b.cols) {
            throw new InvalidOperationException("Dimensions of matrices aren't equal.");
        }

        Matrix result = new Matrix(a.rows, a.cols);
        for (int i = 0; i < a.rows; i++) {
            for (int j = 0; j < b.cols; j++) {
                result.data[i,j] = a.data[i,j] - b.data[i,j];
            }
        }

        return result;
    }

    public static Matrix Multiply(Matrix a, Matrix b) {
        // Matrix product
        if (a.cols != b.rows) {
            throw new InvalidOperationException("Matrix 'a' columns and Matrix 'b' rows aren't equal.");
        }

        Matrix result = new Matrix(a.rows, b.cols);

        for (int i = 0; i < result.rows; i++) {
            for (int j = 0; j < result.cols; j++) {
                double sum = 0;
                for (int k = 0; k < a.cols; k++) {
                    sum += a.data[i,k] * b.data[k,j];
                }

                result.data[i,j] = sum;
            }
        }

        return result;
    }

    public void Multiply(Matrix m) {
        // Element-wise multiplication
        if (rows != m.rows || cols != m.cols) {
            throw new InvalidOperationException("Dimensions of matrices aren't equal.");
        }

        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                data[i,j] *= m.data[i,j];
            }
        }
    }

    public void Add(Matrix m) {
        // Element-wise addition
        if (rows != m.rows || cols != m.cols) {
            throw new InvalidOperationException("Dimensions of matrices aren't equal.");
        }

        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                data[i,j] += m.data[i,j];
            }
        }
    }

    public void Print() {
        // Printing matrix
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(data[i, j].ToString() + " ");
            }
            Console.WriteLine();
        }
    }
}   