using Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MovieLibrary
{
    public class MainService : IMainService
    {
        private MovieContext _dbContext;
        private readonly ILogger<MainService> _logger;
        private readonly IDbContextFactory<MovieContext> _dbContextFactory;
        public MainService(ILogger<MainService> logger,IDbContextFactory<MovieContext> dbContextFactory)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
            _dbContext = _dbContextFactory.CreateDbContext();
        }

        public void Invoke()
        {
            bool exit = false;
            int choice = 0;
            Console.WriteLine("Welcome to Blockbuster!");
            do
            {
                choice = MovieMenu();

                if (choice == 1)
                {
                    SearchMovie();
                }
                else if (choice == 2)
                {
                    AddUpdateNewMovie();
                }
                else if (choice == 3)
                {
                    UpdateMovie();
                }
                else if (choice == 4)
                {
                    DeleteMovie();
                }
                else if (choice == 5)
                {
                    exit = true;
                }
            } while (!exit);
            Console.WriteLine("Thank you for visiting Blockbuster, Good Bye!");
        }
        public int MovieMenu()
        {
            int choice = 0;
            Console.WriteLine("----- Movie Menu -----");
            Console.WriteLine("1) Search for a Movie");
            Console.WriteLine("2) Add a Movie");
            Console.WriteLine("3) Update a Movie");
            Console.WriteLine("4) Delete a Movie");
            Console.WriteLine("5) To exit");
            choice = Input.GetIntWithPrompt("Select an option: ", "Please try again");  
            do
            {
                if (choice > 5 || choice < 1)
                {
                    Console.WriteLine("Please select a menu option");
                    choice = Input.GetIntWithPrompt("Select an option: ", "Please try again");
                }
            } while (choice > 5 || choice < 1);

            return choice;
        }
        public void SearchMovie()
        {           
            Console.WriteLine();                                                                                                        
            string movieSearch = Input.GetStringWithPrompt("What Movie would you like to search for? ", "Please try again, entry can't be null: ");
           // Console.WriteLine("What Movie would you like to search for?")   //This would work for allowing a null entry to return all movies
           // var movieSearch = Console.ReadLine();   
            var movielist = _dbContext.Movies.Where(x => x.Title.Contains(movieSearch)).ToList();
            if (movielist.Count() == 0)
            {
                Console.WriteLine($"There are {movielist.Count()} similar references that match your search");
            }
            else if (movielist.Count() == 1)
            {
                Console.WriteLine($"There is {movielist.Count()} similar reference that matches your search");
                Console.WriteLine();
                foreach (var movie in movielist)
                {
                    Console.WriteLine("Moive ID: {0}", movie.Id);
                    Console.WriteLine("     Movie Title: {0} ", movie.Title);
                }
            }
            else if
                 (movielist.Count() > 1)
            {
                Console.WriteLine($"There are {movielist.Count()} similar reference that matches your search");
                Console.WriteLine();
                foreach (var movie in movielist)
                {
                    Console.WriteLine("Moive ID: {0}", movie.Id);
                    Console.WriteLine("     Movie Title: {0} ", movie.Title);
                }
            }
            Console.WriteLine();
        }
        public void AddUpdateNewMovie()
        {                      
            string movieSearch = Input.GetStringWithPrompt("Enter the Movie Title to search if a similar reference already exsist: ", "Please try again, Movie Title can't be blank: ");        
            var movieadd = _dbContext.Movies.Where(x => x.Title.Contains(movieSearch)).ToList();
            if (movieadd.Count() > 0)
            {                   
                if (movieadd.Count() == 1)
                {
                    Console.WriteLine($"There is {movieadd.Count()} similar reference that matches the movie you'd like to add:");
                    Console.WriteLine();
                    foreach (var movie in movieadd)
                    {
                        Console.WriteLine("Moive ID: {0}", movie.Id);
                        Console.WriteLine("     Movie Title: {0} ", movie.Title);
                    }
                }
                else if (movieadd.Count() > 1)
                {
                    Console.WriteLine($"There are {movieadd.Count()} similar references that match the movie you'd like to add:");
                    Console.WriteLine();
                    foreach (var movie in movieadd)
                    {
                        Console.WriteLine("Moive ID: {0}", movie.Id);
                        Console.WriteLine("     Movie Title: {0} ", movie.Title);
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Would you like to:");
                Console.WriteLine("1. Update an exsisting reference");
                Console.WriteLine("2. Add a new reference");
                Console.WriteLine("3. Return to the Main Menu");
                int selection = Input.GetIntWithPrompt("Please select a number: ", "Please try again: ");
                do
                {
                    if (selection > 3 || selection < 1)
                    {
                        selection = Input.GetIntWithPrompt("Please select 1, 2, or 3: ", "Please try again");
                    }
                } while (selection > 3 || selection < 1);
                if (selection == 1) 
                {                      
                    bool doneadd = false;
                    do
                    {
                        Console.WriteLine();
                        Console.WriteLine("Which movie would you like to update? ");                       
                        foreach (var movie in movieadd)
                        {
                            Console.WriteLine("Moive ID: {0}, {1}", movie.Id, movie.Title);
                        }
                        Console.WriteLine();
                        int movieToUpdate = Input.GetIntWithPrompt("Select a movie by the Id that you'd like to update: ", "Please try again: ");
                        if (movieadd.Any(x => x.Id == movieToUpdate))
                        {
                            bool validSelection = false;
                            do
                            {
                                var updateConfirm = _dbContext.Movies.Where(x => x.Id == movieToUpdate).FirstOrDefault();
                                Console.WriteLine($"Is Id: {updateConfirm.Id}, Title: {updateConfirm.Title} the correct movie you'd like to update?");
                                Console.WriteLine("Y to confirm, N to select again, or E to return to the Main Menu: ");
                                string yesNo = Console.ReadLine();
                                yesNo = yesNo.ToUpper();
                                if (yesNo == "Y")
                                {
                                    bool doneDate = false;
                                    string oldTitle = updateConfirm.Title;
                                    string movieTitle = Input.GetStringWithPrompt("Enter the new Movie Title: ", "The Movie Title can not be blank, Please try again: ");
                                    Console.WriteLine("Please enter the release date in YYYY-MM-DD format:");
                                    var releaseDate = Console.ReadLine();
                                    do
                                    {
                                        var didParse = DateTime.TryParse(releaseDate, out var rdate);
                                        if (didParse)
                                        {
                                            doneDate = true;
                                        }
                                        else
                                        {
                                            Console.WriteLine("That date was not in the correct format: ");
                                            Console.WriteLine("Please enter the release date in YYYY-MM-DD format:");
                                            releaseDate = Console.ReadLine();
                                        }
                                    } while (!doneDate);
                                    updateConfirm.Title = movieTitle;
                                    updateConfirm.ReleaseDate = Convert.ToDateTime(releaseDate);
                                    _dbContext.SaveChanges();
                                    Console.WriteLine();
                                    Console.WriteLine($"Id: {updateConfirm.Id},Movie Title: {oldTitle} has been updated to Id: {updateConfirm.Id}, Movie Title: {updateConfirm.Title}");
                                    Console.WriteLine("Returning to the main menu.");
                                    Console.WriteLine();
                                    validSelection = true;
                                    doneadd = true;
                                }
                                else if (yesNo == "N")
                                {
                                    Console.WriteLine("Please select again");
                                    validSelection = true;
                                }
                                else if (yesNo == "E")
                                {
                                    Console.WriteLine("Returning to the Main Menu");
                                    validSelection = true;
                                    doneadd = true;
                                }
                                else
                                {
                                    Console.WriteLine("That is not a vaild selection, Please try again.");
                                }
                            } while (!validSelection);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("That is not a listed movie id, please try again");
                        }                          
                    } while (!doneadd);
                }
                else if (selection == 2) 
                {                     
                    NewMovie();
                }
                else if (selection == 3) 
                {
                    Console.WriteLine("Returning to the main menu: ");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine($"There are {movieadd.Count()} similar references that match your search");
                NewMovie();
            }           
        }
        public void NewMovie()
        {
            bool doneDate = false;
            string movieTitle = Input.GetStringWithPrompt("Enter the new Movie Title: ", "The Movie Title can not be blank, Please try again: ");         
            Console.WriteLine("Please enter the release date in YYYY-MM-DD format:");
            var releaseDate = Console.ReadLine();           
            do
            {
                var didParse = DateTime.TryParse(releaseDate, out var rdate);
                if (didParse)
                {
                    doneDate = true;
                }
                else
                {
                    Console.WriteLine("That date was not in the correct format: ");
                    Console.WriteLine("Please enter the release date in YYYY-MM-DD format:");
                    releaseDate = Console.ReadLine();
                }
            } while (!doneDate);
            var movie = new Movie();
            movie.Title = movieTitle;
            movie.ReleaseDate = Convert.ToDateTime(releaseDate);
            Console.WriteLine($"{movie.Title} has been added");
            _dbContext.Movies.Add(movie);
            _dbContext.SaveChanges();
            Console.WriteLine();
        }
        public void UpdateMovie()
        {
            string movieSearch = Input.GetStringWithPrompt("Enter a movie title to search for that you'd like to update: ", "Please try again, Movie Title can't be blank: ");
            var movieUpdate = _dbContext.Movies.Where(x => x.Title.Contains(movieSearch)).ToList();
            if (movieUpdate.Count() > 0)
            {
                if (movieUpdate.Count() == 1)
                {
                    Console.WriteLine($"There is {movieUpdate.Count()} similar reference that matches the movie you'd like to update:");
                    Console.WriteLine();
                    foreach (var movie in movieUpdate)
                    {
                        Console.WriteLine("Moive ID: {0}", movie.Id);
                        Console.WriteLine("     Movie Title: {0} ", movie.Title);
                    }
                }
                else if
                     (movieUpdate.Count() > 1)
                {
                    Console.WriteLine($"There are {movieUpdate.Count()} similar references that match the movie you'd like to update:");
                    Console.WriteLine();
                    foreach (var movie in movieUpdate)
                    {
                        Console.WriteLine("Moive ID: {0}", movie.Id);
                        Console.WriteLine("     Movie Title: {0} ", movie.Title);
                    }
                }
                Console.WriteLine("");
                Console.WriteLine("Would you like to:");
                Console.WriteLine("1. Update an exsisting reference.");
                Console.WriteLine("2. Return to the Main Menu.");
                int selection = Input.GetIntWithPrompt("Please select a number: ", "Please try again: ");
                do
                {
                    if (selection > 2 || selection < 1)
                    {
                        selection = Input.GetIntWithPrompt("Please select 1 or 2: ", "Please try again");
                    }
                } while (selection > 2 || selection < 1);
                if (selection == 1)
                {
                    bool doneadd = false;
                    do
                    {
                        Console.WriteLine();
                        Console.WriteLine("Which movie would you like to update? ");                      
                        foreach (var movie in movieUpdate)
                        {
                            Console.WriteLine("Moive ID: {0}, {1}", movie.Id, movie.Title);

                        }
                        Console.WriteLine();
                        int movieToUpdate = Input.GetIntWithPrompt("Select a movie by the Id that you'd like to update: ", "Please try again: ");
                        if
                        (movieUpdate.Any(x => x.Id == movieToUpdate))
                        {
                            bool validSelection = false;
                            do
                            {
                                var updateConfirm = _dbContext.Movies.Where(x => x.Id == movieToUpdate).FirstOrDefault();
                                Console.WriteLine($"Is Id: {updateConfirm.Id}, Title: {updateConfirm.Title} the correct movie you'd like to update?");
                                Console.WriteLine("Y to confirm or N to return to the Main Menu:");
                                string yesNo = Console.ReadLine();
                                yesNo = yesNo.ToUpper();
                                if (yesNo == "Y")
                                {
                                    bool doneDate = false;
                                    string oldTitle = updateConfirm.Title;
                                    string movieTitle = Input.GetStringWithPrompt("Enter the new Movie Title: ", "The Movie Title can not be blank, Please try again: ");
                                    Console.WriteLine("Please enter the release date in YYYY-MM-DD format:");
                                    var releaseDate = Console.ReadLine();
                                    do
                                    {
                                        var didParse = DateTime.TryParse(releaseDate, out var rdate);
                                        if (didParse)
                                        {
                                            doneDate = true;
                                        }
                                        else
                                        {
                                            Console.WriteLine("That date was not in the correct format: ");
                                            Console.WriteLine("Please enter the release date in YYYY-MM-DD format:");
                                            releaseDate = Console.ReadLine();
                                        }
                                    } while (!doneDate);
                                    updateConfirm.Title = movieTitle;
                                    updateConfirm.ReleaseDate = Convert.ToDateTime(releaseDate);
                                    _dbContext.SaveChanges();
                                    Console.WriteLine();
                                    Console.WriteLine($"Id: {updateConfirm.Id},Movie Title: {oldTitle} has been updated to Id: {updateConfirm.Id}, Movie Title: {updateConfirm.Title}");
                                    Console.WriteLine("Returning to the main menu.");
                                    Console.WriteLine();
                                    validSelection = true;
                                    doneadd = true;
                                }
                                else if (yesNo == "N")
                                {
                                    Console.WriteLine("Returning to the Main Menu");
                                    Console.WriteLine();
                                    validSelection = true;
                                    doneadd = true;
                                }
                                else
                                {
                                    Console.WriteLine("Selection not valid, Please try again.");
                                }
                            } while (!validSelection);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("That is not a listed movie id, please try again");
                        }
                    } while (!doneadd);
                }
                else if (selection == 2)
                {
                    Console.WriteLine("Returning to the main menu: ");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine($"There are {movieUpdate.Count()} similar references that match your search");
                Console.WriteLine();
            }
        }
        public void DeleteMovie()
        {
            string movieSearch = Input.GetStringWithPrompt("Enter a movie title to search for that you'd like to Delete: ", "Please try again, Movie Title can't be blank: ");
            var movieDelete = _dbContext.Movies.Where(x => x.Title.Contains(movieSearch)).ToList();
            if (movieDelete.Count() > 0)
            {
                if (movieDelete.Count() == 1)
                {
                    Console.WriteLine($"There is {movieDelete.Count()} similar reference that matches the movie you'd like to delete:");
                    Console.WriteLine();
                    foreach (var movie in movieDelete)
                    {
                        Console.WriteLine("Moive ID: {0}", movie.Id);
                        Console.WriteLine("     Movie Title: {0} ", movie.Title);
                    }
                }
                else if
                     (movieDelete.Count() > 1)
                {
                    Console.WriteLine($"There are {movieDelete.Count()} similar references that match the movie you'd like to delete:");
                    Console.WriteLine();
                    foreach (var movie in movieDelete)
                    {
                        Console.WriteLine("Moive ID: {0}", movie.Id);
                        Console.WriteLine("     Movie Title: {0} ", movie.Title);
                    }
                }
                Console.WriteLine("");
                Console.WriteLine("Would you like to:");
                Console.WriteLine("1. Delete an exsisting reference.");
                Console.WriteLine("2. Return to the Main Menu.");
                int selection = Input.GetIntWithPrompt("Please select a number: ", "Please try again: ");
                do
                {
                    if (selection > 2 || selection < 1)
                    {
                        selection = Input.GetIntWithPrompt("Please select 1 or 2: ", "Please try again");
                    }
                } while (selection > 2 || selection < 1);
                if (selection == 1)
                {
                    bool doneDelete = false;
                    do
                    {
                        Console.WriteLine();
                        Console.WriteLine("Which movie would you like to Delete? ");                       
                        foreach (var movie in movieDelete)
                        {
                            Console.WriteLine("Moive ID: {0}, {1}", movie.Id, movie.Title);

                        }
                        Console.WriteLine();
                        int movieToDelete = Input.GetIntWithPrompt("Select a movie by the Id that you'd like to Delete: ", "Please try again: ");
                        if
                        (movieDelete.Any(x => x.Id == movieToDelete))
                        {
                            bool validSelection = false;
                            do
                            {
                                var deleteConfirm = _dbContext.Movies.Where(x => x.Id == movieToDelete).FirstOrDefault();
                                Console.WriteLine($"Is Id: {deleteConfirm.Id}, Title: {deleteConfirm.Title}, the correct movie you'd like to Delete?");
                                Console.WriteLine("Y to confrim or N to return to Main Menu:");
                                string yesNo = Console.ReadLine();
                                yesNo = yesNo.ToUpper();
                                if (yesNo == "Y")
                                {
                                    bool confirmValidation = false;
                                    validSelection = true;
                                    do
                                    {
                                        Console.WriteLine("Are you sure you want to Delete this Reference?");
                                        Console.WriteLine("Y to confirm or N to return to the Main Menu");
                                        yesNo = Console.ReadLine();
                                        yesNo = yesNo.ToUpper();
                                        if (yesNo == "Y")
                                        {
                                            _dbContext.Remove(deleteConfirm);
                                            _dbContext.SaveChanges();
                                            Console.WriteLine();
                                            Console.WriteLine($"Id: {deleteConfirm.Id},Movie Title: {deleteConfirm.Title} has been Deleted");
                                            Console.WriteLine("Returning to the main menu.");
                                            Console.WriteLine();
                                            confirmValidation = true;
                                           
                                            doneDelete = true;
                                        }
                                        else if (yesNo == "N")
                                        {
                                            Console.WriteLine("Returning to the Main Menu");
                                            Console.WriteLine();
                                            confirmValidation= true;
                                            
                                            doneDelete = true;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Selection not valid, Please try again.");
                                        }                                   
                                    } while (!confirmValidation);
                                }
                                else if (yesNo == "N")
                                {
                                    Console.WriteLine("Returning to the Main Menu");
                                    Console.WriteLine();
                                    validSelection = true;
                                    doneDelete = true;
                                }
                                else
                                {
                                    Console.WriteLine("Selection not valid, Please try again.");
                                }
                            } while (!validSelection);
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("That is not a listed movie id, please try again");
                        }
                    } while (!doneDelete);
                }
                else if (selection == 2)
                {
                    Console.WriteLine("Returning to the main menu: ");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine($"There are {movieDelete.Count()} similar references that match your search");
                Console.WriteLine();
            }
        }
    }   
}

//Code trash can
//var movieadd = db.Movies.Select(x => x.Title).ToList();
//var movieadd = db.Movies.AsEnumerable().Select(x=> x.Title.Split('(')).Where(x=> string.Equals(x,moviet,StringComparison.OrdinalIgnoreCase)).ToList();
//var movieadd = db.Movies.Select(x => x.Title.Split('(')).Select(a => new {mo = });
//var movieadd = db.Movies.Select(x => x.Title.Split('(')).Select(x=> new {movietitle = x[0] }).Where(x=> x.movietitle == moviet);
//var movieadd = db.Movies.AsEnumerable().Select(x => x.Title).Select(x=> x.Split('(')).Select(x => new { movietitle = x[0] }).Where(x => x.movietitle == moviet).ToList();
//var movieadd = db.Movies.FirstOrDefault(x => x.Title.Split('(').Select(x => new { movietitle = x[0] }).Where(x => x.movietitle == moviet);
//var movieadd = db.Movies.FirstOrDefault(x => x.Title == movietitle);

//var didParse = DateTime.TryParse(releaseDate, out var date);
//string releaseDate = Input.GetDateWithPrompt("Please enter a release date in YYYY-MM-DD format, Please try again:");
//string date;
//foreach (var movie in movieadd)
//{
//(movieToUpdate == movie.Id)
//var movie = new Movie();
//_dbContext.Movies.Add(movie);
// }
//_dbContext.Remove(deleteConfirm);
//_dbContext.SaveChanges();
//Console.WriteLine();
//Console.WriteLine($"Id: {deleteConfirm.Id},Movie Title: {deleteConfirm.Title} has been Deleted");
//Console.WriteLine("Returning to the main menu.");
//Console.WriteLine();
//doneadd = true;