using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Manager
{
    [UsedImplicitly]
    public class GridManager : MonoBehaviour
    {

        public const float CubeScale = 3f;

        [SerializeField, UsedImplicitly] 
        private GameObject _cubePrefab;

        [SerializeField, UsedImplicitly] 
        private int _gridWidth;

        [SerializeField, UsedImplicitly]
        private int _gridHeight;

        public List<Cube> Grid { get; private set; }
        public List<Player> Players { get; private set; }
            
        [UsedImplicitly]
        void Awake ()
        {
            Grid = new List<Cube>();
            Players = new List<Player>();
            CreateGrid();
        }

        private void CreateGrid()
        {
            for (int i = 0; i < _gridWidth; i++)
            {
                for (int j = 0; j < _gridHeight; j++)
                {
                    CreateCube(i, j);
                }
            }

            SetAllCubeNeighbors();
        }

        private void SetAllCubeNeighbors()
        {
            for (int x = 0; x < _gridWidth; x++)
            {
                for (int y = 0; y < _gridHeight; y++)
                {
                    SetNeighborsForCube( GetCubeAtPosition( new Vector2(x, y) ) );
                }
            }
        }

        private void SetNeighborsForCube(Cube cube)
        {
            cube.Neighbors = new List<Cube>();
            if (cube.TilePosition.x > 0)
            {
                cube.Neighbors.Add(GetCubeAtPosition(new Vector2(cube.TilePosition.x - 1, cube.TilePosition.y)));
            }

            if (cube.TilePosition.x < _gridWidth - 1)
            {
                cube.Neighbors.Add(GetCubeAtPosition(new Vector2(cube.TilePosition.x + 1, cube.TilePosition.y)));
            }

            if (cube.TilePosition.y > 0)
            {
                cube.Neighbors.Add(GetCubeAtPosition(new Vector2(cube.TilePosition.x, cube.TilePosition.y - 1)));
            }

            if (cube.TilePosition.y < _gridHeight - 1)
            {
                cube.Neighbors.Add(GetCubeAtPosition(new Vector2(cube.TilePosition.x, cube.TilePosition.y + 1)));
            }
        }

        private void CreateCube(int i, int j)
        {
            var cube = ((GameObject)Instantiate(_cubePrefab, new Vector3(i * 3f, 0f, j * 3f), Quaternion.identity)).GetComponent<Cube>();
            cube.transform.parent = transform;
            cube.TilePosition = new Vector2(i, j);
            cube.IsEdgeCube = CreatingEdgeCube(i, j);
            Grid.Add(cube);


        }

        private bool CreatingEdgeCube(int i, int j)
        {
            return i == 0 || j == 0 ||
                   i == _gridWidth-1 ||
                   j == _gridHeight-1;
        }

        public Cube GetCubeAtPosition(Vector2 tileCoordinates)
        {
            return Grid.Find(cube => cube.TilePosition == tileCoordinates);
        }

        public void DestroyIslands()
        {
            var cubesToDestroy = new List<Cube>();

            foreach (var player in Players)
            {
                cubesToDestroy.AddRange( FindIslandsForPlayer(player) );
            }

            foreach (var cube in cubesToDestroy)
            {
                cube.DestroyCube();
            }
        }

        public Vector2 GetTileCoordinatesFromWorldCoordinates(Vector3 worldPosition)
        {
            return new Vector2( (int) (worldPosition.x/CubeScale),
                                (int) (worldPosition.z/CubeScale)
                              );
        }

        private IEnumerable<Cube> FindIslandsForPlayer(Player player)
        {
            var sourceCube = GetCubeAtPosition(player.TilePosition);

            var reachableCubes = new List<Cube>();
            var cubesToVisit = new Queue<Cube>();
            var visitedCubes = new List<Cube>();

            cubesToVisit.Enqueue(sourceCube);

            while (cubesToVisit.Count > 0)
            {
                var cube = cubesToVisit.Dequeue();
                if (cube == null) continue;

                if (!reachableCubes.Contains(cube))
                {
                    reachableCubes.Add(cube);
                }

                visitedCubes.Add(cube);

                foreach (var neighbor in cube.Neighbors)
                {
                    if (!visitedCubes.Contains(neighbor) && !cubesToVisit.Contains(neighbor))
                    {
                        cubesToVisit.Enqueue(neighbor);
                    }
                }
                
            }

            return Grid.FindAll(c => !reachableCubes.Contains(c));
        }
    }
}
