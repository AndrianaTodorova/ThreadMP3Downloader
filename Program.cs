using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;

namespace DownloadMP3
{
    class Program
    {
        static void Main(string[] args)
        {
            const string DIRECTORY = @"DIRECTORY PATH";
            var files = Directory.GetFiles(DIRECTORY);

            //Get the time elapsed using couple of threads
            Stopwatch watch = new Stopwatch();

            watch.Start();
            //Instance of the class which contains the main logic for downloading easily mp3 music
            Mp3Donwloader downloader = new Mp3Donwloader();
            TraverseFiles(files, downloader);

            while (downloader.ProccessedTasks.Count > 0)
            {
                //List containing each song name from the finished tasks
                IList<string> finishedTask = (from downloaderProccessedTask
                    in downloader.ProccessedTasks
                                              where downloaderProccessedTask.Value.IsCompletedSuccessfully
                                              select downloaderProccessedTask.Key).ToList();

                RemoveFinishedTask(downloader, finishedTask);
            }
            watch.Stop();
            // Write result.
            Console.WriteLine("Time elapsed: {0}", watch.Elapsed);
        }

        private static void RemoveFinishedTask(Mp3Donwloader downloader, IList<string> finishedTask)
        {
            foreach (var finishT in finishedTask)
            {
                downloader.ProccessedTasks.Remove(finishT);
            }
        }

        private static void TraverseFiles(string[] files, Mp3Donwloader downloader)
        {
            foreach (var file in files)
            {
                TraverseUrls(downloader, file);
            }
        }

        private static void TraverseUrls(Mp3Donwloader downloader, string file)
        {
            foreach (var contentUrl in downloader.GetContentUrls(file))
            {
                downloader.Download(contentUrl);
            }
        }
    }
}
