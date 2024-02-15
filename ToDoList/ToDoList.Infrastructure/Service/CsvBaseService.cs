using CsvHelper;
using CsvHelper.Configuration;
using System.Collections;
using System.Globalization;
using System.Text;

namespace ToDoList.Infrastructure.Service
{
    public class CsvBaseService<T> // Для формирования файла, хранящего статистику
    {
        private readonly CsvConfiguration _csvConfiguration;
        public CsvBaseService() 
        {
            _csvConfiguration = GetConfigeration();
            
        }

        public byte[] UpLoadFile(IEnumerable data) // Выгрузка файла
        {
            using(var memoreingStream = new MemoryStream()) 
            using(var streamWriter = new StreamWriter(memoreingStream)) 
            using(var csvWriter = new CsvWriter(streamWriter, _csvConfiguration))
            {
                csvWriter.WriteRecords(data);
                streamWriter.Flush();
                return memoreingStream.ToArray();
            }
        }   
        private CsvConfiguration GetConfigeration()
        {
            return new CsvConfiguration(CultureInfo.InvariantCulture) 
            {
                Delimiter = ",",
                Encoding = Encoding.UTF8,
                NewLine = "\r\n"  // Для последующих строк
            };
        }
    }
}
