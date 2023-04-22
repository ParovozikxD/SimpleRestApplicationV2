namespace SimpleRestApplicationV2
{
    public class User
    {
        public User(string name, int age)
        {
            ID = Guid.NewGuid().ToString();
            Name = name;
            Age = age;
        }

        public string ID { get; set; }
        public string Name { get; set; }

        public int Age { get; set; }

    }
}
