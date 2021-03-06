namespace Model.NAI.NDecisionTree {
  //enum is used for versioning, careful with replacing enum values 
  public enum EDecision {
    BaseDecision = 0,
    BaseAction = 1,
    LoggingDecorator = 2,
    Move = 3,
    Attack = 4,
    DoNothing = 5,
    FindNearestTarget = 6,
    StartAttack = 7,
    WaitDiffBetweenDiagonalAndStraightMove = 8,
    WaitFirstEnemyArriving = 9,
    WaitForAlliesToMove = 10,
    CanMove = 11,
    CanStartAttack = 12,
    HasTarget = 13,
    IsAlive = 14,
    IsPlayerDead = 15,
    IsSurrounded = 16,
    IsWithingAttackRange = 17,
    AreEnemiesArrivingToAdjacentTile = 18,
    NeedToWaitFirstEnemyArriving = 19
  }
}