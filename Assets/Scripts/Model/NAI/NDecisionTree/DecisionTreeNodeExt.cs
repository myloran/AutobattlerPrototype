namespace Model.NAI.NDecisionTree {
  public static class DecisionTreeNodeExt {
    public static IDecisionTreeNode Do(this IDecisionTreeNode self, 
        IDecisionTreeNode node) {
      if (self is BaseDecision bd) {
        bd.TrueNode = node;
        return self;
      }
      
      log.Error("You can use it only with BaseDecision");
      return self;
    }
    
    public static IDecisionTreeNode Else(this IDecisionTreeNode self, 
      IDecisionTreeNode node) {
      if (self is BaseDecision bd) {
        bd.FalseNode = node;
        return self;
      }
      
      log.Error("You can use it only with BaseDecision");
      return self;
    }
    static readonly Shared.Addons.OkwyLogging.Logger log = Shared.Addons.OkwyLogging.MainLog.GetLogger(nameof(DecisionTreeNodeExt));
  }
}