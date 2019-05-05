namespace MiniBook.Data.Entities
{
    public class Profile
    {
        public Profile()
        {

        }

        public Profile(string name, string image, string gender)
        {
            Gender = gender;
            Image = image;
            Name = name;
        }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Gender { get; set; }
    }
}