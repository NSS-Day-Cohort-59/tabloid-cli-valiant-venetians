using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TabloidCLI.Models;

namespace TabloidCLI.Repositories

{
    public class PostRepository : DatabaseConnector, IRepository<Post>
    {

        public PostRepository(string connectionString) : base(connectionString) { }

        public List<Post> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id,
                                               Title,
                                               Url,
                                               PublishDateTime,
                                               AuthorId,
                                               BlogId
                                          FROM Post";

                    List<Post> posts = new List<Post>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Post post = new Post()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Url = reader.GetString(reader.GetOrdinal("Url")),
                            PublishDateTime = reader.GetDateTime(reader.GetOrdinal("PublishDateTime")),
                            Author = new Author { Id = reader.GetInt32(reader.GetOrdinal("AuthorId")) },
                            Blog = new Blog { Id = reader.GetInt32(reader.GetOrdinal("BlogId")) }
                        };
                        posts.Add(post);
                    }

                    reader.Close();

                    return posts;
                }
            }
        }

        public List<Post> GetByAuthor(int authorId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT p.Id,
                                               p.Title As PostTitle,
                                               p.URL AS PostUrl,
                                               p.PublishDateTime,
                                               p.AuthorId,
                                               p.BlogId,
                                               a.FirstName,
                                               a.LastName,
                                               a.Bio,
                                               b.Title AS BlogTitle,
                                               b.URL AS BlogUrl
                                          FROM Post p 
                                               LEFT JOIN Author a on p.AuthorId = a.Id
                                               LEFT JOIN Blog b on p.BlogId = b.Id 
                                         WHERE p.AuthorId = @authorId";
                    cmd.Parameters.AddWithValue("@authorId", authorId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Post> posts = new List<Post>();
                    while (reader.Read())
                    {
                        Post post = new Post()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("PostTitle")),
                            Url = reader.GetString(reader.GetOrdinal("PostUrl")),
                            PublishDateTime = reader.GetDateTime(reader.GetOrdinal("PublishDateTime")),
                            Author = new Author()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Bio = reader.GetString(reader.GetOrdinal("Bio")),
                            },
                            Blog = new Blog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BlogId")),
                                Title = reader.GetString(reader.GetOrdinal("BlogTitle")),
                                Url = reader.GetString(reader.GetOrdinal("BlogUrl")),
                            }
                        };
                        posts.Add(post);
                    }

                    reader.Close();

                    return posts;
                }
            }
        }

        public void Insert(Post post)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Post (Title, Url, PublishDateTime, AuthorId, BlogId)
                                                     VALUES (@Title, @Url, @PublishDateTime, @AuthorId, @BlogId)";
                    cmd.Parameters.AddWithValue("@Title", post.Title);
                    cmd.Parameters.AddWithValue("@Url", post.Url);
                    cmd.Parameters.AddWithValue("@PublishDateTime", post.PublishDateTime);
                    cmd.Parameters.AddWithValue("@AuthorId", post.Author.Id);
                    cmd.Parameters.AddWithValue("@BlogId", post.Blog.Id);


                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Update(Post post)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Post 
                                           SET Title = @Title,
                                               URL = @Url
                                               
                                         WHERE id = @id";

                    cmd.Parameters.AddWithValue("@Title", post.Title);
                    cmd.Parameters.AddWithValue("@Url", post.Url);
                    cmd.Parameters.AddWithValue("@id", post.Id);

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
                    cmd.CommandText = @"DELETE FROM Post WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void InsertTag(Post post, Tag tag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO PostTag (PostId, TagId)
                                                       VALUES (@postId, @tagId)";
                    cmd.Parameters.AddWithValue("@postId", post.Id);
                    cmd.Parameters.AddWithValue("@tagId", tag.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTag(int postId, int tagId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM PostTag 
                                         WHERE PostId = @postId AND 
                                               TagId = @tagId";
                    cmd.Parameters.AddWithValue("@postId", postId);
                    cmd.Parameters.AddWithValue("@tagId", tagId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void InsertNote(Post post, Note note)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO PostTag (PostId, NoteId)
                                                       VALUES (@postId, @noteId)";
                    cmd.Parameters.AddWithValue("@postId", post.Id);
                    cmd.Parameters.AddWithValue("@noteId", note.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteNote(int postId, int noteId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM PostTag 
                                         WHERE PostId = @postId AND 
                                               NoteId = @noteId";
                    cmd.Parameters.AddWithValue("@postId", postId);
                    cmd.Parameters.AddWithValue("@noteId", noteId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Post Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT p.Id AS PostId,
                                               p.Title,
                                               p.Url,
                                               p.PublishDateTime,
                                               t.Id AS TagId,
                                               t.Name,
                                               n.Id AS NoteId,
                                               n.Title,
                                               n.Content
                                          FROM Post p 
                                               LEFT JOIN PostTag pt on p.Id = pt.PostId
                                               LEFT JOIN Tag t on t.Id = pt.TagId
                                               LEFT JOIN Note n on n.PostId = p.Id
                                         WHERE p.id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    Post post = null;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (post == null)
                        {
                            post = new Post()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("PostId")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Url = reader.GetString(reader.GetOrdinal("Url")),
                                PublishDateTime = reader.GetDateTime(reader.GetOrdinal("PublishDateTime")),
                            };
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("TagId")))
                        {
                            post.Tags.Add(new Tag()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("TagId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            });
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("NoteId")))
                        {
                            post.Notes.Add(new Note()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("NoteId")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Content = reader.GetString(reader.GetOrdinal("Content")),
                            });
                        }
                    }
                    reader.Close();

                    return post;
                }
            }

        }
    }
}
