using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollect : MonoBehaviour
{
    public AudioClip collected;
    public AudioSource audioSource;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //audioSource.PlayOneShot(collected);
            //audioSource.Play();
            AudioSource.PlayClipAtPoint(collected, transform.position);
            player.GetComponent<PlayerManager>().Heal();
            Destroy(this.gameObject);
            //player.GetComponent<PlayerManager>().Heal();
            

        }
    }

}
