using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI
{
    public class NoteRepository : DatabaseConnector
    {
        public NoteRepository(string connectionString) : base(connectionString) { }
        public List<Note> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Note";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Note> notes = new List<Note>();

                        while (reader.Read())
                        {
                            Note note = new Note
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Content = reader.GetString(reader.GetOrdinal("Content")),
                                Post = new Post { Id = reader.GetInt32(reader.GetOrdinal("PostId")) }
                            };
                            notes.Add(note);
                        }

                        return notes;
                    }
                }
            }
        }
        public List<Note> GetByPost(Post post)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Note WHERE PostId = @postId";
                    cmd.Parameters.AddWithValue("@postId", post.Id);

                    List<Note> notes = new List<Note>();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Note note = new Note
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"))
                        };
                        notes.Add(note);
                    }

                    reader.Close();

                    return notes;
                }
            }
        }
        public void Insert(Note note)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Note (Title, Content, CreateDateTime, PostId)
                                                     VALUES (@Title, @Content, @CreateDateTime, @PostId)";
                    cmd.Parameters.AddWithValue("@Title", note.Title);
                    cmd.Parameters.AddWithValue("@Content", note.Content);
                    cmd.Parameters.AddWithValue("@PostId", note.Post.Id);
                    cmd.Parameters.AddWithValue("@CreateDateTime", note.CreateDateTime);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM Note WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    } 
}

