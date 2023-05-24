using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayMamager : MonoBehaviour
{
    LineRenderer linerend;
    RaycastHit2D hit_info;
    [SerializeField]
    float addforce = 50 ;
    
    // Start is called before the first frame update
    void Update()
    {

        hit_info = Physics2D.Raycast(transform.position, transform.up * 10,10);
        Debug.DrawRay(transform.position, transform.up * 10 );
        // collider‚Ì’†g‚ª”ñnull‚È‚çŒğ·—L‚è
        if (hit_info.collider != null&& hit_info.rigidbody != null)
        {
            hit_info.rigidbody.AddForce(transform.up * addforce);
        }
       
    }

}
