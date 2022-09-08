using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deleteNPC : MonoBehaviour
{
    private void Update()
    {
        if (this.transform.position.x >= 45)
            Destroy(gameObject);
    }
}
