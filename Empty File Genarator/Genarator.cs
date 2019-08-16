using System;
using System.IO;
using System.Threading;

namespace EmptyFileGenarator
{
    public enum FileSizeUnit { B, KB, MB, GB, TB }
    public class Genarator
    {
        string KB, MB;
        public char B { get; set; } = 'B';
        public Action<string> Message { get; set; }
        public int IntegerPart { get; set; }
        public int DoublePart { get; set; }
        public string Path { get; set; }
        public Genarator(int integerPart, int doublePart)
        {
            IntegerPart = integerPart;
            DoublePart = doublePart;
            KB = new string(B, 1024);
            MB = new string(B, 1024 * 1024);
        }
        /// <summary>
        /// Genarate empty file/files.
        /// </summary>
        /// <param name="count"> number of files (a negetive number for filling the drive)</param>
        /// <param name="Unit">B or KB or MG or GB</param>
        public void FileGen(int count, FileSizeUnit Unit)
        {
            int i = 0;
            Func<StreamWriter, bool> intPartFunc, doublePartFunc;
            StreamWriter streamWriter = null;
            switch (Unit)
            {
                case FileSizeUnit.B:
                    intPartFunc = (sw) =>
                    {
                        try
                        {
                            for (int j = 0; j < IntegerPart; j++) sw.Write(B); return true;
                        }
                        catch (IOException)
                        {
                            return false;
                        }
                    };
                    doublePartFunc = (sw) => { return true; };
                    break;
                case FileSizeUnit.KB:
                    intPartFunc = (sw) =>
                    {
                        try
                        {
                            for (int j = 0; j < IntegerPart; j++) sw.Write(KB); return true;
                        }
                        catch (IOException)
                        {
                            return false;
                        }
                    };
                    doublePartFunc = (sw) =>
                    {
                        try
                        {
                            for (int j = 0; j < DoublePart; j++) sw.Write(B); return true;
                        }
                        catch (IOException)
                        {
                            return false;
                        }
                    };
                    break;
                case FileSizeUnit.MB:
                    intPartFunc = (sw) =>
                    {
                        try
                        {
                            for (int j = 0; j < IntegerPart; j++) { sw.Write(MB); sw.Flush(); }
                            return true;
                        }
                        catch (IOException)
                        {
                            return false;
                        }
                    };
                    doublePartFunc = (sw) =>
                    {
                        try
                        {
                            for (int j = 0; j < DoublePart; j++) sw.Write(KB); return true;
                        }
                        catch (IOException)
                        {
                            return false;
                        }
                    };
                    break;
                case FileSizeUnit.GB:
                    intPartFunc = (sw) =>
                    {
                        try
                        {
                            for (int k = 0; k < IntegerPart; k++) for (int j = 0; j < 1024; j++) { sw.Write(MB); sw.Flush(); }
                            return true;
                        }
                        catch (IOException)
                        {
                            return false;
                        }
                    };
                    doublePartFunc = (sw) =>
                    {
                        try
                        {
                            for (int j = 0; j < DoublePart; j++) { sw.Write(MB); sw.Flush(); }
                            return true;
                        }
                        catch (IOException)
                        {
                            return false;
                        }
                    };
                    break;
                case FileSizeUnit.TB:
                    intPartFunc = (sw) =>
                    {
                        try
                        {
                            for (int z = 0; z < IntegerPart; z++) for (int k = 0; k < 1024; k++) for (int j = 0; j < 1024; j++) { sw.Write(MB); sw.Flush(); }
                            return true;
                        }
                        catch (IOException)
                        {
                            return false;
                        }
                    };
                    doublePartFunc = (sw) =>
                    {
                        try
                        {
                            for (int k = 0; k < IntegerPart; k++) for (int j = 0; j < DoublePart; j++) { sw.Write(MB); sw.Flush(); }
                            return true;
                        }
                        catch (IOException)
                        {
                            return false;
                        }
                    };
                    break;
                default:
                    intPartFunc = (sw) => { return true; };
                    doublePartFunc = (sw) => { return true; };
                    break;
            }

            for (int j = 0; j != count; j++)
            {
                while (File.Exists($"{Path}\\{i}.txt"))
                    i++;
                streamWriter = new StreamWriter($"{Path}\\{i}.txt");
                if (!doublePartFunc(streamWriter) || !intPartFunc(streamWriter))
                {
                    Message("Drive is full");
                    try
                    {
                        streamWriter.Close();
                    }
                    catch (Exception) { }
                    return;
                }
                streamWriter.Close();
            }
        }
        /// <summary>
        /// Genarate empty file/files but in new thread.
        /// </summary>
        /// <param name="count"> number of files (a negetive number for filling the drive)</param>
        /// <param name="Unit">B or KB or MG or GB</param>
        /// <param name="onEnd"> this will run at the end</param>
        public void FileGen(int count, FileSizeUnit Unit, Action onEnd)
        {
            new Thread(() =>
            {
                FileGen(count, Unit);
                onEnd();
            }).Start();
        }
    }
}
