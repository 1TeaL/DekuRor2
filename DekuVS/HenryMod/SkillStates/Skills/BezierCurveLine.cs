using System;
using UnityEngine;


namespace DekuMod.SkillStates
{



	// Token: 0x0200003A RID: 58
	[RequireComponent(typeof(LineRenderer))]
	[ExecuteAlways]
	public class BezierCurveLine : MonoBehaviour
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600010C RID: 268 RVA: 0x000069BF File Offset: 0x00004BBF
		// (set) Token: 0x0600010D RID: 269 RVA: 0x000069C7 File Offset: 0x00004BC7
		public LineRenderer lineRenderer { get; private set; }

		// Token: 0x0600010E RID: 270 RVA: 0x000069D0 File Offset: 0x00004BD0
		private void Awake()
		{
			this.lineRenderer = base.GetComponent<LineRenderer>();
			this.windPhaseShift = UnityEngine.Random.insideUnitSphere * 360f;
			Array.Resize<Vector3>(ref this.vertexList, this.lineRenderer.positionCount + 1);
			this.UpdateBezier(0f);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00006A21 File Offset: 0x00004C21
		public void OnEnable()
		{
			Array.Resize<Vector3>(ref this.vertexList, this.lineRenderer.positionCount + 1);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00006A3B File Offset: 0x00004C3B
		private void LateUpdate()
		{
			this.UpdateBezier(Time.deltaTime);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00006A48 File Offset: 0x00004C48
		public void UpdateBezier(float deltaTime)
		{
			this.windTime += deltaTime;
			this.p0 = base.transform.position;
			if (this.endTransform)
			{
				this.p1 = this.endTransform.position;
			}
			if (this.animateBezierWind)
			{
				this.finalv0 = this.v0 + new Vector3(Mathf.Sin(0.017453292f * (this.windTime * 360f + this.windPhaseShift.x) * this.windFrequency.x) * this.windMagnitude.x, Mathf.Sin(0.017453292f * (this.windTime * 360f + this.windPhaseShift.y) * this.windFrequency.y) * this.windMagnitude.y, Mathf.Sin(0.017453292f * (this.windTime * 360f + this.windPhaseShift.z) * this.windFrequency.z) * this.windMagnitude.z);
				this.finalv1 = this.v1 + new Vector3(Mathf.Sin(0.017453292f * (this.windTime * 360f + this.windPhaseShift.x + this.p1.x) * this.windFrequency.x) * this.windMagnitude.x, Mathf.Sin(0.017453292f * (this.windTime * 360f + this.windPhaseShift.y + this.p1.z) * this.windFrequency.y) * this.windMagnitude.y, Mathf.Sin(0.017453292f * (this.windTime * 360f + this.windPhaseShift.z + this.p1.y) * this.windFrequency.z) * this.windMagnitude.z);
			}
			else
			{
				this.finalv0 = this.v0;
				this.finalv1 = this.v1;
			}
			for (int i = 0; i < this.vertexList.Length; i++)
			{
				float t = (float)i / (float)(this.vertexList.Length - 2);
				this.vertexList[i] = this.EvaluateBezier(t);
			}
			this.lineRenderer.SetPositions(this.vertexList);
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00006CB0 File Offset: 0x00004EB0
		private Vector3 EvaluateBezier(float t)
		{
			Vector3 a = Vector3.Lerp(this.p0, this.p0 + this.finalv0, t);
			Vector3 b = Vector3.Lerp(this.p1, this.p1 + this.finalv1, 1f - t);
			return Vector3.Lerp(a, b, t);
		}

		// Token: 0x04000129 RID: 297
		private Vector3[] vertexList = Array.Empty<Vector3>();

		// Token: 0x0400012A RID: 298
		public Vector3 p0 = Vector3.zero;

		// Token: 0x0400012B RID: 299
		public Vector3 v0 = Vector3.zero;

		// Token: 0x0400012C RID: 300
		public Vector3 p1 = Vector3.zero;

		// Token: 0x0400012D RID: 301
		public Vector3 v1 = Vector3.zero;

		// Token: 0x0400012E RID: 302
		public Transform endTransform;

		// Token: 0x0400012F RID: 303
		public bool animateBezierWind;

		// Token: 0x04000130 RID: 304
		public Vector3 windMagnitude;

		// Token: 0x04000131 RID: 305
		public Vector3 windFrequency;

		// Token: 0x04000132 RID: 306
		private Vector3 windPhaseShift;

		// Token: 0x04000133 RID: 307
		private Vector3 lastWind;

		// Token: 0x04000134 RID: 308
		private Vector3 finalv0;

		// Token: 0x04000135 RID: 309
		private Vector3 finalv1;

		// Token: 0x04000136 RID: 310
		private float windTime;
	}
}