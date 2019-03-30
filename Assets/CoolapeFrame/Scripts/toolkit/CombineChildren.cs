/*
******************************************************************************** 
  *Copyright(C),coolae.net 
  *Author:  chenbin
  *Version:  2.0 
  *Date:  2017-01-09
  *Description:  合并mesh，达到合并drawcall目的
  *Others:  
  *History:
*********************************************************************************
*/

using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif
/*
Attach this script as a parent to some game objects. The script will then combine the meshes at startup.
This is useful as a performance optimization since it is faster to render one big mesh than many small meshes. See the docs on graphics performance optimization for more info.
Different materials will cause multiple meshes to be created, thus it is useful to share as many textures/material as you can.
*/
namespace Coolape
{
    [AddComponentMenu("Mesh/Combine Children")]
    public class CombineChildren : MonoBehaviour
    {

        /// Usually rendering with triangle strips is faster.
        /// However when combining objects with very low triangle counts, it can be faster to use triangles.
        /// Best is to try out which value is faster in practice.
        public bool generateTriangleStrips = false;
        public bool applyOnstart = false;
        public bool iscombined = false;
        public UnityEngine.Rendering.ShadowCastingMode MeshRenderCastShadow = UnityEngine.Rendering.ShadowCastingMode.Off;
        public bool isMeshRenderReciveShadow = false;
        public bool useSharedMesh = true;
        public bool meshKeyWithName = true;
        public static Hashtable __meshMap = new Hashtable();
        Transform tr;

        public Transform transform
        {
            get
            {
                if (tr == null)
                {
                    tr = gameObject.transform;
                }
                return tr;
            }
        }


        /// This option has a far longer preprocessing time at startup but leads to better runtime performance.
        void Start()
        {
            if (applyOnstart)
            {
                combineChildren();
            }
        }

