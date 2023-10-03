using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PoolFunction : MonoBehaviour, IPoolable
    {
        public void Activate()
        {
            Debug.Log("Activate");
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            Debug.Log("Deactivate");
            gameObject.SetActive(false);
        }

        public void Destroy()
        {
            Debug.Log("Destroy");
            Destroy(gameObject);
        }
    }
}
