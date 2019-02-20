using System;
namespace npower_beyond.Domain
{
    public class UserResponse
    {
       public UserData[] Results { get; set; }
    }

    public class UserData
    {
        public string Gender { get; set; }
        public UserName Name { get; set; }
        public UserPicture Picture { get; set; }
    }

    public class UserName
    {
        public string Title { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
    }

    public class UserPicture
    {
        public string Large { get; set; }
    }
}
