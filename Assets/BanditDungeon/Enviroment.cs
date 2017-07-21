using System.Collections;
using System.Collections.Generic;
using Jerry;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    private Transform panel;
    private Transform prefab;

    private Agent agent;
    private List<Chest> chestList = new List<Chest>();
    private int chestNum = 5;
    /// <summary>
    /// 0/1/2
    /// </summary>
    private float diff = 0f;

    void Awake()
    {
        panel = this.transform.FindChild("Panel");
        prefab = this.transform.FindChild("Prefab");
        prefab.gameObject.SetActive(false);
    }

    void Start()
    {
        chestList.Clear();
        JerryUtil.DestroyAllChildren(panel);

        int winner = Random.Range(0, chestNum);
        float adjust = diff * 0.1f;
        for (int i = 0; i < chestNum; i++)
        {
            Chest ch = JerryUtil.CloneGo<Chest>(new JerryUtil.CloneGoData()
            {
                parant = panel,
                prefab = prefab.gameObject,
                active = true,
                clean = false,
            });
            ch.SetValT(winner == i ? Random.Range(0.6f, 1.0f - adjust) : Random.Range(0.0f + adjust, 0.4f));
            chestList.Add(ch);
        }

        agent = new Agent(chestNum, true);

        this.StartCoroutine(IE_DoTrial());
    }

    private IEnumerator IE_DoTrial()
    {
        while (true)
        {
            int action = agent.PickAction();
            float reward = chestList[action].Selected();
            float val = agent.UpdatePolicy(action, reward);
            chestList[action].RefreshVal(val);
            //yield return new WaitForSeconds(0.1f);
            yield return new WaitForEndOfFrame();
        }
    }
}