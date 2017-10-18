using System;
using System.ComponentModel;

namespace LanguageFeaturesCS
{
    public class UnorderedClass
    {
        public void SampleMethod() {
            AnotherMethod();
        }

        public string SomeProperty
        {
            get;
            set;
        }

        private readonly string aRandomField = "Unknown";

        private void AnotherMethod() {
        }

        public string FirstName { get; set; } = "Jim";

        public string LastName { get; set; } = "Wooley";

        public UnorderedClass() { }

        public string FullName()
        {
            return $"{FirstName} {LastName ?? aRandomField}";
        }

        public event EventHandler PropertyChanged;

        public UnorderedClass(string first, string last)
        {
            FirstName = first;
            LastName = last;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FullName)));
        }
    }
}
