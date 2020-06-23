using UnityEngine;
using XNode;

namespace Data.DecisionTree.Nodes {
	public class DecisionNode : Node {
		[Input, HideInInspector] public bool input;
		[Output, HideInInspector] public bool output1;
		[Output, HideInInspector] public bool output2;

		int selected;
		// Use this for initialization
		protected override void Init() {
			base.Init();
		
		}

		// Return the correct value of an output port when requested
		public override object GetValue(NodePort port) {
			return null; // Replace this
		}
	}
}