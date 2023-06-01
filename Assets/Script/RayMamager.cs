using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayMamager : MonoBehaviour
{
    RaycastHit2D _hitInfo;
    [SerializeField]
    float _addForce = 50 ;
    
    // Start is called before the first frame update
    void Update()
    {

    }
    private void FixedUpdate()
    {
        int layerMask = ~LayerMask.GetMask(new string[] { "Field" });
        _hitInfo = Physics2D.Raycast(transform.position, transform.up * 10, 10, layerMask);
        Debug.DrawRay(transform.position, transform.up * 10);
        // colliderÇÃíÜêgÇ™îÒnullÇ»ÇÁåç∑óLÇË
        if (_hitInfo.collider != null && _hitInfo.rigidbody != null)
        {
            //Debug.Log(gameObject.name + $"{transform.up);
            var force = transform.up.normalized * _addForce;
            Debug.Log($"{name}: {force}, {_hitInfo.rigidbody.velocity}");
            _hitInfo.rigidbody.AddForce(force);

        }
    }

}
