using UnityEngine;
using System.Collections;
using Pamux.Lib.Procedural.Data;
using System.Threading.Tasks;
using System;

namespace Pamux.Lib.Procedural
{
    public class MeshGenerator
    {
        public static MeshGenerator Instance = null;

        private MeshSettings settings;
        private MeshSettings meshSettings=>settings;
        private int numVertsPerLine;
        private int[,] vertexIndicesMap;

        public MeshGenerator(MeshSettings settings)
        {
            if (Instance != null)
            {
                throw new System.Exception($"MeshGenerator is a singleton.");
            }

            Instance = this;

            this.settings = settings;

            numVertsPerLine = settings.numVertsPerLine;
            vertexIndicesMap = new int[numVertsPerLine, numVertsPerLine];
        }

        public MeshData GenerateTerrainMeshOld(float[,] heightMap, int levelOfDetail)
        {

            int skipIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
            int numVertsPerLine = meshSettings.numVertsPerLine;

            Vector2 topLeft = new Vector2(-1, 1) * meshSettings.meshWorldSize / 2f;

            MeshData meshData = new MeshData(numVertsPerLine, skipIncrement, meshSettings.useFlatShading);

            int[,] vertexIndicesMap = new int[numVertsPerLine, numVertsPerLine];
            int meshVertexIndex = 0;
            int outOfMeshVertexIndex = -1;

            for (int y = 0; y < numVertsPerLine; y++)
            {
                for (int x = 0; x < numVertsPerLine; x++)
                {
                    bool isOutOfMeshVertex = y == 0 || y == numVertsPerLine - 1 || x == 0 || x == numVertsPerLine - 1;
                    bool isSkippedVertex = x > 2 && x < numVertsPerLine - 3 && y > 2 && y < numVertsPerLine - 3 && ((x - 2) % skipIncrement != 0 || (y - 2) % skipIncrement != 0);
                    if (isOutOfMeshVertex)
                    {
                        vertexIndicesMap[x, y] = outOfMeshVertexIndex;
                        outOfMeshVertexIndex--;
                    }
                    else if (!isSkippedVertex)
                    {
                        vertexIndicesMap[x, y] = meshVertexIndex;
                        meshVertexIndex++;
                    }
                }
            }

            for (int y = 0; y < numVertsPerLine; y++)
            {
                for (int x = 0; x < numVertsPerLine; x++)
                {
                    bool isSkippedVertex = x > 2 && x < numVertsPerLine - 3 && y > 2 && y < numVertsPerLine - 3 && ((x - 2) % skipIncrement != 0 || (y - 2) % skipIncrement != 0);

                    if (!isSkippedVertex)
                    {
                        bool isOutOfMeshVertex = y == 0 || y == numVertsPerLine - 1 || x == 0 || x == numVertsPerLine - 1;
                        bool isMeshEdgeVertex = (y == 1 || y == numVertsPerLine - 2 || x == 1 || x == numVertsPerLine - 2) && !isOutOfMeshVertex;
                        bool isMainVertex = (x - 2) % skipIncrement == 0 && (y - 2) % skipIncrement == 0 && !isOutOfMeshVertex && !isMeshEdgeVertex;
                        bool isEdgeConnectionVertex = (y == 2 || y == numVertsPerLine - 3 || x == 2 || x == numVertsPerLine - 3) && !isOutOfMeshVertex && !isMeshEdgeVertex && !isMainVertex;

                        int vertexIndex = vertexIndicesMap[x, y];
                        Vector2 percent = new Vector2(x - 1, y - 1) / (numVertsPerLine - 3);
                        Vector2 vertexPosition2D = topLeft + new Vector2(percent.x, -percent.y) * meshSettings.meshWorldSize;
                        float height = heightMap[x, y];

                        if (isEdgeConnectionVertex)
                        {
                            bool isVertical = x == 2 || x == numVertsPerLine - 3;
                            int dstToMainVertexA = ((isVertical) ? y - 2 : x - 2) % skipIncrement;
                            int dstToMainVertexB = skipIncrement - dstToMainVertexA;
                            float dstPercentFromAToB = dstToMainVertexA / (float)skipIncrement;

                            Coord coordA = new Coord((isVertical) ? x : x - dstToMainVertexA, (isVertical) ? y - dstToMainVertexA : y);
                            Coord coordB = new Coord((isVertical) ? x : x + dstToMainVertexB, (isVertical) ? y + dstToMainVertexB : y);

                            float heightMainVertexA = heightMap[coordA.x, coordA.y];
                            float heightMainVertexB = heightMap[coordB.x, coordB.y];

                            height = heightMainVertexA * (1 - dstPercentFromAToB) + heightMainVertexB * dstPercentFromAToB;

                            EdgeConnectionVertexData edgeConnectionVertexData = new EdgeConnectionVertexData(vertexIndex, vertexIndicesMap[coordA.x, coordA.y], vertexIndicesMap[coordB.x, coordB.y], dstPercentFromAToB);
                            meshData.DeclareEdgeConnectionVertex(edgeConnectionVertexData);
                        }

                        meshData.AddVertex(new Vector3(vertexPosition2D.x, height, vertexPosition2D.y), percent, vertexIndex);

                        bool createTriangle = x < numVertsPerLine - 1 && y < numVertsPerLine - 1 && (!isEdgeConnectionVertex || (x != 2 && y != 2));

                        if (createTriangle)
                        {
                            int currentIncrement = (isMainVertex && x != numVertsPerLine - 3 && y != numVertsPerLine - 3) ? skipIncrement : 1;

                            int a = vertexIndicesMap[x, y];
                            int b = vertexIndicesMap[x + currentIncrement, y];
                            int c = vertexIndicesMap[x, y + currentIncrement];
                            int d = vertexIndicesMap[x + currentIncrement, y + currentIncrement];
                            meshData.AddTriangle(a, d, c);
                            meshData.AddTriangle(d, a, b);
                        }
                    }
                }
            }

            meshData.ProcessMesh();

            return meshData;
        }

