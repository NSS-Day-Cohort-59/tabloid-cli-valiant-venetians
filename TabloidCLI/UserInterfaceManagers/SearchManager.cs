using System;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    internal class SearchManager : IUserInterfaceManager
    {
        private IUserInterfaceManager _parentUI;
        private TagRepository _tagRepository;

        public SearchManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _tagRepository = new TagRepository(connectionString);
        }

        public IUserInterfaceManager Execute()
        {
            Console.Clear();
            Console.WriteLine("Search Menu");
            Console.WriteLine(" 1) Search Blogs");
            Console.WriteLine(" 2) Search Authors");
            Console.WriteLine(" 3) Search Posts");
            Console.WriteLine(" 4) Search All");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    SearchBlogs();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    return this;
                case "2":
                    SearchAuthors();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    return this;
                case "3":
                    SearchPosts();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    return this;
                case "4":
                    SearchAll();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void SearchAuthors()
        {
            Console.Write("Tag> ");
            string tagName = Console.ReadLine();

            SearchResults<Author> results = _tagRepository.SearchAuthors(tagName);

            if (results.NoResultsFound)
            {
                Console.WriteLine($"No results for {tagName}");
            }
            else
            {
                results.Display(true);
            }
        }

        private void SearchBlogs()
        {
            Console.Write("Tag> ");
            string tagName = Console.ReadLine();

            SearchResults<Blog> results = _tagRepository.SearchBlogs(tagName);

            if (results.NoResultsFound)
            {
                Console.WriteLine($"No results for {tagName}");
            }
            else
            {
                results.Display(true);
            }
        }

        private void SearchPosts()
        {
            Console.Write("Tag> ");
            string tagName = Console.ReadLine();

            SearchResults<Post> results = _tagRepository.SearchPosts(tagName);

            if (results.NoResultsFound)
            {
                Console.WriteLine($"No results for {tagName}");
            }
            else
            {
                results.Display(true);
            }
        }
        private void SearchAll()
        {
            Console.Write("Tag> ");
            string tagName = Console.ReadLine();
            Console.WriteLine();

            SearchResults<Author> authorResults = _tagRepository.SearchAuthors(tagName);
            SearchResults<Blog> blogResults = _tagRepository.SearchBlogs(tagName);
            SearchResults<Post> postResults = _tagRepository.SearchPosts(tagName);

            if (authorResults.NoResultsFound && blogResults.NoResultsFound && postResults.NoResultsFound)
            {
                Console.WriteLine($"No results for {tagName} on anything");
                Console.WriteLine();
                return;
            }

            if (!authorResults.NoResultsFound)
            {
                Console.WriteLine("Author Results");
                authorResults.Display(false);
            }
            if (!blogResults.NoResultsFound)
            {
                Console.WriteLine("Blog Results");
                blogResults.Display(false);
            }
            if (!postResults.NoResultsFound)
            {
                Console.WriteLine("Post Results");
                postResults.Display(false);
            }

        }
    }
}