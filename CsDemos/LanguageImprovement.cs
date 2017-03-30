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
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string FullName()
        {
            return String.Format("{0} {1}", lastName, firstName);
        }
        public int Age { get; set; }
        public bool IsOld()
        {
            var result = false;
            if (Age == 0)
            {
                throw new ArgumentNullException("Age");
            }
            if (Age > 70)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}
