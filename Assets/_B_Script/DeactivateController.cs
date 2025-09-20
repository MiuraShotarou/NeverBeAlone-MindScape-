using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateController : MonoBehaviour
{
    public void DeactivateThis()
    {
        gameObject.SetActive(false);
    }
}
