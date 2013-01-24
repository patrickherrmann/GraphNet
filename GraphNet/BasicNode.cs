namespace GraphNet
{
    public class BasicNode : DotElement, IGraphNode
    {
        public string Name { get; private set; }

        public BasicNode(string name)
        {
            Name = name;
            AddStyle("label", name);
        }

        public string GetDotMarkup()
        {
            return this + GetStyles() + ";";
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
