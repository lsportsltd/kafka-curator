using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace KafkaCurator.Abstractions
{
    public abstract class Enumeration
    {
        public string Name { get; }

        protected Enumeration(string name) => Name = name;
        
        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
            typeof(T)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => f.GetValue(null))
                .Cast<T>();
    }
}