        private int[,] InitializeVertexIndicesMap(int skipIncrement)
        {
            var numVertsPerLine = meshSettings.numVertsPerLine;

            int[,] vertexIndicesMap = new int[numVertsPerLine, numVertsPerLine];
            int meshVertexIndex = 0;
            int outOfMeshVertexIndex = -1;

            for (int y = 0; y < numVertsPerLine; y++)
            {
                for (int x = 0; x < numVertsPerLine; x++)
                {
                    bool isOutOfMeshVertex = y == 0 || y == numVertsPerLine - 1 || x == 0 || x == numVertsPerLine - 1;
                    bool isSkippedVertex = x > 2 && x < numVertsPerLine - 3 && y > 2 && y < numVertsPerLine - 3 && ((x - 2) % skipIncrement != 0 || (y - 2) % skipIncrement != 0);
                    if (isOutOfMeshVertex)
                    {
                        vertexIndicesMap[x, y] = outOfMeshVertexIndex;
                        outOfMeshVertexIndex--;
                    }
                    else if (!isSkippedVertex)
                    {
                        vertexIndicesMap[x, y] = meshVertexIndex;
                        meshVertexIndex++;
                    }
                }
            }
            return vertexIndicesMap;
        }

        private int[,] InitializeVertexIndicesMapNew(int skipIncrement)
        {
            var numVertsPerLine = meshSettings.numVertsPerLine;

            int[,] vertexIndicesMap = new int[numVertsPerLine, numVertsPerLine];
            int meshVertexIndex = 0;
            int outOfMeshVertexIndex = -1;

            for (var y = 0; y < numVertsPerLine; ++y)
            {
                for (var x = 0; x < numVertsPerLine; ++x)
                {
                    var v = new VertexInfo(x, y, skipIncrement, null, null);

                    bool isOutOfMeshVertex = y == 0 || y == numVertsPerLine - 1 || x == 0 || x == numVertsPerLine - 1;
                    bool isSkippedVertex = x > 2 && x < numVertsPerLine - 3 && y > 2 && y < numVertsPerLine - 3 && ((x - 2) % skipIncrement != 0 || (y - 2) % skipIncrement != 0);
                    if (isOutOfMeshVertex)
                    {
                        vertexIndicesMap[x, y] = outOfMeshVertexIndex;
                        outOfMeshVertexIndex--;
                        continue;
                    }

                    if (!isSkippedVertex)
                    {
                        vertexIndicesMap[x, y] = meshVertexIndex;
                        meshVertexIndex++;
                    }

                    /*var v = new VertexInfo(x, y, skipIncrement, null, vertexIndicesMap);

                    if (v.isOutOfMesh)
                    {
                        vertexIndicesMap[x, y] = outOfMeshVertexIndex;
                        --outOfMeshVertexIndex;
                        continue;
                    }

                    if (!v.isSkipped)
                    {
                        vertexIndicesMap[x, y] = meshVertexIndex;
                        ++meshVertexIndex;
                    }*/
                }
            }

            return vertexIndicesMap;
        }

