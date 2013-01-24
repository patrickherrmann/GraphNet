namespace GraphNet
{
    public class BasicEdge : DotElement, IGraphEdge<BasicNode>
    {
        public BasicNode From { get; private set; }
        public BasicNode To { get; private set; }

        public BasicEdge(BasicNode from, BasicNode to)
        {
            From = from;
            To = to;
        }

        public string GetDotMarkup()
        {
            return this + GetStyles() + ";";
        }

        public override string ToString()
        {
            return From + " -> " + To;
        }
    }
}
