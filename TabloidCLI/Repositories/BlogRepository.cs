using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System;
using TabloidCLI.Models;

namespace TabloidCLI.Repositories
{
    internal class BlogRepository : DatabaseConnector, IRepository<Blog>
    {
        public BlogRepository(string connectionString) : base(connectionString) { }

        public List<Blog> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Blog";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Blog> blogs = new List<Blog>();

                        while (reader.Read())
                        {
                            Blog newBlog = new Blog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Url = reader.GetString(reader.GetOrdinal("URL")),
                            };
                            blogs.Add(newBlog);
                        }

                        return blogs;
                    }
                }
            }
        }

        public Blog Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT b.Id AS BlogId,
                                               b.Title,
                                               b.URL,
                                               t.Id AS TagId,
                                               t.Name
                                          FROM Blog b
                                               LEFT JOIN BlogTag bt on b.Id = bt.BlogId
                                               LEFT JOIN Tag t on t.Id = bt.TagId
                                         WHERE b.id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    Blog blog = null;

                    SqlDataReader reader = cmd.ExecuteReader();
                    {
                        while (reader.Read())
                        {
                            if (blog == null)
                            {
                                blog = new Blog()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("BlogId")),
                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    Url = reader.GetString(reader.GetOrdinal("URL"))
                                };
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("TagId")))
                            {
                                blog.Tags.Add(new Tag()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("TagId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                });
                            }
                        }
                    }
                    return blog;
                }
            }
        }

        public void Insert(Blog blog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Blog (Title, URL)
                                                     VALUES (@title, @url)";
                    cmd.Parameters.AddWithValue("@title", blog.Title);
                    cmd.Parameters.AddWithValue("@url", blog.Url);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Blog blog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Blog
                                           SET Title = @title,
                                               URL = @url
                                         WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@title", blog.Title);
                    cmd.Parameters.AddWithValue("@url", blog.Url);
                    cmd.Parameters.AddWithValue("@id", blog.Id);

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
                    cmd.CommandText = @"DELETE FROM Blog WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void InsertTag(Blog blog, Tag tag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO BlogTag (BlogId, TagId)
                                                       VALUES (@blogId, @tagId)";
                    cmd.Parameters.AddWithValue("@blogId", blog.Id);
                    cmd.Parameters.AddWithValue("@tagId", tag.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTag(int blogId, int tagId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM BlogTag 
                                         WHERE AuthorId = @blogid AND 
                                               TagId = @tagId";
                    cmd.Parameters.AddWithValue("@blogId", blogId);
                    cmd.Parameters.AddWithValue("@tagId", tagId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Blog> FilteredBlogs(int AuthorId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT DISTINCT b.*
                                        FROM Blog b
                                        LEFT JOIN Post p ON p.BlogId = b.Id
                                        LEFT JOIN Author a ON p.AuthorId = a.Id
                                        WHERE a.Id = @AuthorId";
                    cmd.Parameters.AddWithValue("@AuthorId", AuthorId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Blog> blogs = new List<Blog>();

                        while (reader.Read())
                        {
                            Blog newBlog = new Blog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Url = reader.GetString(reader.GetOrdinal("URL")),
                            };
                            blogs.Add(newBlog);
                        }

                        return blogs;
                    }
                }
            }
        }
    }
}


