using UnityEditor;
using UnityEngine;

public class FindBulletShooterInstances : EditorWindow
{
    [MenuItem("Tools/Find BulletShooter Instances")]
    public static void ShowWindow()
    {
        GetWindow<FindBulletShooterInstances>("Find BulletShooter Instances");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Find All BulletShooter Instances"))
        {
            FindInstances();
        }
    }

    void FindInstances()
    {
        BulletShooter[] instances = FindObjectsOfType<BulletShooter>();
        foreach (BulletShooter instance in instances)
        {
            Debug.Log("Found BulletShooter on: " + instance.gameObject.name, instance.gameObject);
        }
    }
}
