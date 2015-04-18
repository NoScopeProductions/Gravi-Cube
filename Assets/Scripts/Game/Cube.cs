using System.Collections.Generic;
using Game.Manager;
using JetBrains.Annotations;
using UnityEngine;

namespace Game
{
    [UsedImplicitly]
    public class Cube : MonoBehaviour
    {
        private Renderer _renderer;
        private GridManager _gridManager;

        public int Health { get; private set; }
        public Vector2 TilePosition { get; set; }
        public List<Cube> Neighbors { get; set; }
        public bool IsEdgeCube { private get; set; }

        [UsedImplicitly]
        void Start ()
        {
            Health = IsEdgeCube ? 1 : 3;
            _renderer = GetComponent<Renderer>();
            _gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
            AdjustCubeColor();
        }

        // YAGNI ???
        /*
        public void AddHealth()
        {
            Mathf.Clamp(++_health, -1, 3);
            AdjustCubeColor();
        }
        */
        public void DecreaseHealth()
        {
            Mathf.Clamp(--Health, 0, 3);

            if (Health <= 0)
            {
                DestroyCube();
                _gridManager.DestroyIslands();
            }

            AdjustCubeColor();
        }

        public void DestroyCube()
        {
            foreach (var neighbor in Neighbors)
            {
                neighbor.Neighbors.Remove(this);
            }

            _gridManager.Grid.Remove(this);
            gameObject.SetActive(false);
        }

        private void AdjustCubeColor()
        {
            switch (_health)
            {
                case  0:
                case  1:
                    _renderer.material.color = Color.red;
                    break;
                case 2:
                    _renderer.material.color = Color.yellow;
                    break;
                case 3:
                    _renderer.material.color = Color.white;
                    break;
                default:
                    _renderer.material.color = Color.white;
                    break;
            }
        }
    }
}
