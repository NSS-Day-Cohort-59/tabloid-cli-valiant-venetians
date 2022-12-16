using System;
using System.Collections.Generic;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    internal class NoteManager : IUserInterfaceManager
    {
        private IUserInterfaceManager _parentUI;
        private NoteRepository _noteRepository;
        private PostRepository _postRepository;
        private int _postId;
        private int _noteId;

        public NoteManager(IUserInterfaceManager parentUI, string connectionString, int postId)
        {
            _parentUI = parentUI;
            _noteRepository = new NoteRepository(connectionString);
            _postRepository = new PostRepository(connectionString);
            _noteId = postId; // ?
            _postId = postId;
        }

        public IUserInterfaceManager Execute()
        {
            Console.Clear();

            Post post = _postRepository.Get(_postId);
            Console.WriteLine($"Notes");
            Console.WriteLine(" 1) View Notes");
            Console.WriteLine(" 2) Add Note");
            Console.WriteLine(" 3) Remove Note");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    ListNotes();
                    return this;
                case "2":
                    Add();
                    return this;
                case "3":
                    Remove();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }
        private void ListNotes()
        {
            Console.WriteLine();
            
            List<Note> notes = _noteRepository.GetByPost(new Post { Id = _postId } );

            if (notes.Count == 0)
            {
                Console.WriteLine("There are no notes on this post.");
            }
            else
            {
                foreach (Note note in notes)
                {
                    Console.WriteLine($"{note.Title} | {note.CreateDateTime.ToShortDateString()}");
                    Console.WriteLine(note.Content);
                    Console.WriteLine();
                }
            }


            Console.Write("Press any key to continue");
            Console.ReadKey();
        }
        private void Add()
        {
            Console.WriteLine("New Note");
            Note note = new Note();

            Console.Write(" Title: ");
            note.Title = Console.ReadLine();

            Console.Write(" Note Content: ");
            note.Content = Console.ReadLine();

            note.CreateDateTime= DateTime.Now;

            note.Post = new Post { Id = _postId };

            _noteRepository.Insert(note);
        }

        private Note Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a Note:";
            }

            Console.WriteLine(prompt);

            List<Note> notes = _noteRepository.GetAll();

            for (int i = 0; i < notes.Count; i++)
            {
                Note note = notes[i];
                Console.WriteLine($" {i + 1}) {note.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return notes[choice - 1];
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }
        private void Remove()
        {
            Note noteToDelete = Choose("Which note would you like to remove?");
            if (noteToDelete != null)
            {
                _noteRepository.Delete(noteToDelete.Id);
            }
        }
    }
}