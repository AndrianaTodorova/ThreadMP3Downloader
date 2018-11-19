using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DownloadMP3
{
    public class Mp3Donwloader
    {

        public Mp3Donwloader()
        {
            this.ProccessedTasks = new Dictionary<string, Task>();
        }

        public IDictionary<string, Task> ProccessedTasks { get; set; }

        public IEnumerable<string> GetContentUrls(string filepath)
        {
            //Gets the content from the specified filePath
            return File.ReadAllLines(filepath).ToList();

        }

        public void Download(string uri)
        {
            //Regex for the name of the mp3 files
            Regex reg = new Regex(@"(?:.*\/)(.*?[.]+[a-z0-9]{2,4}$)");
            string fileName = reg.Match(uri).Groups[1].Value.Replace("%20", " ");
            var task = Task.Run(() => this.DownloadMP3(uri));
            var awaiter = task.GetAwaiter();
            //When a task is completed gets the result of it and then writes its content with a fileName specified
            awaiter.OnCompleted(() => this.WriteDataToFile(fileName, awaiter.GetResult()));
            this.ProccessedTasks.Add(fileName, task);
        }

        private void WriteDataToFile(string fileName, byte[] content)
        {
            File.WriteAllBytes($"{fileName}", content);
            Console.WriteLine($"{fileName} downloaded");
        }

        private byte[] DownloadMP3(string uri)
        {
            //Using WebClient class for downloading data from specified URL
            using (WebClient client = new WebClient())
            {
                return client.DownloadData(uri);
            }
        }
    }
}
