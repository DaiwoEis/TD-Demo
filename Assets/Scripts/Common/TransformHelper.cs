using UnityEngine;
using System.Collections.Generic;

public static class TransformHelper
{

    public static Transform FindChildByName(this Transform self, string childName)
    {
        Transform target = self.Find(childName);
        if (target != null) return target;

        foreach (Transform childTrans in self)
        {
            target = FindChildByName(childTrans, childName);
            if (target != null)
                return target;
        }

        return null;
    }

    public static T FindChildComponentByName<T>(this Transform self, string childName) where T : Component
    {
        var childTrans = FindChildByName(self, childName);
        if (childTrans != null)
            return childTrans.GetComponent<T>();

        return null;
    }

    public static void LookPostion(Transform self, Vector3 targetPos, float rotateSpeed)
    {
        Vector3 dir = targetPos - self.position;
        LookDirection(self, dir, rotateSpeed);
    }

    public static void LookDirection(Transform self, Vector3 targetDir, float rotateSpeed)
    {
        if (targetDir == Vector3.zero) return;

        Quaternion dir = Quaternion.LookRotation(targetDir);
        self.rotation = Quaternion.Lerp(self.rotation, dir, Time.deltaTime * rotateSpeed);
    }

    public static Transform[] FindArround(Transform currentTF, float distance, float angle, string[] targetTags)
    {
        List<Transform> list = new List<Transform>();
        foreach (var tag in targetTags)
        {
            GameObject[] tempGOs = GameObject.FindGameObjectsWithTag(tag);
            list.AddRange(tempGOs.Select(o => o.transform));
        }

        list = list.FindAll(tf =>
            Vector3.Distance(tf.position, currentTF.position) <= distance &&
            Vector3.Angle(currentTF.forward, tf.position - currentTF.position) <= angle / 2
        );
        return list.ToArray();
    }

    public static void Reset(this Transform trans)
    {
        trans.localPosition = Vector3.zero;
        trans.localScale = Vector3.one;
        trans.localRotation = Quaternion.identity;
    }
}
