using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SliceCharacter : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer _mesh;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private Slice _slice;
    
    private void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _slice = GetComponent<Slice>();
    }

    public void Slice(Vector3 normal, Vector3 origin)
    {
        _meshFilter.mesh = new Mesh()
        {
            vertices = _mesh.sharedMesh.vertices,
            triangles = _mesh.sharedMesh.triangles,
            uv = _mesh.sharedMesh.uv,
            uv2 = _mesh.sharedMesh.uv2,
            uv3 = _mesh.sharedMesh.uv3,
            uv4 = _mesh.sharedMesh.uv4,
            uv5 = _mesh.sharedMesh.uv5,
            uv6 = _mesh.sharedMesh.uv6,
            uv7 = _mesh.sharedMesh.uv7,
            uv8 = _mesh.sharedMesh.uv8,
            normals = _mesh.sharedMesh.normals,
            tangents = _mesh.sharedMesh.tangents,
            colors = _mesh.sharedMesh.colors,
            colors32 = _mesh.sharedMesh.colors32,
            boneWeights = _mesh.sharedMesh.boneWeights,
            bindposes = _mesh.sharedMesh.bindposes,
            bounds = _mesh.sharedMesh.bounds,
            subMeshCount = _mesh.sharedMesh.subMeshCount
        };
        _meshRenderer.material = _mesh.material;
        _mesh.enabled = false;
        _slice.ComputeSlice(normal, origin);
    }
}
