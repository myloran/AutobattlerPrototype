using System;
using System.Collections.Generic;
using System.Linq;
using Controller.DecisionTree.Nodes;
using Model.NAI.NDecisionTree;
using UnityEngine;
using XNode;
using IDecisionTreeNode = Model.NAI.NDecisionTree.IDecisionTreeNode;

namespace Controller.DecisionTree {
	[CreateAssetMenu]
	public class DecisionTreeGraph : NodeGraph {
		[HideInInspector] public string[] DecisionTypeNames;
		[HideInInspector] public string[] ActionTypeNames;
		[HideInInspector] public int[] DecisionTypeIds;
		[HideInInspector] public int[] ActionTypeIds;
		public Action OnInit = () => {};

		public void Init() {
			//TODO: go through all old decision and action type ids and compare them with new ones and if they differ, don't load them and instead prompt user to save them 
			
			Lookup<BaseDecision>(out DecisionTypeNames, out DecisionTypeIds);
			Lookup<BaseAction>(out ActionTypeNames, out ActionTypeIds);

			foreach (var node in nodes)
				if (node is DecisionTreeSaverNode saver)
					saver.Load();
			
			OnInit();
			OnInit = () => { };
		}

		void Lookup<T>(out string[] types, out int[] ids) {
			var decisionEnums = LookupDecisionTreeEnums(typeof(T));
			types = decisionEnums.Select(e => e.ToString()).ToArray();
			ids = decisionEnums.Cast<int>().ToArray();
		}

		IEnumerable<EDecisionTreeType> LookupDecisionTreeEnums(Type type) =>
			AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(type.IsAssignableFrom)
				.Where(t => t != type)
				.Select(Activator.CreateInstance)
				.Cast<IDecisionTreeNode>()
				.Select(n => n.Type)
				.OrderBy(n => n);
	}
}