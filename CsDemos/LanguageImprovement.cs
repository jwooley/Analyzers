using System;

namespace TestProject
{
    public class LanguageImprovement
    {
        public static void LanguageImprovementTests()
        {
            int x;
            if (int.TryParse("123", out x))
                DoSomething();
            DoSomethingElse();

        }


        public static void DoSomething() { }
        public static void DoSomethingElse() { }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName()
        {
            return $"{FirstName} {LastName}";
        }
        public int Age { get; set; }
        public bool IsOld()
        {
            var result = false;
            if (Age == 0)
            {
                throw new ArgumentNullException(nameof(Age));
            }
            result = Age > 70;
            return result;
        }

    }
}
