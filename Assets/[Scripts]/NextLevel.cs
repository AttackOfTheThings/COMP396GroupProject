using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public GameObject player;
    private int nextScene;

    // Start is called before the first frame update
    void Start()
    {
        nextScene = SceneManager.GetActiveScene().buildIndex + 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("In Cube");
        if(collision.gameObject == player)
        {
            Debug.Log("In if Statement");
            SceneManager.LoadScene(nextScene);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("In Cube");
        if (other.gameObject == player)
        {
            Debug.Log("In if Statement");
            SceneManager.LoadScene(nextScene);
        }
    }
}
