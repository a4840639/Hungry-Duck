using UnityEngine;
using System.Collections;

public class shallow_wave : MonoBehaviour {
	int size;
	float[,] old_h;
	float[,] h;
	float[,] new_h;


	// Use this for initialization
	void Start () {
		size = 64;
		old_h = new float[size, size];
		h = new float[size, size];
		new_h = new float[size, size];
	
		//Resize the mesh into a size*size grid
		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		mesh.Clear ();
		Vector3[] vertices=new Vector3[size*size];
		for (int i=0; i<size; i++)
		for (int j=0; j<size; j++) 
		{
			vertices[i*size+j].x=i*0.2f-size*0.1f;
			vertices[i*size+j].y=0;
			vertices[i*size+j].z=j*0.2f-size*0.1f;
		}
		int[] triangles = new int[(size - 1) * (size - 1) * 6];
		int index = 0;
		for (int i=0; i<size-1; i++)
		for (int j=0; j<size-1; j++)
		{
			triangles[index*6+0]=(i+0)*size+(j+0);
			triangles[index*6+1]=(i+0)*size+(j+1);
			triangles[index*6+2]=(i+1)*size+(j+1);
			triangles[index*6+3]=(i+0)*size+(j+0);
			triangles[index*6+4]=(i+1)*size+(j+1);
			triangles[index*6+5]=(i+1)*size+(j+0);
			index++;
		}
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals ();
	


	}

	void Shallow_Wave()
	{	
		float rate = 0.005f;
		float damping = 0.999f;
		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {
				float l, r, u, d;
				float height = h [x, y];
				if (x > 0) {
					l = h [x - 1, y] - height;
				} else {
					l = 0;
				}
				if (x < size - 1) {
					r = h [x + 1, y] - height;
				} else {
					r = 0;
				}
				if (y > 0) {
					u = h [x, y - 1] - height;
				} else {
					u = 0;
				}
				if (y < size - 1) {
					d = h [x, y + 1] - height;
				} else {
					d = 0;
				}
				new_h [x, y] = height + (height - old_h [x, y]) * damping + (l + r + u + d) * rate;
			}
		}

		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {
				old_h [x, y] = h [x, y];
				h [x, y] = new_h [x, y];
			}
		}
	}

	// Update is called once per frame
	void Update () 
	{
		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		Vector3[] vertices = mesh.vertices;

		//Step 1: Copy vertices.y into h
		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {
				h [x, y] = vertices [x * size + y].y;
			}
		}

		//Step 2: User interaction
		if (Input.GetKeyDown ("r")) {
			float m = 0.05f + Random.value * 0.05f;
			int i = Random.Range (0, size - 1);
			int j = Random.Range (0, size - 1);
			h [i, j] -= m;
		}
	
		//Step 3: Run Shallow Wav
		for (int c = 0; c < 8; c++) {
			Shallow_Wave ();
		}

		//Step 4: Copy h back into mesh
		for (int x = 0; x < size; x++) {
			for (int y = 0; y < size; y++) {
				vertices [x * size + y].y = h [x, y];
			}
		}

		mesh.vertices = vertices;
		mesh.RecalculateNormals ();

	}
}
