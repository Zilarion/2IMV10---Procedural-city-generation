﻿using System;
using UnityEngine;
using System.Collections.Generic;

namespace ProceduralCity
{
	public class EdgeBrick : EdgeModule
	{
		Color color;
		public EdgeBrick (Transform parent, Color c)
		{
			this.parent = parent;
			this.setCellDimensions(new Vector3(1.0f, 1.0f, 4.0f));
			this.setCellPadding(new Vector3(0.05f, 0.05f, 0.05f));
			this.color = c;
		}

		#region implemented abstract members of EdgeModule

		public override List<MeshFilter> apply (HighLevelEdge edge)
		{
			List<MeshFilter> meshes = new List<MeshFilter> ();
			Vector3 cornerDimensions = new Vector3 (1.0f, 1.0f, 1.0f);

			// Get from and translate to world space
			Vector3 from = edge.getFrom ().getVertex().getPoint();
			Vector3 to = edge.getTo ().getVertex().getPoint();
			from = parent.TransformPoint (from);
			to = parent.TransformPoint (to);

			// Get direction and translate to world space
			Vector3 direction = edge.getDirection ();
			direction = parent.TransformDirection (direction);
			direction.Normalize ();

			// Set from and to actual center points of the corner
			from = from + Vector3.Scale (edge.getFrom ().getTranslateVector (), cornerDimensions / 2) + Vector3.Scale(direction, cornerDimensions /2);
			to = to + Vector3.Scale (edge.getTo ().getTranslateVector (), cornerDimensions / 2) - Vector3.Scale(direction, cornerDimensions / 2);

			float edgeMagnitude = (to-from).magnitude;

			// These are all world space
			Vector3 dimensions = this.getCellDimensions ();
			Vector3 scale = this.getCellSize();

			Vector3 normal = edge.getNormal ();

			meshes.AddRange(FillCellModule.fillCell (from, to, scale, dimensions, parent, color, normal));

			return meshes;
		}

		#endregion
	}
}

