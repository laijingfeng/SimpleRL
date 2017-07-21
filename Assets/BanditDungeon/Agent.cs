using UnityEngine;

public class Agent
{
    private float learning_rate = 0.005f;
    private float confidence = 2.1f;
    private float[] value_table;

    public Agent(int actionSize, bool optimistic)
    {
        value_table = new float[actionSize];
        for (int i = 0; i < actionSize; i++)
        {
            value_table[i] = optimistic ? 1.0f : 0.0f;
        }
    }

    public int PickAction()
    {
        return Util.SoftRandom(value_table, confidence);
    }

    public float UpdatePolicy(int action, float reward)
    {
        value_table[action] += learning_rate * (reward - value_table[action]);
        return value_table[action];
    }
}