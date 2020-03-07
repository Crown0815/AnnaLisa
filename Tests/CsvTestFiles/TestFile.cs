using System;
using System.IO;

namespace AnnaLisa.Testing.CsvTestFiles
{
    public class TestFile : IDisposable
    {
        public string FilePath { get; }

        public TestFile(string folderPath, params string[] lines) 
            : this(folderPath, string.Join(Environment.NewLine, lines)){}
        
        public TestFile(string folderPath, string contents)
        {
            FilePath = Path.Join(folderPath, Path.GetRandomFileName());
            File.WriteAllText(FilePath, contents);
        }
        
        public void Dispose() => File.Delete(FilePath);
    }
}