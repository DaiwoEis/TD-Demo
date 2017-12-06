using System;

public static class TypeExtension  
{
    public static string RemoveNameSpace(this Type type)
    {
        string[] splitsStr = type.ToString().Split('.');
        return splitsStr[splitsStr.Length - 1];
    }
	
}
