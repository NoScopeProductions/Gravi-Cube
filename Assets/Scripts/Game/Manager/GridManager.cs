using System.Collections.Generic;
using Game;
using JetBrains.Annotations;
using UnityEngine;

namespace Manager
{
    [UsedImplicitly]
    public class GridManager : MonoBehaviour
    {
        [SerializeField, UsedImplicitly] 
        private GameObject _cubePrefab;

        [SerializeField, UsedImplicitly] 
        private int _gridWidth;

        [SerializeField, UsedImplicitly]
        private int _gridHeight;

        public List<GameObject> Grid { get; private set; }

        [UsedImplicitly]
        void Awake ()
        {
            Grid = new List<GameObject>();
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
        }

        private void CreateCube(int i, int j)
        {
            var cube = ((GameObject)Instantiate(_cubePrefab, new Vector3(i * 3f, 0f, j * 3f), Quaternion.identity)).GetComponent<Cube>();
            cube.transform.parent = transform;
            cube.IsEdgeCube = CreatingEdgeCube(i, j);
            Grid.Add(cube.gameObject);
        }

        private bool CreatingEdgeCube(int i, int j)
        {
            return i == 0 || j == 0 ||
                   i == _gridWidth-1 ||
                   j == _gridHeight-1;
        }
    }
}
