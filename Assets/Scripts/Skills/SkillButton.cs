using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour {

	public Button button;
	public CanvasRenderer cooldownRenderer;
	public Material m;

	private float radius;
	private RectTransform rect;
	private Vector2[] vertices;

	void Awake() {
		rect = GetComponent<RectTransform> ();
		radius = rect.rect.size.x / 2f;
		button = GetComponent<Button> ();

		vertices = new Vector2[] {
			new Vector2 (0f, 0f),
			new Vector2 (0f, radius),
			new Vector2 (-radius, radius),
			new Vector2 (-radius, -radius),
			new Vector2 (radius, -radius),
			new Vector2 (radius, radius)
		};

		cooldownRenderer.SetMaterial(m, null);
	}

	public void OnReady() {
		if (gameObject.activeSelf) {
			gameObject.SetActive (false);
		}
	}

	public void UpdateCooldown(float percent, int leftTime) {
		if (!gameObject.activeSelf) {
			gameObject.SetActive (true);
		}

		// set mesh
		Mesh m = CreateMeshFilter (percent);
		cooldownRenderer.SetMesh (m);

		// set number
	}

	private Mesh CreateMeshFilter(float percent) {
		Mesh m = new Mesh ();
		Vector2[] v2s = CalculateVertices (percent);
		Vector3[] v3s = new Vector3[v2s.Length];
		for (int i = 0; i < v3s.Length; i++) {
			v3s [i] = v2s [i];
		}
		m.vertices = v3s;
		m.uv = new Vector2[m.vertices.Length];
		m.triangles = CalculateTriangles (v2s);

		return m;
	}

	private Vector2[] CalculateVertices(float percent) {
		Vector2[] vs;
		if (percent < 0.125f) {
			vs = new Vector2[7];
			Array.Copy (vertices, vs, 6);
		} else if (percent < 0.375f) {
			vs = new Vector2[6];
			Array.Copy (vertices, vs, 5);
		} else if (percent < 0.625f) {
			vs = new Vector2[5];
			Array.Copy (vertices, vs, 4);
		} else if (percent < 0.875f) {
			vs = new Vector2[4];
			Array.Copy (vertices, vs, 3);
		} else {
			vs = new Vector2[3];
			Array.Copy (vertices, vs, 2);
		}
		vs[vs.Length - 1] = CalculateLastVertex(percent);

		return vs;
	}

	private Vector2 CalculateLastVertex(float percent) {
		Vector2 v = new Vector2();
		if (percent < 0.125f || percent >= 0.875f) {
			v = new Vector2 (radius * Mathf.Tan (Mathf.PI * 2f * percent), radius);
		} else if (percent < 0.375f) {
			v = new Vector2 (radius, radius * Mathf.Tan (Mathf.Deg2Rad * (90f - 360f * percent)));
		} else if (percent < 0.625f) {
			v = new Vector2 (radius * Mathf.Tan (Mathf.Deg2Rad * (180f - 360f * percent)), -radius);
		} else if (percent < 0.875f) {
			v = new Vector2 (-radius, -radius * Mathf.Tan (Mathf.Deg2Rad * (270f - 360f * percent)));
		}
		return v;
	}

	private int[] CalculateTriangles(Vector2[] vs) {
		Triangulator tri = new Triangulator (vs);
		return tri.Triangulate ();
	}
}
