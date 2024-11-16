using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    private ShapeGenerator _shapeGenerator;
    public Mesh _mesh;
    private int _resolution;
    private Vector3[] _vertices;
    private Vector3 _localUp;
    private Vector3 _axisA;
    private Vector3 _axisB;


    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        this._shapeGenerator = shapeGenerator;
        this._mesh = mesh;
        this._resolution = resolution;
        this._localUp = localUp;


        _axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        _axisB = Vector3.Cross(localUp, _axisA);

        ShapeGenerator.ClearGeneratedPoints();
    }

    public void ConstructMesh()
    {
        _vertices = new Vector3[_resolution * _resolution];
        int[] triangles = new int[(_resolution - 1) * (_resolution - 1) * 6];
        int triIndex = 0;


        for (int y = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++)
            {
                int i = x + y * _resolution;

                // Definicja punktu na sferze jednostkowej
                Vector2 percent = new Vector2(x, y) / (_resolution - 1);
                Vector3 pointOnUnitCube = _localUp + (percent.x - .5f) * 2 * _axisA + (percent.y - .5f) * 2 * _axisB;

                // Przeskalowanie punktu na sferze jednostkowej, aby uzyskać grubszą warstwę
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                _vertices[i] = _shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere);

                if (x < _resolution - 1 && y < _resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + _resolution + 1;
                    triangles[triIndex + 2] = i + _resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + _resolution + 1;

                    triIndex += 6;
                }
            }
        }

        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();

    }


    public Vector3[] GetVertices()
    {
        return _vertices;
    }
    public int[] GetTriangles()
    {
        return _mesh.triangles;
    }
}
