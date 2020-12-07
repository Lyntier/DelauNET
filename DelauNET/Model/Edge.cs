using System;
using System.Reflection;

namespace DelauNET.Model
{
    public struct Edge
    {
        public readonly Vertex P1, P2;

        public Edge(Vertex p1, Vertex p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public Vertex Center
        {
            get
            {
                var centerX = (P1.X + P2.X) / 2f;
                var centerY = (P1.Y + P2.Y) / 2f;
                return new Vertex(centerX, centerY);
            }
        }

        public Edge Bisector
        {
            get
            {
                if (Math.Abs(P1.X - P2.X) < float.Epsilon)
                {
                    return new Edge(Center, new Vertex(1, 0) );
                } 
                if (Math.Abs(P1.Y - P2.Y) < float.Epsilon)
                {
                    return new Edge(Center, new Vertex(0, 1));
                }
                
                float slope = (P2.Y - P1.Y) / (P2.X - P1.X);
                slope = 0f - 1f / slope;
                return new Edge(Center, new Vertex(1, slope));
            }
        }

        public override string ToString()
        {
            return $"Edge: {P1.ToString()} ==> {P2.ToString()}";
        }
    }
}