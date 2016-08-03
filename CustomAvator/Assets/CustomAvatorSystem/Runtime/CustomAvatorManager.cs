/**********************************************************
*Author: wangjiaying
*Date: 2016.8.3
*Func:
**********************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CustomAvator.Runtime
{
    public class CustomAvatorManager : Singleton<CustomAvatorManager>
    {
        public void BuildingMesh(Transform meshRoot, bool destroy = false, params Transform[] targets)
        {
            List<SkinnedMeshRenderer> meshs = new List<SkinnedMeshRenderer>();
            for (int i = 0; i < targets.Length; i++)
            {
                meshs.AddRange(targets[i].GetComponentsInChildren<SkinnedMeshRenderer>());
            }

            BuildingMesh(meshRoot, meshs.ToArray());
            if (destroy)
            {
                for (int i = 0; i < meshs.Count; i++)
                {
                    GameObject.Destroy(meshs[i]);
                }
            }
        }

        private List<Transform> _modelBones;
        public void BuildingMesh(Transform meshRoot, params SkinnedMeshRenderer[] renders)
        {
            SkinnedMeshRenderer skin = meshRoot.GetComponent<SkinnedMeshRenderer>();
            if (!skin) skin = meshRoot.gameObject.AddComponent<SkinnedMeshRenderer>();
            Mesh mesh = skin.sharedMesh;
            if (mesh == null) mesh = new Mesh();
            mesh.name = "Actor";
            mesh.Clear();

            _modelBones = new List<Transform>(meshRoot.GetComponentsInChildren<Transform>(true));

            List<CombineInstance> combineInstances = new List<CombineInstance>();
            List<Material> materials = new List<Material>();
            List<Transform> bones = new List<Transform>();

            for (int i = 0; i < renders.Length; i++)
            {
                materials.AddRange(renders[i].materials);

                Mesh m = renders[i].sharedMesh;

                for (int j = 0; j < m.subMeshCount; j++)
                {
                    CombineInstance com = new CombineInstance();
                    com.mesh = m;
                    com.subMeshIndex = j;

                    combineInstances.Add(com);

                    FindBones(bones, renders[i].bones);
                    //RemoveRepeatBones(bones, renders[i].bones);
                }

            }

            mesh.CombineMeshes(combineInstances.ToArray(), false, false);
            skin.sharedMesh = mesh;
            skin.bones = bones.ToArray();
            skin.materials = materials.ToArray();
        }

        private void RemoveRepeatBones(List<Transform> bones, Transform[] bone2)
        {
            for (int i = 0; i < bone2.Length; i++)
            {
                if (bones.Contains(bone2[i])) continue;
                bones.Add(bone2[i]);
            }
        }

        private void FindBones(List<Transform> bonesHolder, Transform[] bones)
        {
            for (int i = 0; i < bones.Length; i++)
            {
                if (_modelBones.Contains(bones[i]))
                    bonesHolder.Add(bones[i]);
            }
        }

        private Transform[] FindBones(Transform root, IEnumerable<string> boneNames)
        {
            List<Transform> bones = new List<Transform>(30);

            List<Transform> boneList = new List<Transform>(root.GetComponentsInChildren<Transform>(true));

            foreach (string boneName in boneNames)
            {
                Transform trans = boneList.Find(delegate (Transform t)
                {
                    if (0 == string.Compare(boneName, t.name, false))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });

                if (null != trans)
                {
                    bones.Add(trans);
                }
                else
                {
                    Debug.LogError("cant find bone:" + boneName);
                }
            }

            return bones.ToArray();
        }

        private static List<string> GetBoneNames(IEnumerable<Transform> bones)
        {
            List<Transform> tmp = new List<Transform>(bones);

            return tmp.ConvertAll<string>(delegate (Transform trans)
            {
                return trans.name;
            });
        }
    }
}