        public MeshData GenerateTerrainMesh(float[,] heightMap, int levelOfDetail)
        {
            int skipIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
            var vertexIndicesMap = InitializeVertexIndicesMapNew(skipIncrement);

            int numVertsPerLine = meshSettings.numVertsPerLine;

            Vector2 topLeft = new Vector2(-1, 1) * meshSettings.meshWorldSize / 2f;

            MeshData meshData = new MeshData(numVertsPerLine, skipIncrement, meshSettings.useFlatShading);

            

            for (int y = 0; y < numVertsPerLine; y++)
            {
                for (int x = 0; x < numVertsPerLine; x++)
                {
                    bool isSkippedVertex = x > 2 && x < numVertsPerLine - 3 && y > 2 && y < numVertsPerLine - 3 && ((x - 2) % skipIncrement != 0 || (y - 2) % skipIncrement != 0);

                    if (!isSkippedVertex)
                    {
                        bool isOutOfMeshVertex = y == 0 || y == numVertsPerLine - 1 || x == 0 || x == numVertsPerLine - 1;
                        bool isMeshEdgeVertex = (y == 1 || y == numVertsPerLine - 2 || x == 1 || x == numVertsPerLine - 2) && !isOutOfMeshVertex;
                        bool isMainVertex = (x - 2) % skipIncrement == 0 && (y - 2) % skipIncrement == 0 && !isOutOfMeshVertex && !isMeshEdgeVertex;
                        bool isEdgeConnectionVertex = (y == 2 || y == numVertsPerLine - 3 || x == 2 || x == numVertsPerLine - 3) && !isOutOfMeshVertex && !isMeshEdgeVertex && !isMainVertex;

                        int vertexIndex = vertexIndicesMap[x, y];
                        Vector2 percent = new Vector2(x - 1, y - 1) / (numVertsPerLine - 3);
                        Vector2 vertexPosition2D = topLeft + new Vector2(percent.x, -percent.y) * meshSettings.meshWorldSize;
                        float height = heightMap[x, y];

                        if (isEdgeConnectionVertex)
                        {
                            bool isVertical = x == 2 || x == numVertsPerLine - 3;
                            int dstToMainVertexA = ((isVertical) ? y - 2 : x - 2) % skipIncrement;
                            int dstToMainVertexB = skipIncrement - dstToMainVertexA;
                            float dstPercentFromAToB = dstToMainVertexA / (float)skipIncrement;

                            Coord coordA = new Coord((isVertical) ? x : x - dstToMainVertexA, (isVertical) ? y - dstToMainVertexA : y);
                            Coord coordB = new Coord((isVertical) ? x : x + dstToMainVertexB, (isVertical) ? y + dstToMainVertexB : y);

                            float heightMainVertexA = heightMap[coordA.x, coordA.y];
                            float heightMainVertexB = heightMap[coordB.x, coordB.y];

                            height = heightMainVertexA * (1 - dstPercentFromAToB) + heightMainVertexB * dstPercentFromAToB;

                            EdgeConnectionVertexData edgeConnectionVertexData = new EdgeConnectionVertexData(vertexIndex, vertexIndicesMap[coordA.x, coordA.y], vertexIndicesMap[coordB.x, coordB.y], dstPercentFromAToB);
                            meshData.DeclareEdgeConnectionVertex(edgeConnectionVertexData);
                        }

                        meshData.AddVertex(new Vector3(vertexPosition2D.x, height, vertexPosition2D.y), percent, vertexIndex);

                        bool createTriangle = x < numVertsPerLine - 1 && y < numVertsPerLine - 1 && (!isEdgeConnectionVertex || (x != 2 && y != 2));

                        if (createTriangle)
                        {
                            int currentIncrement = (isMainVertex && x != numVertsPerLine - 3 && y != numVertsPerLine - 3) ? skipIncrement : 1;

                            int a = vertexIndicesMap[x, y];
                            int b = vertexIndicesMap[x + currentIncrement, y];
                            int c = vertexIndicesMap[x, y + currentIncrement];
                            int d = vertexIndicesMap[x + currentIncrement, y + currentIncrement];
                            meshData.AddTriangle(a, d, c);
                            meshData.AddTriangle(d, a, b);
                        }
                    }
                }
            }

            meshData.ProcessMesh();

            return meshData;
        }



        public Task<MeshData> GenerateTerrainMeshNew(float[,] heightMap, int levelOfDetail)
        {
            int skipIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
            var vertexIndicesMap = InitializeVertexIndicesMap(skipIncrement);

            var meshData = new MeshData(numVertsPerLine, skipIncrement, settings.useFlatShading);

            var meshVertexIndex = 0;
            var outOfMeshVertexIndex = -1;

            

            for (var y = 0; y < numVertsPerLine; ++y)
            {
                for (var x = 0; x < numVertsPerLine; ++x)
                {
                    var v = new VertexInfo(x, y, skipIncrement, heightMap, vertexIndicesMap);
                    if (v.isSkipped)
                    {
                        continue;
                    }

                    var vertexIndex = v.VertexIndex;
                    var percent = new Vector2(x - 1, y - 1) / (numVertsPerLine - 3);
                    var vertexPosition2D = settings.TopLeft + new Vector2(percent.x, -percent.y) * settings.meshWorldSize;

                   if (v.isEdgeConnection)
                   {
                        var edgeConnectionVertexData = new EdgeConnectionVertexData(vertexIndex, v.VertexIndexCoordA, v.VertexIndexCoordB, v.dstPercentFromAToB);
                        meshData.DeclareEdgeConnectionVertex(edgeConnectionVertexData);
                   }

                    try { 
                        meshData.AddVertex(new Vector3(vertexPosition2D.x, v.height, vertexPosition2D.y), percent, vertexIndex);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        //Debug.Log(vertexIndex);
                    }

                    if (!v.shouldCreateTriangle)
                    {
                        continue;
                    }

                    int[] corners = v.Corners;
                   
                    meshData.AddTriangle(corners[0], corners[3], corners[2]);
                    meshData.AddTriangle(corners[3], corners[0], corners[1]);
                }
            }

            meshData.ProcessMesh();

            return Task.FromResult(meshData);
        }
    }
}