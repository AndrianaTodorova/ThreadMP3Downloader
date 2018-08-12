# ThreadMP3Downloader
Threading enables your C# program to perform concurrent processing so that you can do more than one operation at a time. 
For example, you can use threading to download a couple of mp3 files and the proccess is way faster than doing it on 
just one main thread.This is because with several threads you don't need to wait one big size mp3 file to get downloaded, 
and then to download another smaller one (this can happen concurrently).This simple program is using only files with url's 
navigated to the mp3 and downloads the content in several threads.You can see the difference (with StopWatch for example) 
between using only one thread (the main) and using several different threads.