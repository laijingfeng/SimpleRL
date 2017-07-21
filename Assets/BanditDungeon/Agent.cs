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
        float[] probs = SoftMax(value_table, confidence);
        float cumulative = 0.0f;
        int selectedElement = 0;
        float diceRoll = Random.Range(0f, 1f);
        for (int i = 0; i < probs.Length; i++)
        {
            cumulative += probs[i];
            if (diceRoll < cumulative)
            {
                selectedElement = i;
                break;
            }
        }
        return selectedElement;
    }

    public float UpdatePolicy(int action, float reward)
    {
        value_table[action] += learning_rate * (reward - value_table[action]);
        return value_table[action];
    }

    private float[] SoftMax(float[] values, float temp)
    {
        float[] softmax_values = new float[values.Length];
        float[] exp_values = new float[values.Length];
        float sum = 0f;
        for (int i = 0; i < values.Length; i++)
        {
            exp_values[i] = Mathf.Exp(values[i] / temp);
            sum += exp_values[i];
        }

        for (int i = 0; i < values.Length; i++)
        {
            softmax_values[i] = exp_values[i] / sum;
        }
        return softmax_values;
    }
}