using System;
using CsvHelper; // I was able to use the CsvHelper to print out the list of movies but was very difficult to know how to get it to do anything else, Im still new to some of the concepts
using Helper;
using System.IO;
using System.Globalization;
using System.Linq;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;
using System.Data;
using System.Collections.Generic;
using BetterConsoleTables;// This one was also hard to understand ontop of looping through the Csvhelper list
using MovieLibrary;
using Microsoft.Extensions.DependencyInjection;

namespace MovieLibrary
{
    public class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var startup = new Startup();
                var serviceProvider = startup.ConfigureServices();
                var service = serviceProvider.GetService<IMainService>();

                service?.Invoke();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }

    }  
}



//bool exit = false;
//int choice = 0;
////  List<MovieInfo> list = new List<MovieInfo>();
////  List<string> genre = new List<string>();
//Console.WriteLine("Welcome to Blockbuster!");
//do
//{
//    choice = MovieInfo.MovieMenu();

//    if (choice == 1)
//    {
//        MovieInfo.SearchMovie();
//    }
//    else if (choice == 2)
//    {
//        MovieInfo.AddNewMovie();
//    }
//    else if (choice == 3)
//    {
//        exit = true;
//    }
//    else
//    {
//        Program.DoSomething();
//    }

//} while (!exit);
//Console.WriteLine("Thank you for visiting Blockbuster, Good Bye!");



