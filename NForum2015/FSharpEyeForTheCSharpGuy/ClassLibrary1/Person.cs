namespace ClassLibrary1
{
    public class Person11
    {
        private readonly string name;
        private readonly int age;

        public Person11(string name, int age)
        {
            this.name = name;
            this.age = age;
        }

        public string Name
        {
            get { return name; }
        }

        public int Age
        {
            get { return age; }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", name, age);
        }
    }
}