        Component[] filters = null;
        //合并
        public void combineChildren(bool onlyCreateMesh = false)
        {
            if (iscombined)
                return;
            if (!onlyCreateMesh)
            {
                iscombined = true;
            }
            filters = GetComponentsInChildren(typeof(MeshFilter));
            Matrix4x4 myTransform = transform.worldToLocalMatrix;
            Hashtable materialToMesh = new Hashtable();

            Hashtable materialMap = new Hashtable();

            for (int i = 0; i < filters.Length; i++)
            {
                MeshFilter filter = (MeshFilter)filters[i];
                Renderer curRenderer = filters[i].GetComponent<Renderer>();
                MeshCombineUtility.MeshInstance instance = new MeshCombineUtility.MeshInstance();
                instance.mesh = filter.sharedMesh;

                if (curRenderer != null && curRenderer.gameObject.activeInHierarchy
                    && curRenderer.gameObject.activeSelf
                    && curRenderer.enabled && instance.mesh != null)
                {
                    instance.transform = myTransform * filter.transform.localToWorldMatrix;

                    Material[] materials = curRenderer.sharedMaterials;
                    for (int m = 0; m < materials.Length; m++)
                    {
                        if (materials[m] == null)
                            continue;
                        instance.subMeshIndex = System.Math.Min(m, instance.mesh.subMeshCount - 1);

                        //					ArrayList objects = (ArrayList)materialToMesh[materials[m]];
                        //					ArrayList objects = (ArrayList)(materialMap[materials[m].mainTexture.GetInstanceID()]);
                        ArrayList objects = (ArrayList)(materialMap[materials[m].name]);
                        if (objects != null)
                        {
                            objects.Add(instance);
                        }
                        else
                        {
                            objects = new ArrayList();
                            objects.Add(instance);
                            //						materialMap[materials[m].mainTexture.GetInstanceID()] = objects;
                            materialMap[materials[m].name] = objects;
                            materialToMesh.Add(materials[m], objects);
                        }
                    }
                    if (!onlyCreateMesh)
                    {
                        curRenderer.enabled = false;
                    }
                }
            }

            foreach (DictionaryEntry de in materialToMesh)
            {
                ArrayList elements = (ArrayList)de.Value;
                MeshCombineUtility.MeshInstance[] instances = (MeshCombineUtility.MeshInstance[])elements.ToArray(typeof(MeshCombineUtility.MeshInstance));
                // We have a maximum of one material, so just attach the mesh to our own game object
                if (materialToMesh.Count == 1)
                {
                    string meshKey = de.Key.GetHashCode().ToString();
                    if (meshKeyWithName) {
                        meshKey +=  "_" + gameObject.name;
                    }
                    Mesh mesh = __meshMap[meshKey] as Mesh;
                    if (mesh == null || !useSharedMesh)
                    {
                        mesh = MeshCombineUtility.Combine(instances, generateTriangleStrips);
                        __meshMap[meshKey] = mesh;
                    }

                    if (onlyCreateMesh)
                    {
                        saveMesh(gameObject.name, mesh);
                    }
                    else
                    {
                        //					mesh = setuv2(mesh);
                        // Make sure we have a mesh filter & renderer
                        MeshFilter filter = (MeshFilter)GetComponent(typeof(MeshFilter));
                        if (filter == null)
                            filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;

                        MeshRenderer newrender = gameObject.GetComponent<MeshRenderer>();
                        if (newrender == null)
                            newrender = gameObject.AddComponent<MeshRenderer>();

                        filter.mesh = mesh;
                        newrender.material = (Material)de.Key;
                        //					renderer.material.shader = Shader.Find("Unlit/Transparent Cutout");
                        newrender.enabled = true;
                        newrender.shadowCastingMode = MeshRenderCastShadow;
                        newrender.receiveShadows = isMeshRenderReciveShadow;
                    }
                }
                // We have multiple materials to take care of, build one mesh / gameobject for each material
                // and parent it to this object
                else
                {

                    string meshKey = de.Key.GetHashCode().ToString();
                    if (meshKeyWithName)
                    {
                        meshKey += "_" + gameObject.name;
                    }
                    Mesh mesh = __meshMap[meshKey] as Mesh;
                    if (mesh == null || !useSharedMesh)
                    {
                        mesh = MeshCombineUtility.Combine(instances, generateTriangleStrips);
                        __meshMap[meshKey] = mesh;
                    }
                    if (onlyCreateMesh)
                    {
                        saveMesh(gameObject.name, mesh);
                    }
                    else
                    {
                        //					mesh = setuv2(mesh);
                        GameObject go = new GameObject("Combined mesh");
                        go.transform.parent = transform;
                        go.transform.localScale = Vector3.one;
                        go.transform.localRotation = Quaternion.identity;
                        go.transform.localPosition = Vector3.zero;
                        MeshFilter filter = go.AddComponent(typeof(MeshFilter)) as MeshFilter;
                        MeshRenderer newrender = go.AddComponent<MeshRenderer>();
                        newrender.material = (Material)de.Key;
                        newrender.shadowCastingMode = MeshRenderCastShadow;
                        newrender.receiveShadows = isMeshRenderReciveShadow;
                        //					renderer.material.shader = Shader.Find("Unlit/Transparent Cutout");
                        filter.mesh = mesh;
                        newCombineds.Add(go);
                    }
                }
            }
        }
        //	[SerializeField]
        //	Vector2[] combineUv2;
        void saveMesh(string name, Mesh mesh)
        {
#if UNITY_EDITOR
            string dir = Application.dataPath;

            string path = EditorUtility.SaveFilePanel("Save Mesh", dir, name, "asset");
            if (!string.IsNullOrEmpty(path))
            {
                path = "Assets" + path.Replace(Application.dataPath, "");
                Unwrapping.GenerateSecondaryUVSet(mesh);
                //			combineUv2 = mesh.uv2;
                //			mesh = setuv2(mesh);
                AssetDatabase.CreateAsset(mesh, path);

            }
#endif
        }

        Mesh setuv2(Mesh mesh)
        {
            //		mesh.uv2 = combineUv2;
            return mesh;
        }

        ArrayList newCombineds = new ArrayList();
        //分解
        public void explainChildren()
        {
            if (!iscombined)
                return;
            iscombined = false;
            int count = 0;
            if (filters != null && filters.Length > 0)
            {
                count = filters.Length;
                Renderer curRenderer = null;
                for (int i = 0; i < count; i++)
                {
                    if (filters[i] == null)
                        continue;
                    curRenderer = filters[i].GetComponent<Renderer>();
                    if (curRenderer != null)
                    {
                        curRenderer.enabled = true;
                    }
                }
            }

            MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                mr.enabled = false;
#if UNITY_EDITOR
                DestroyImmediate(mr);
#endif
            }

            count = newCombineds.Count;
            for (int i = 0; i < count; i++)
            {
                GameObject.Destroy((GameObject)(newCombineds[i]));
            }
            newCombineds.Clear();
        }
    }
}