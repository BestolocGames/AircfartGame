using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.MapGeneration
{
    public class OceanGeometry : MonoBehaviour
    {
         [SerializeField] private WavesGenerator _wavesGenerator;
        
         [SerializeField] private Transform _viewer;
        
         [SerializeField] private Material _oceanMaterial;
         
         [SerializeField] private bool _updateMaterialProperties;
        
         [SerializeField]
        bool _showMaterialLods;

         [SerializeField] private float _lengthScale = 10;
         
         [SerializeField, Range(1, 40)] private int _vertexDensity = 30;
         
         [SerializeField, Range(0, 8)] private int _clipLevels = 8;
        
         [SerializeField, Range(0, 100)] private float _skirtSize = 50;

         private List<Element> _rings = new List<Element>();
         private List<Element> _trims = new List<Element>();
         private Element _center;
        Element _skirt;
        Quaternion[] _trimRotations;
        int _previousVertexDensity;
        float _previousSkirtSize;

        Material[] _materials;

        private void Start()
        {
            if (_viewer == null)
                _viewer = Camera.main.transform;

            _oceanMaterial.SetTexture("_Displacement_c0", _wavesGenerator.Cascade0.Displacement);
            _oceanMaterial.SetTexture("_Derivatives_c0", _wavesGenerator.Cascade0.Derivatives);
            _oceanMaterial.SetTexture("_Turbulence_c0", _wavesGenerator.Cascade0.Turbulence);

            _oceanMaterial.SetTexture("_Displacement_c1", _wavesGenerator.Cascade1.Displacement);
            _oceanMaterial.SetTexture("_Derivatives_c1", _wavesGenerator.Cascade1.Derivatives);
            _oceanMaterial.SetTexture("_Turbulence_c1", _wavesGenerator.Cascade1.Turbulence);

            _oceanMaterial.SetTexture("_Displacement_c2", _wavesGenerator.Cascade2.Displacement);
            _oceanMaterial.SetTexture("_Derivatives_c2", _wavesGenerator.Cascade2.Derivatives);
            _oceanMaterial.SetTexture("_Turbulence_c2", _wavesGenerator.Cascade2.Turbulence);


            _materials = new Material[3];
            _materials[0] = new Material(_oceanMaterial);
            _materials[0].EnableKeyword("CLOSE");

            _materials[1] = new Material(_oceanMaterial);
            _materials[1].EnableKeyword("MID");
            _materials[1].DisableKeyword("CLOSE");

            _materials[2] = new Material(_oceanMaterial);
            _materials[2].DisableKeyword("MID");
            _materials[2].DisableKeyword("CLOSE");

            _trimRotations = new Quaternion[]
            {
                Quaternion.AngleAxis(180, Vector3.up),
                Quaternion.AngleAxis(90, Vector3.up),
                Quaternion.AngleAxis(270, Vector3.up),
                Quaternion.identity,
            };

            InstantiateMeshes();
        }

        private void Update()
        {
            if (_rings.Count != _clipLevels || _trims.Count != _clipLevels
                                          || _previousVertexDensity != _vertexDensity || !Mathf.Approximately(_previousSkirtSize, _skirtSize))
            {
                InstantiateMeshes();
                _previousVertexDensity = _vertexDensity;
                _previousSkirtSize = _skirtSize;
            }

            UpdatePositions();
            UpdateMaterials();
        }

        void UpdateMaterials()
        {
            if (_updateMaterialProperties && !_showMaterialLods)
            {
                for (int i = 0; i < 3; i++)
                {
                    _materials[i].CopyPropertiesFromMaterial(_oceanMaterial);
                }
                _materials[0].EnableKeyword("CLOSE");
                _materials[1].EnableKeyword("MID");
                _materials[1].DisableKeyword("CLOSE");
                _materials[2].DisableKeyword("MID");
                _materials[2].DisableKeyword("CLOSE");
            }
            if (_showMaterialLods)
            {
                _materials[0].SetColor("_Color", Color.red * 0.6f);
                _materials[1].SetColor("_Color", Color.green * 0.6f);
                _materials[2].SetColor("_Color", Color.blue * 0.6f);
            }

            int activeLevels = ActiveLodlevels();
            _center.MeshRenderer.material = GetMaterial(_clipLevels - activeLevels - 1);

            for (int i = 0; i < _rings.Count; i++)
            {
                _rings[i].MeshRenderer.material = GetMaterial(_clipLevels - activeLevels + i);
                _trims[i].MeshRenderer.material = GetMaterial(_clipLevels - activeLevels + i);
            }
        }

        Material GetMaterial(int lodLevel)
        {
            if (lodLevel - 2 <= 0)
                return _materials[0];

            if (lodLevel - 2 <= 2)
                return _materials[1];

            return _materials[2];
        }

        void UpdatePositions()
        {
            int k = GridSize();
            int activeLevels = ActiveLodlevels();
            float y = 2.8f;

            float scale = ClipLevelScale(-1, activeLevels);
            Vector3 previousSnappedPosition = Snap(_viewer.position, scale * 2);
            _center.Transform.position = previousSnappedPosition + OffsetFromCenter(-1, activeLevels);
            _center.Transform.position = new Vector3(_center.Transform.position.x, y, _center.Transform.position.z);
            _center.Transform.localScale = new Vector3(scale, 1, scale);

            for (int i = 0; i < _clipLevels; i++)
            {
                _rings[i].Transform.gameObject.SetActive(i < activeLevels);
                _trims[i].Transform.gameObject.SetActive(i < activeLevels);
                if (i >= activeLevels) continue;

                scale = ClipLevelScale(i, activeLevels);
                Vector3 centerOffset = OffsetFromCenter(i, activeLevels);
                Vector3 snappedPosition = Snap(_viewer.position, scale * 2);

                Vector3 trimPosition = centerOffset + snappedPosition + scale * (k - 1) / 2 * new Vector3(1, 0, 1);
                int shiftX = previousSnappedPosition.x - snappedPosition.x < float.Epsilon ? 1 : 0;
                int shiftZ = previousSnappedPosition.z - snappedPosition.z < float.Epsilon ? 1 : 0;
                trimPosition += shiftX * (k + 1) * scale * Vector3.right;
                trimPosition += shiftZ * (k + 1) * scale * Vector3.forward;
                _trims[i].Transform.position = trimPosition;
                _trims[i].Transform.position = new Vector3(_trims[i].Transform.position.x, y, _trims[i].Transform.position.z);
                _trims[i].Transform.rotation = _trimRotations[shiftX + 2 * shiftZ];
                _trims[i].Transform.localScale = new Vector3(scale, 1, scale);

                _rings[i].Transform.position = snappedPosition + centerOffset;
                _rings[i].Transform.position = new Vector3(_rings[i].Transform.position.x, y, _rings[i].Transform.position.z);
                _rings[i].Transform.localScale = new Vector3(scale, 1, scale);
                previousSnappedPosition = snappedPosition;
            }

            scale = _lengthScale * 2 * Mathf.Pow(2, _clipLevels);
            _skirt.Transform.position = new Vector3(-1, 0, -1) * scale * (_skirtSize + 0.5f - 0.5f / GridSize()) + previousSnappedPosition;
            _skirt.Transform.position = new Vector3(_skirt.Transform.position.x, y, _skirt.Transform.position.z);
            _skirt.Transform.localScale = new Vector3(scale, 1, scale);
        }

        int ActiveLodlevels()
        {
            return _clipLevels - Mathf.Clamp((int)Mathf.Log((1.7f * Mathf.Abs(_viewer.position.y) + 1) / _lengthScale, 2), 0, _clipLevels);
        }

        float ClipLevelScale(int level, int activeLevels)
        {
            return _lengthScale / GridSize() * Mathf.Pow(2, _clipLevels - activeLevels + level + 1);
        }

        Vector3 OffsetFromCenter(int level, int activeLevels)
        {
            return (Mathf.Pow(2, _clipLevels) + GeometricProgressionSum(2, 2, _clipLevels - activeLevels + level + 1, _clipLevels - 1))
                * _lengthScale / GridSize() * (GridSize() - 1) / 2 * new Vector3(-1, 0, -1);
        }

        float GeometricProgressionSum(float b0, float q, int n1, int n2)
        {
            return b0 / (1 - q) * (Mathf.Pow(q, n2) - Mathf.Pow(q, n1));
        }

        int GridSize()
        {
            return 4 * _vertexDensity + 1;
        }

        Vector3 Snap(Vector3 coords, float scale)
        {
            if (coords.x >= 0)
                coords.x = Mathf.Floor(coords.x / scale) * scale;
            else
                coords.x = Mathf.Ceil((coords.x - scale + 1) / scale) * scale;

            if (coords.z < 0)
                coords.z = Mathf.Floor(coords.z / scale) * scale;
            else
                coords.z = Mathf.Ceil((coords.z - scale + 1) / scale) * scale;

            coords.y = 0;
            return coords;
        }

        void InstantiateMeshes()
        {
            foreach (var child in gameObject.GetComponentsInChildren<Transform>())
            {
                if (child != transform)
                    Destroy(child.gameObject);
            }
            _rings.Clear();
            _trims.Clear();

            int k = GridSize();
            _center = InstantiateElement("Center", CreatePlaneMesh(2 * k, 2 * k, 1, Seams.All), _materials[_materials.Length - 1]);
            Mesh ring = CreateRingMesh(k, 1);
            Mesh trim = CreateTrimMesh(k, 1);
            for (int i = 0; i < _clipLevels; i++)
            {
                _rings.Add(InstantiateElement("Ring " + i, ring, _materials[_materials.Length - 1]));
                _trims.Add(InstantiateElement("Trim " + i, trim, _materials[_materials.Length - 1]));
            }
            _skirt = InstantiateElement("Skirt", CreateSkirtMesh(k, _skirtSize), _materials[_materials.Length - 1]);
        }

        Element InstantiateElement(string name, Mesh mesh, Material mat)
        {
            GameObject go = new GameObject();
            go.name = name;
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            MeshFilter meshFilter = go.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            meshRenderer.receiveShadows = true;
            meshRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.Camera;
            meshRenderer.material = mat;
            meshRenderer.allowOcclusionWhenDynamic = false;
            return new Element(go.transform, meshRenderer);
        }

        Mesh CreateSkirtMesh(int k, float outerBorderScale)
        {
            Mesh mesh = new Mesh();
            mesh.name = "Clipmap skirt";
            CombineInstance[] combine = new CombineInstance[8];

            Mesh quad = CreatePlaneMesh(1, 1, 1);
            Mesh hStrip = CreatePlaneMesh(k, 1, 1);
            Mesh vStrip = CreatePlaneMesh(1, k, 1);


            Vector3 cornerQuadScale = new Vector3(outerBorderScale, 1, outerBorderScale);
            Vector3 midQuadScaleVert = new Vector3(1f / k, 1, outerBorderScale);
            Vector3 midQuadScaleHor = new Vector3(outerBorderScale, 1, 1f / k);

            combine[0].transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, cornerQuadScale);
            combine[0].mesh = quad;

            combine[1].transform = Matrix4x4.TRS(Vector3.right * outerBorderScale, Quaternion.identity, midQuadScaleVert);
            combine[1].mesh = hStrip;

            combine[2].transform = Matrix4x4.TRS(Vector3.right * (outerBorderScale + 1), Quaternion.identity, cornerQuadScale);
            combine[2].mesh = quad;

            combine[3].transform = Matrix4x4.TRS(Vector3.forward * outerBorderScale, Quaternion.identity, midQuadScaleHor);
            combine[3].mesh = vStrip;

            combine[4].transform = Matrix4x4.TRS(Vector3.right * (outerBorderScale + 1)
                                                 + Vector3.forward * outerBorderScale, Quaternion.identity, midQuadScaleHor);
            combine[4].mesh = vStrip;

            combine[5].transform = Matrix4x4.TRS(Vector3.forward * (outerBorderScale + 1), Quaternion.identity, cornerQuadScale);
            combine[5].mesh = quad;

            combine[6].transform = Matrix4x4.TRS(Vector3.right * outerBorderScale
                                                 + Vector3.forward * (outerBorderScale + 1), Quaternion.identity, midQuadScaleVert);
            combine[6].mesh = hStrip;

            combine[7].transform = Matrix4x4.TRS(Vector3.right * (outerBorderScale + 1)
                                                 + Vector3.forward * (outerBorderScale + 1), Quaternion.identity, cornerQuadScale);
            combine[7].mesh = quad;
            mesh.CombineMeshes(combine, true);
            return mesh;
        }

        Mesh CreateTrimMesh(int k, float lengthScale)
        {
            Mesh mesh = new Mesh();
            mesh.name = "Clipmap trim";
            CombineInstance[] combine = new CombineInstance[2];

            combine[0].mesh = CreatePlaneMesh(k + 1, 1, lengthScale, Seams.None, 1);
            combine[0].transform = Matrix4x4.TRS(new Vector3(-k - 1, 0, -1) * lengthScale, Quaternion.identity, Vector3.one);

            combine[1].mesh = CreatePlaneMesh(1, k, lengthScale, Seams.None, 1);
            combine[1].transform = Matrix4x4.TRS(new Vector3(-1, 0, -k - 1) * lengthScale, Quaternion.identity, Vector3.one);

            mesh.CombineMeshes(combine, true);
            return mesh;
        }

        Mesh CreateRingMesh(int k, float lengthScale)
        {
            Mesh mesh = new Mesh();
            mesh.name = "Clipmap ring";
            if ((2 * k + 1) * (2 * k + 1) >= 256 * 256)
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            CombineInstance[] combine = new CombineInstance[4];

            combine[0].mesh = CreatePlaneMesh(2 * k, (k - 1) / 2, lengthScale, Seams.Bottom | Seams.Right | Seams.Left);
            combine[0].transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

            combine[1].mesh = CreatePlaneMesh(2 * k, (k - 1) / 2, lengthScale, Seams.Top | Seams.Right | Seams.Left);
            combine[1].transform = Matrix4x4.TRS(new Vector3(0, 0, k + 1 + (k - 1) / 2) * lengthScale, Quaternion.identity, Vector3.one);

            combine[2].mesh = CreatePlaneMesh((k - 1) / 2, k + 1, lengthScale, Seams.Left);
            combine[2].transform = Matrix4x4.TRS(new Vector3(0, 0, (k - 1) / 2) * lengthScale, Quaternion.identity, Vector3.one);

            combine[3].mesh = CreatePlaneMesh((k - 1) / 2, k + 1, lengthScale, Seams.Right);
            combine[3].transform = Matrix4x4.TRS(new Vector3(k + 1 + (k - 1) / 2, 0, (k - 1) / 2) * lengthScale, Quaternion.identity, Vector3.one);

            mesh.CombineMeshes(combine, true);
            return mesh;
        }

        Mesh CreatePlaneMesh(int width, int height, float lengthScale, Seams seams = Seams.None, int trianglesShift = 0)
        {
            Mesh mesh = new Mesh();
            mesh.name = "Clipmap plane";
            if ((width + 1) * (height + 1) >= 256 * 256)
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            Vector3[] vertices = new Vector3[(width + 1) * (height + 1)];
            int[] triangles = new int[width * height * 2 * 3];
            Vector3[] normals = new Vector3[(width + 1) * (height + 1)];

            for (int i = 0; i < height + 1; i++)
            {
                for (int j = 0; j < width + 1; j++)
                {
                    int x = j;
                    int z = i;

                    if ((i == 0 && seams.HasFlag(Seams.Bottom)) || (i == height && seams.HasFlag(Seams.Top)))
                        x = x / 2 * 2;
                    if ((j == 0 && seams.HasFlag(Seams.Left)) || (j == width && seams.HasFlag(Seams.Right)))
                        z = z / 2 * 2;

                    vertices[j + i * (width + 1)] = new Vector3(x, 0, z) * lengthScale;
                    normals[j + i * (width + 1)] = Vector3.up;
                }
            }

            int tris = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int k = j + i * (width + 1);
                    if ((i + j + trianglesShift) % 2 == 0)
                    {
                        triangles[tris++] = k;
                        triangles[tris++] = k + width + 1;
                        triangles[tris++] = k + width + 2;

                        triangles[tris++] = k;
                        triangles[tris++] = k + width + 2;
                        triangles[tris++] = k + 1;
                    }
                    else
                    {
                        triangles[tris++] = k;
                        triangles[tris++] = k + width + 1;
                        triangles[tris++] = k + 1;

                        triangles[tris++] = k + 1;
                        triangles[tris++] = k + width + 1;
                        triangles[tris++] = k + width + 2;
                    }
                }
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.normals = normals;
            return mesh;
        }

        class Element
        {
            public Transform Transform;
            public MeshRenderer MeshRenderer;

            public Element(Transform transform, MeshRenderer meshRenderer)
            {
                Transform = transform;
                MeshRenderer = meshRenderer;
            }
        }


        [System.Flags]
        enum Seams
        {
            None = 0,
            Left = 1,
            Right = 2,
            Top = 4,
            Bottom = 8,
            All = Left | Right | Top | Bottom
        };
    }
}


