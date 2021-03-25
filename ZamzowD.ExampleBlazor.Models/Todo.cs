using System;

namespace ZamzowD.ExampleBlazor.Models
{
    public class Todo : IEquatable<Todo>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }

        public bool Equals(Todo other)
        {
            return Id == other.Id
                && UserId == other.UserId
                && Title == other.Title
                && Completed == other.Completed;
        }
    }
}
