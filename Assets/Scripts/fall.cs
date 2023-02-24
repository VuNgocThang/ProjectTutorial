using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fall : MonoBehaviour
{

    [SerializeField] private string name;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            playerController.HeartsHandle();
            playerController.transform.position = new Vector2(35, 0);
        }
    }
}
