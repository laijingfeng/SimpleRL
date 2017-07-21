using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class QLearing : MonoBehaviour
{
    private Text info;

    public float[,] R = new float[6, 6] 
    {
        {-1, -1, -1, -1, 0, -1},
        {-1, -1, -1, 0, -1, 100},
        {-1, -1, -1, 0, -1, -1},
        {-1, 0, 0, -1, 0, -1},
        {0, -1, -1, 0, -1, 100},
        {-1, 0, -1, -1, 0, 100},
    };

    public float[,] Q = new float[6, 6] 
    {
        {0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0},
    };

    void Awake()
    {
        info = this.transform.FindChild("Text").GetComponent<Text>();
        this.StartCoroutine(Episode());
    }

    private IEnumerator Episode()
    {
        while (true)
        {
            yield return StartCoroutine(OneEpisode());
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator OneEpisode()
    {
        info.text = "";
        for (int i = 0; i < 5; i++)
        {
            AddInfo(i);
            yield return new WaitForSeconds(1f);
        }
    }

    private void AddInfo(int id)
    {
        if (string.IsNullOrEmpty(info.text))
        {
            info.text += id.ToString();
        }
        else
        {
            info.text += string.Format("->{0}", id);
        }
    }
}