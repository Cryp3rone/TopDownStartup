using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using UnityEngine;
using UnityEngine.Playables;

public class bullet : MonoBehaviour
{
    [SerializeField] float _speed = 50f;

    private Vector3 direction;
    private int dmg;
    
    void Update()
    {
        transform.Translate(direction*Time.deltaTime*_speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var i = collision.GetComponentInParent<HealthProxy>();
        i?.Damage(dmg);

        /*HashSet<Health> hh = new HashSet<Health>();

        if(hh.Add(i._target) == false)
        {
            //skip
        }*/

        if(collision.gameObject.name == "Wall")
            Destroy(gameObject);
    }

    private void OnBecameVisible()
    {
        StartCoroutine(wait());
    }

    public void SetDirectionAndDmg(Vector3 direction, int dmg)
    {
        this.direction = direction.normalized;
        this.dmg = dmg;
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(2);

        this.gameObject.SetActive(false);
    }
}
