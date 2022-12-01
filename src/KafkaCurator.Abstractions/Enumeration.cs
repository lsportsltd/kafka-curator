namespace KafkaCurator.Abstractions
{
    public abstract class Enumeration
    {
        public string Name { get; }

        protected Enumeration(string name) => Name = name;
        
        public override string ToString() => Name;
    }
}