#pragma strict
var scale = 2.0;
var speed = 2.0;
private var baseHeight : Vector3[];
var useOriginal : boolean = false;

//波动效果
function Update () {
	var mesh : Mesh = GetComponent(MeshFilter).mesh;
	if (baseHeight == null)
		baseHeight = mesh.vertices;
	// gameObject.Destroy(GetComponent(MeshCollider));
	var vertices = new Vector3[baseHeight.Length];
	for (var i=0;i<vertices.Length;i++)
	{
		var vertex = baseHeight[i];
		if (useOriginal) {
			vertex.y += Mathf.Sin(Time.time * speed+ baseHeight[i].x + baseHeight[i].y + baseHeight[i].z) * scale;
		} else {
			vertex.y += Mathf.Sin(Time.time * speed+ baseHeight[i].x + baseHeight[i].y) * (scale*.5) + Mathf.Sin(Time.time * speed+ baseHeight[i].z + baseHeight[i].y) * (scale*.5);
		}
		vertices[i] = vertex;
	}
	mesh.vertices = vertices;
	mesh.RecalculateNormals();
	gameObject.Destroy(GetComponent(MeshCollider));
	var collider : MeshCollider = GetComponent(MeshCollider);
	if (collider == null) {
		collider = gameObject.AddComponent(MeshCollider);
		collider.isTrigger = true;
	}
}