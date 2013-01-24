namespace GraphNet
{
    public interface IGraphEdge<out T> where T : IGraphNode
    {
        string GetDotMarkup();
        T From { get; }
        T To { get; }
    }
}