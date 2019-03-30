using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coolape
{
	public class CLTweenColor : TweenColor
	{
		public Renderer render;
		public bool isShardedMaterial = true;

		public Material material {
			get {
				if (render == null)
					return null;
				if (isShardedMaterial) {
					return render.sharedMaterial;
				}
				return render.material;
			}
		}

		public string shaderPropName = "";

		public Color value {
			get {
				if (material != null) {
					if (string.IsNullOrEmpty (shaderPropName)) {
						return material.color;
					} else {
						return material.GetColor (shaderPropName);
					}
				}
				return Color.black;
			}
			set {
				if (material != null) {
					if (string.IsNullOrEmpty (shaderPropName)) {
						material.color = value;
					} else {
						material.SetColor (shaderPropName, value);
					}
				}
			}
		}

		protected override void OnUpdate (float factor, bool isFinished)
		{
			value = Color.Lerp (from, to, factor);
		}
	}
}
