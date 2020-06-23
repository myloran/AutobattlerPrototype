using System;
using System.Collections.Generic;
using System.Linq;
using Model.NAI.Decisions;
using Model.NAI.NDecisionTree;
using UnityEngine;
using XNode;

namespace Data.DecisionTree {
	[CreateAssetMenu]
	public class DecisionTreeGraph : NodeGraph {
		[HideInInspector] public string[] DecisionTypes;
		[HideInInspector] public string[] ActionTypes;
		[HideInInspector] public int[] DecisionTypeEnums;
		[HideInInspector] public int[] ActionTypeEnums;

		public void Init() {
			DecisionTypes = LookupDecisionTreeTypes(typeof(BaseDecision));
			// ActionTypes = LookupDecisionTreeTypes(typeof(BaseAction));
		}

		string[] LookupDecisionTreeTypes(Type type) {
			return AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(type.IsAssignableFrom)
				.Where(t => t != type)
				.Where(t => t == typeof(CanMove))
				.Select(Activator.CreateInstance)
				.Cast<IDecisionTreeNode>()
				.Select(n => n.Type.ToString())
				.ToArray();
			// types
				// .Select(t => t.Name)
				// .ToArray();
		}
	}
}