using System.IO;

public class SequentialNumberManager
{
    private readonly string _filePath;
    private static readonly object _lock = new object();

    public SequentialNumberManager(string filePath)
    {
        _filePath = filePath;
    }

    public int GetNextSequentialNumber()
    {
        lock (_lock)
        {
            int lastNumber = 1;

            if (File.Exists(_filePath))
            {
                var content = File.ReadAllText(_filePath);
                if (int.TryParse(content, out lastNumber))
                {
                    lastNumber++;
                }
            }

            File.WriteAllText(_filePath, lastNumber.ToString());
            return lastNumber;
        }
    }
}