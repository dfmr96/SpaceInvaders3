using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject pressStart;
    [SerializeField] float counter, delay;

    void Update()
    {
        counter += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(1);
        }

        if (counter > 0.5f)
        {
            delay += Time.deltaTime;
            pressStart.SetActive(true);

            if (delay > 0.5f)
            {
                counter = 0;
                delay = 0;
            }
        }
        else
        {
            pressStart.SetActive(false);
        }
    }
}
