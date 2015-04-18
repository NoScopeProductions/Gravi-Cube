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
            AnimateFall();
        }

        private void AnimateFall()
        {
            iTween.ShakeRotation(gameObject, new Vector3(10f, 10f, 10f), 2f);

            iTween.MoveBy(gameObject, iTween.Hash("y", -30f,
                "time", 8f,
                "delay", Random.Range(0.1f, 0.75f)
                ));
            iTween.FadeTo(gameObject, 0f, 3f);
        }

        private void AdjustCubeColor()
        {
            switch (Health)
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
