
namespace EDU_TCP
{
    public class UserClass
    {
        public string Name { get; set; }
        public string Info { get; set; }

        public UserClass()
        {
            Name = "Unknow";
            Info = "Not have User`s info...";
        }

        public UserClass(string _name, string _info)
        {
            Name = _name;
            Info = _info;
        }

    }
}
