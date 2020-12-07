namespace DelauNET.Model
{
    public struct Circle
    {
        public readonly Vertex Origin;
        public readonly float Radius;

        public Circle(Vertex origin, float radius)
        {
            Origin = origin;
            Radius = radius;
        }

        public override string ToString()
        {
            return $"Origin: {Origin} | Radius: {Radius}";
        }
    }
}