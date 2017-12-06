using System;

[Serializable]
public class LevelArray<T>
{
    public T[] Datas;

    public LevelArray(T[] datas)
    {
        Datas = datas;
    }

    public T this[int index]
    {
        get { return Datas[index - 1]; }
        set { Datas[index - 1] = value; }
    }

    public int Length { get { return Datas.Length; } }
}
