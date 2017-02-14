using UnityEngine;
using System.Collections;
using VRTK;
using VRTK.SecondaryControllerGrabActions;
using VRTK.GrabAttachMechanics;



/*
TubeRenderer.js

This script is created by Ray Nothnagel of Last Bastion Games. It is
free for use and available on the Unify Wiki.

For other components I've created, see:
http://lastbastiongames.com/middleware/

(C) 2008 Last Bastion Games

--------------------------------------------------------------

EDIT: MODIFIED BY JACOB FLETCHER FOR USE WITH THE ROPE SCRIPT
http://www.reverieinteractive.com

Modified again by Paul Calande to not run like total garbage.
*/

public class TubeVertex
{
	public Vector3 point = Vector3.zero;
	public float radius = 1;
	public Color color = Color.white;

	public TubeVertex(Vector3 pt, float r, Color c)
	{
		point=pt;
		radius=r;
		color=c;
	}
}

public class TubeRenderer : MonoBehaviour
{
	public TubeVertex[] vertices;
	public Material material;
	public int crossSegments = 3;
	public float flatAtDistance = -1;
	public float movePixelsForRebuild = 6;
	public float maxRebuildTime = 0.1f;
	public bool useMeshCollision = false;

	private Vector3 lastCameraPosition1;
	private Vector3 lastCameraPosition2;
	private Vector3[] crossPoints;
	private int lastCrossSegments;
	private float lastRebuildTime = 0;
	private Mesh mesh;
	private bool usingBumpmap = false;

    private Renderer rend;
    private MeshCollider meshcol;
    private MeshFilter meshfilt;

    private bool readyToUpdateMesh = false;

    public void Reset()
	{
		vertices = new TubeVertex[2];
		vertices[0] = new TubeVertex(Vector3.zero, 1.0f, Color.white);
		vertices[1] = new TubeVertex(Vector3.right, 1.0f, Color.white);
	}
    void Awake()
    {
        gameObject.AddComponent(typeof(VRTK_ClimbableGrabAttach));
        gameObject.AddComponent(typeof(VRTK_SwapControllerGrabAction));


        VRTK_InteractableObject vrtkio = gameObject.AddComponent<VRTK_InteractableObject>();
        vrtkio.disableWhenIdle = false;
        vrtkio.enabled = true;
        vrtkio.isGrabbable = true;
        vrtkio.holdButtonToGrab = true;
        vrtkio.isUsable = true;
        vrtkio.holdButtonToUse = true;
        vrtkio.useOnlyIfGrabbed = true;


    }

	void Start()
	{
		Reset();
        mesh = new Mesh();
		gameObject.AddComponent(typeof(MeshFilter));

      



        MeshRenderer mr = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		mr.material = material;
		if(material)
		{
			if(material.GetTexture("_BumpMap")) usingBumpmap = true;
		}

        rend = GetComponent<Renderer>();
        meshfilt = GetComponent<MeshFilter>();

        if (useMeshCollision)
        {
            meshcol = GetComponent<MeshCollider>();
            if (!meshcol)
            {
                meshcol = gameObject.AddComponent<MeshCollider>();
            }

            meshcol.convex = true;
            meshcol.inflateMesh = true;
            meshcol.skinWidth = .07f;
        }

        // Bake some calculations.
        if (crossSegments != lastCrossSegments)
        {
            float theta = 2 * Mathf.PI / crossSegments;
            crossPoints = new Vector3[crossSegments];
            for (int c = 0; c < crossSegments; c++)
            {
                crossPoints[c] = new Vector3(Mathf.Cos(theta * c), Mathf.Sin(theta * c), 0);
            }
            lastCrossSegments = crossSegments;
        }

        if (vertices.Length <= 1)
        {
            rend.enabled = false;
        }
    }

	void LateUpdate ()
	{
        //UpdateMesh();

        if (readyToUpdateMesh)
        {
            UpdateMesh();
            readyToUpdateMesh = false;
        }
    }


	private Vector4[] CalculateTangents(Vector3[] verts)
	{
		Vector4[] tangents = new Vector4[verts.Length];

		for(int i=0;i<tangents.Length;i++)
		{
			Vector3 vertex1 = i > 0 ? verts[i-1] : verts[i];
			Vector3 vertex2 = i < tangents.Length - 1 ? verts[i+1] : verts[i];
			Vector3 tan = (vertex1 - vertex2).normalized;
			tangents[i] = new Vector4( tan.x, tan.y, tan.z, 1.0f );
		}
		return tangents;   
	}



	//sets all the points to points of a Vector3 array, as well as capping the ends.
	public void SetPoints(Vector3[] points, float radius, Color col)
	{
		if (points.Length < 2) return;
		vertices = new TubeVertex[points.Length+2];

		Vector3 v0offset = (points[0]-points[1])*0.01f;
		vertices[0] = new TubeVertex(v0offset+points[0], 0.0f, col);
		Vector3 v1offset = (points[points.Length-1] - points[points.Length-2])*0.01f;
		vertices[vertices.Length-1] = new TubeVertex(v1offset+points[points.Length-1], 0.0f, col);

		for (int p=0;p<points.Length;p++) 
		{
			vertices[p+1] = new TubeVertex(points[p], radius, col);
		}
	}

    public void UpdateMesh()
    {
        Vector3[] meshVertices = new Vector3[vertices.Length * crossSegments];
        Vector2[] uvs = new Vector2[vertices.Length * crossSegments];
        Color[] colors = new Color[vertices.Length * crossSegments];
        int[] tris = new int[vertices.Length * crossSegments * 6];
        int[] lastVertices = new int[crossSegments];
        int[] theseVertices = new int[crossSegments];
        Quaternion rotation = Quaternion.identity;

        for (int p = 0; p < vertices.Length; ++p)
        {
            if (p < vertices.Length - 1)
            {
                rotation = Quaternion.FromToRotation(Vector3.forward, vertices[p + 1].point - vertices[p].point);
            }

            for (int c = 0; c < crossSegments; ++c)
            {
                int vertexIndex = p * crossSegments + c;
                meshVertices[vertexIndex] = vertices[p].point + rotation * crossPoints[c] * vertices[p].radius;
                uvs[vertexIndex] = new Vector2((float)c / crossSegments, (float)p / vertices.Length);
                colors[vertexIndex] = vertices[p].color;

                lastVertices[c] = theseVertices[c];
                theseVertices[c] = p * crossSegments + c;
            }

            //make triangles
            if (p > 0)
            {
                for (int c = 0; c < crossSegments; c++)
                {
                    int start = (p * crossSegments + c) * 6;
                    tris[start] = lastVertices[c];
                    tris[start + 1] = lastVertices[(c + 1) % crossSegments];
                    tris[start + 2] = theseVertices[c];
                    tris[start + 3] = tris[start + 2];
                    tris[start + 4] = tris[start + 1];
                    tris[start + 5] = theseVertices[(c + 1) % crossSegments];
                }
            }
        }

        //Clear mesh for new build  (jf)   
        mesh.Clear();
        mesh.vertices = meshVertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        if (usingBumpmap)
        {
            mesh.tangents = CalculateTangents(meshVertices);
        }
        mesh.uv = uvs;

        if (useMeshCollision)
        {
            meshcol.sharedMesh = mesh;
        }

        meshfilt.mesh = mesh;
    }

    public void PrepareToUpdateMesh()
    {
        readyToUpdateMesh = true;
    }


}