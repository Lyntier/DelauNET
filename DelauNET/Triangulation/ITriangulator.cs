using System.Collections.Generic;
using DelauNET.Model;

namespace DelauNET.Triangulation
{
    public interface ITriangulator
    {
        /// <summary>
        /// Holds the triangles as a result of a performed triangulation.
        /// </summary>
        IList<Triangle> Triangles { get; }
        
        /// <summary>
        /// Triangulates the given <see cref="Vertex">vertices</see> and returns
        /// the result as a list of <see cref="Triangle">triangles</see>. Also
        /// stores these in the property <see cref="Triangles"/>.
        /// </summary>
        IEnumerable<Triangle> Triangulate(List<Vertex> points);
    }
}