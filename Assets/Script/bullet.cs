using Game;
using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using UnityEngine;
using UnityEngine.Playables;

public class bullet : MonoBehaviour
{
    [SerializeField] float _speed;

    private Vector3 direction;
    private int dmg;
    
    void Update()
    {
        transform.Translate(direction*Time.deltaTime*_speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "EnemyHitBox")
        {
            var i = collision.GetComponent<HealthProxy>();
            i?.Damage(this.gameObject, dmg);
            gameObject.SetActive(false);
        }

        if(collision.gameObject.name == "Wall")
            gameObject.SetActive(false);   
    }

    private void OnBecameVisible()
    {
        StartCoroutine(wait());
    }

    private void OnBecameInvisible()
    {
        PlayerMunition.SharedInstance.AddBulletBackToInactive(this);
    }

    public void SetDirectionAndDmg(Vector3 direction, int dmg)
    {
        this.direction = direction.normalized;
        this.dmg = dmg;
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(2);

        if(isActiveAndEnabled) 
            this.gameObject.SetActive(false);
    }
}
