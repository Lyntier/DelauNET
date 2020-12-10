using System.Collections.Generic;
using DelauNET.Model;

namespace DelauNET.Triangulation
{
    public interface ITriangulator
    {
        bool Initialize(ICollection<Vertex> vertices, float tolerance = 0.0001f);
        void ProcessAll();
    }
}