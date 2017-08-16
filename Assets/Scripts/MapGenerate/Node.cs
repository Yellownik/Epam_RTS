﻿using UnityEngine;

namespace MapGenerate
{
	public class Node
	{
		public Vector3 position;
		public int vertexIndex = -1;

		public Node(Vector3 position)
		{
			this.position = position;
		}
	}
}