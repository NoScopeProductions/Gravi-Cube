using JetBrains.Annotations;
using Manager;
using UnityEngine;

namespace Game
{
    [UsedImplicitly]
    public class Cube : MonoBehaviour
    {
        private Renderer _renderer;

        private int _health;
        private GridManager _gridManager;

        public bool IsEdgeCube { private get; set; }

        [UsedImplicitly]
        void Start ()
        {
            _health = IsEdgeCube ? 1 : 3;
            _renderer = GetComponent<Renderer>();
            _gridManager = GameObject.Find("GridManager").GetComponent<GridManager>();

            AdjustCubeColor();
        }

        public void AddHealth()
        {
            Mathf.Clamp(++_health, -1, 3);
            AdjustCubeColor();
        }

        private void DecreaseHealth()
        {
            Mathf.Clamp(--_health, -1, 3);

            if (_health <= 0)
            {
                _gridManager.Grid.Remove(gameObject);
                Destroy(gameObject);
            }

            AdjustCubeColor();
        }

        private void AdjustCubeColor()
        {
            switch (_health)
            {
                case -1:
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

        [UsedImplicitly]
        public void OnTriggerExit(Collider col)
        {
            DecreaseHealth();
        }
    }
}
