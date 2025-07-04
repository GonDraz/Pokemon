using GonDraz.Managers;
using UnityEngine;

namespace Map
{
    public class MapTrigger : MonoBehaviour
    {
        private GameObject _player;

        private void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == _player) EventManager.MapTriggerEnter.Invoke(gameObject.scene.name);
        }
    }
}