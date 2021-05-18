using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class HelloWorld : MonoBehaviour
{
    [SerializeField] private string welcomeMsg;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): " + welcomeMsg);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): ...");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): ...");
    }


    private long stayCount = 0;

    private void OnCollisionStay2D(Collision2D collision)
    {
        stayCount++;

        //Debug.Log(MethodBase.GetCurrentMethod().Name + "(): " + stayCount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): ...");
    }

    private void OnApplicationPause(bool pause)
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): ...");
    }

    private void OnApplicationQuit()
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): ...");
    }

    private void OnApplicationFocus(bool focus)
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): ...");
    }

    private void OnBecameInvisible()
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name + 
            "(): destroying this object: " + gameObject.name);

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): ...");
    }

    private void OnDisable()
    {
        Debug.Log(MethodBase.GetCurrentMethod().Name + "(): ...");
    }
}
