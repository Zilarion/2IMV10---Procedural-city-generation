﻿using System;
using UnityEngine;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class ModelRedBricks : ModelModule
	{
		public ModelRedBricks (Transform parent)
		{
			this.parent = parent;
			Color colorWhite = Color.Lerp(Color.white, Color.black, 0.3f);
			Color colorRed = Color.Lerp(Color.red, Color.black, 0.5f);
			this.setCornerModule (new CornerBlackBrick (parent, colorWhite));
			this.setVerticalEdgesModule (new EdgeHorizontalBricks (parent, colorRed));
			this.setHorizontalEdgesModule (new EdgeBrick (parent, colorWhite));
			this.setVerticalRegionModule (new RegionBricks (parent, colorRed));
//			this.setHorizontalRegionModule (new SingleHorizontalRegion (parent, colorWhite));
		}

		public override bool apply (HighLevelMesh mesh)
		{
			List<MeshFilter> meshes = new List<MeshFilter> ();
			// Apply all corners
			meshes.AddRange(this.applyCorners (mesh.getCorners ()));

			// Apply all edges
			meshes.AddRange(this.applyEdges (mesh.getEdges ()));

			// Apply all regions
			meshes.AddRange(this.applyRegions (mesh.getRegions ()));

			MeshFilter[] meshFilters = meshes.ToArray ();

			CombineInstance[] combine = new CombineInstance[meshFilters.Length];
			int i = 0;
			while (i < meshFilters.Length) {
				combine[i].mesh = meshFilters[i].sharedMesh;
				combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
				meshFilters [i].gameObject.SetActive (false);
				i++;
			}
			GameObject regionCombined = new GameObject ();
			regionCombined.name = "RedBrickBuilding";
			regionCombined.AddComponent<MeshFilter> ();
			MeshRenderer renderer = regionCombined.AddComponent<MeshRenderer> ();
			regionCombined.GetComponent<MeshFilter>().mesh = new Mesh();
			regionCombined.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);

			Material newMat = Resources.Load("Materials/Concrete_Asphalt_02", typeof(Material)) as Material;
			renderer.sharedMaterial = newMat;
			renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			regionCombined.isStatic = true;
			regionCombined.gameObject.SetActive (true);

			return true;
		}
	}
}

