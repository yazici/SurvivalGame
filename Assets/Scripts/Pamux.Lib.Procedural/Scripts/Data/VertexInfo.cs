using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pamux.Lib.Procedural.Data
{
    public struct VertexInfo
    {
        public readonly int x;
        public readonly int y;
        private readonly int skipIncrement;
        private readonly float[,] heightMap;
        private int[,] vertexIndicesMap;

        public static TerrainSettings terrainSettings;
        public static TerrainSettings TerrainSettings
        {
            get {
                return terrainSettings;
            }
            set
            {
                if (terrainSettings == value)
                {
                    return;
                }
                terrainSettings = value;

                numVertsPerLine = terrainSettings.meshSettings.numVertsPerLine;
            }
        }

        private static int numVertsPerLine;

        public bool isSkipped => x > 2 && x < numVertsPerLine - 3 && y > 2 && y < numVertsPerLine - 3 && ((x - 2) % skipIncrement != 0 || (y - 2) % skipIncrement != 0);
        public bool isOutOfMesh => x == 0 || x == numVertsPerLine - 1 || y == 0 || y == numVertsPerLine - 1;

        public bool isMeshEdge => !isOutOfMesh && (y == 1 || y == numVertsPerLine - 2 || x == 1 || x == numVertsPerLine - 2);

        public bool isMain => (x - 2) % skipIncrement == 0 && (y - 2) % skipIncrement == 0 && !isOutOfMesh && !isMeshEdge;

        public bool isEdgeConnection => (y == 2 || y == numVertsPerLine - 3 || x == 2 || x == numVertsPerLine - 3) && !isOutOfMesh && !isMeshEdge && !isMain;

        public bool isVertical;

        public int dstToMainVertexA;
        public int dstToMainVertexB;
        public float dstPercentFromAToB;


        public Coord coordA => new Coord((isVertical) ? x : x - dstToMainVertexA, (isVertical) ? y - dstToMainVertexA : y);
        public Coord coordB => new Coord((isVertical) ? x : x + dstToMainVertexB, (isVertical) ? y + dstToMainVertexB : y);

        public float heightMainVertexA => heightMap[coordA.x, coordA.y];
        public float heightMainVertexB => heightMap[coordB.x, coordB.y];

        public float height =>
            isEdgeConnection
                ? heightMainVertexA * (1 - dstPercentFromAToB) + heightMainVertexB * dstPercentFromAToB
                : heightMap[x, y];


        public bool shouldCreateTriangle => x < numVertsPerLine - 1 && y < numVertsPerLine - 1 && (!isEdgeConnection || (x != 2 && y != 2));
        public int VertexIndex => vertexIndicesMap[x, y];
        public int VertexIndexCoordA => vertexIndicesMap[coordA.x, coordA.y];
        public int VertexIndexCoordB => vertexIndicesMap[coordB.x, coordB.y];

        public int[] Corners
        {
            get
            { 
                var si = (isMain && x != numVertsPerLine - 3 && y != numVertsPerLine - 3)
                            ? skipIncrement
                            : 1;

                return new int[] {
                    vertexIndicesMap[x, y],
                    vertexIndicesMap[x + si, y],
                    vertexIndicesMap[x, y + si],
                    vertexIndicesMap[x + si, y + si]
                };
            }
        }

        
        public VertexInfo(int x, int y, int skipIncrement, float[,] heightMap, int[,] vertexIndicesMap)
        {
            this.x = x;
            this.y = y;
            this.skipIncrement = skipIncrement;
            this.heightMap = heightMap;
            this.vertexIndicesMap = vertexIndicesMap;

            this.isVertical = x == 2 || x == numVertsPerLine - 3;

            this.dstToMainVertexA = (isVertical ? y - 2 : x - 2) % this.skipIncrement;
            this.dstToMainVertexB = this.skipIncrement - this.dstToMainVertexA;
            this.dstPercentFromAToB = this.dstToMainVertexA / (float)this.skipIncrement;
        }

        
    }
}