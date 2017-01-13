using System;

namespace ClassLibrary1
{
    class Interop
    {
        public static void Main(string[] args)
        {
            Test();
        }

        public static void Test()
        {
            var p = new Person1.Person("Magnus", 36);
            var personSameValues = new Person1.Person("Magnus", 36);
            Console.WriteLine(p == personSameValues);

            var p1 = new Person1.Persona("Tove", 5);
            var personaSameValues = new Person1.Persona("Tove", 5);
            Console.WriteLine(p1.GetHashCode() == personaSameValues.GetHashCode());
        }
    }
}
