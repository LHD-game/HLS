using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffButton : MonoBehaviour
{

    public void On(GameObject OnOffObject)
    {
        OnOffObject.SetActive(true);
    }
    public void Off(GameObject OnOffObject)
    {
        OnOffObject.SetActive(false);
    }
}
