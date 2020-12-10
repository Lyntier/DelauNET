using DelauNET.Model;
using UnityEngine;

namespace DelauNET.Unity
{
    public static class TypeExtensions
    {
        public static Vertex AsVertex(this Vector2 vector)
        {
            return new Vertex(vector.x, vector.y);
        }

        public static Vector2 AsVector(this Vertex vertex)
        {
            return new Vector2(vertex.X, vertex.Y);
        }
    }
}