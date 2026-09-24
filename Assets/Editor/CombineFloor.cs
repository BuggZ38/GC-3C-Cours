using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CombineFloor : EditorWindow
{
    [MenuItem("Tools/Dungeon/Combine Selected Floor")]
    public static void Combine()
    {
        GameObject floor = Selection.activeGameObject;

        if (floor == null)
        {
            Debug.LogError("Sélectionne ton Floor.");
            return;
        }

        MeshFilter[] meshFilters = floor.GetComponentsInChildren<MeshFilter>();

        if (meshFilters.Length == 0)
        {
            Debug.LogError("Aucun MeshFilter trouvé dans Floor.");
            return;
        }

        List<CombineInstance> combines = new List<CombineInstance>();
        List<Material> materials = new List<Material>();

        foreach (MeshFilter mf in meshFilters)
        {
            if (mf.sharedMesh == null)
                continue;

            MeshRenderer renderer = mf.GetComponent<MeshRenderer>();

            if (renderer == null)
                continue;

            Material[] mats = renderer.sharedMaterials;

            for (int subMesh = 0; subMesh < mf.sharedMesh.subMeshCount; subMesh++)
            {
                CombineInstance combine = new CombineInstance();

                combine.mesh = mf.sharedMesh;
                combine.subMeshIndex = subMesh;

                combine.transform =
                    floor.transform.worldToLocalMatrix *
                    mf.transform.localToWorldMatrix;

                combines.Add(combine);

                if (subMesh < mats.Length)
                    materials.Add(mats[subMesh]);
                else
                    materials.Add(null);
            }
        }

        Mesh combinedMesh = new Mesh();

        combinedMesh.name = floor.name + "_Combined";

        combinedMesh.indexFormat =
            UnityEngine.Rendering.IndexFormat.UInt32;

        combinedMesh.CombineMeshes(
            combines.ToArray(),
            true,
            true
        );

        GameObject combinedObject =
            new GameObject(floor.name + "_Combined");

        combinedObject.transform.SetParent(floor.transform.parent);

        combinedObject.transform.position = Vector3.zero;
        combinedObject.transform.rotation = Quaternion.identity;
        combinedObject.transform.localScale = Vector3.one;

        MeshFilter combinedFilter =
            combinedObject.AddComponent<MeshFilter>();

        MeshRenderer combinedRenderer =
            combinedObject.AddComponent<MeshRenderer>();

        combinedFilter.sharedMesh = combinedMesh;

        combinedRenderer.sharedMaterials =
            materials.ToArray();

        // Sauvegarde du mesh
        string path =
            "Assets/" + combinedObject.name + ".asset";

        AssetDatabase.CreateAsset(combinedMesh, path);
        AssetDatabase.SaveAssets();

        // Supprime les anciens objets
        Undo.DestroyObjectImmediate(floor);

        Debug.Log(
            "Floor fusionné avec succès : " +
            combinedObject.name
        );
    }
}