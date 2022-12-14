using System;
using System.Collections.Generic;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    public class PostManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private PostRepository _postRepository;
        private AuthorRepository _authorRepository;
        private BlogRepository _blogRepository;
        private string _connectionString;

        public PostManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _postRepository = new PostRepository(connectionString);
            _connectionString = connectionString;
            _authorRepository = new AuthorRepository(connectionString);
            _blogRepository = new BlogRepository(connectionString);
        }

        
        
        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Post Menu");
            Console.WriteLine(" 1) Add Post");
            Console.WriteLine(" 2) List All Posts");
            Console.WriteLine(" 3) Edit Post");
            Console.WriteLine(" 4) Remove Post");
            Console.WriteLine(" 5) Post Details");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "5":
                    Post post = Choose();
                    if (post == null)
                    {
                        return this;
                    }
                    else
                    {
                        return new PostDetailManager(this, _connectionString, post.Id);
                    }
                case "4":
                    Remove();
                    return this;
                case "3":
                    Edit();
                    return this;
                case "2":
                    GetAll();
                    return this;
                case "1":
                    Add();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }



        private void Add()
        {
            Console.WriteLine("New Post");
            Post post = new Post();

            Console.Write("Title: ");
            post.Title = Console.ReadLine();

            Console.Write("URL: ");
               post.Url = Console.ReadLine();

            Console.Write("Author: ");
            List<Author> authors =
                _authorRepository.GetAll();
            for (int i = 0; i < authors.Count; i++)
            {
                Author author = authors[i];
                Console.WriteLine($"{i+1}) {author.FullName}");
            }
                Console.Write(">");
                post.Author = authors[int.Parse(Console.ReadLine()) -1];
           
            Console.Write("Blog: ");
            List<Blog> blogs =
                _blogRepository.FilteredBlogs(post.Author.Id);
            for (int i = 0; i < blogs.Count; i++)
            {
                Blog blog = blogs[i];
                Console.WriteLine($"{i + 1}) {blog.Title}");
            }
            Console.Write(">");
                post.Blog = blogs[int.Parse(Console.ReadLine()) -1];



            post.PublishDateTime = DateTime.Now;

            _postRepository.Insert(post);
        }

        private void GetAll()
        {
            List<Post> posts = _postRepository.GetAll();
            foreach (Post p in posts)
            {
                Console.WriteLine($"{p.Title}");
                Console.WriteLine($"{p.Url}");
            }
            Console.Write("Press any key to continue");
            Console.ReadKey();
        }

        private Post Choose(string prompt = null)
        {
            Console.Clear();
            if (prompt == null)
            {
                prompt = "Please choose a Post:";
            }

            Console.WriteLine(prompt);
            Console.WriteLine();

            List<Post> posts = _postRepository.GetAll();

            for (int i = 0; i < posts.Count; i++)
            {
                Post post = posts[i];
                Console.WriteLine($@"{i + 1}) {post.Title} 
URL: {post.Url}
Author Id: {post.Author.Id} 
Blog Id: {post.Blog.Id}");
                Console.WriteLine();
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return posts[choice - 1];
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }


        private void Edit()
        {
            Post postToEdit = Choose("Which Post would you like to edit?");
            if (postToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            Console.Write("New title (blank to leave unchanged: ");
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
            {
                postToEdit.Title = title;
            }
            Console.Write("New URL (blank to leave unchanged: ");
            string URL = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(URL))
            {
                postToEdit.Url = URL;
            }

            _postRepository.Update(postToEdit);
        }

        private void Remove()
        {
            Post postToDelete = Choose("Which post would you like to remove?");
            if (postToDelete != null)
            {
                _postRepository.Delete(postToDelete.Id);
            }

            
        }
    }
}
