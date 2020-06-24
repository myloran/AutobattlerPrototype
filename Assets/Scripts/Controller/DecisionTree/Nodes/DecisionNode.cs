using Model.NAI.NDecisionTree;
using UnityEngine;
using XNode;

namespace Controller.DecisionTree.Nodes {
	public class DecisionNode : Node, IDecisionTreeTypeNode {
		[Input(connectionType = ConnectionType.Override), HideInInspector] public bool Input;
		[Output(connectionType = ConnectionType.Override), HideInInspector] public bool Output1;
		[Output(connectionType = ConnectionType.Override), HideInInspector] public bool Output2;

		public int Selected { get; set; } //TODO: Set Selected on windows focused based on decision id

		public EDecision Type { get; } = EDecision.BaseDecision; //TODO: remove it to remove reference to model
		
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