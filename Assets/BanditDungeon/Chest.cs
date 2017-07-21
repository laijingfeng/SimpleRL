using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    private Text txtValT;
    private Text txtVal;
    private Text txtCnt;

    private float valT;
    private int cnt = 0;

    void Awake()
    {
        txtValT = this.transform.FindChild("ValT").GetComponent<Text>();
        txtVal = this.transform.FindChild("Val").GetComponent<Text>();
        txtCnt = this.transform.FindChild("Cnt").GetComponent<Text>();
    }

    void Start()
    {
        cnt = 0;
        float v = valT * 2 + -1f;
        txtValT.text = string.Format("True:{0}({1})", v.ToString("F2"), valT.ToString("F2"));
        RefreshVal(0);
    }

    public void SetValT(float val)
    {
        valT = val;
    }

    public float Selected()
    {
        return valT > Random.Range(0.0f, 1.0f) ? 1.0f : -1.0f;
    }

    public void RefreshVal(float val)
    {
        txtVal.text = string.Format("Val:{0}", val.ToString("F2"));
        cnt++;
        txtCnt.text = string.Format("Cnt:{0}", cnt.ToString());
    }
}