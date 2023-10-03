using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private SpawnerManager spawnerManager;
        // Start is called before the first frame update
        void Start()
        {
            spawnerManager.StartSystem();
        }
    }
}
