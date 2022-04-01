//. ESQUEMAS DE CONTROLE
public enum MovementControlScheme {None, WASD, Arrows}
public enum RotationControlScheme {None, QE, Mouse}

//.WAVE/FORMATION/ENEMY/BOSS RELATED
public enum WaveType {Normal, Elite, Boss}
public enum EnemyType {Shooter}
public enum WigglePattern 
{
    HorizontalSine, 
    VerticalSine, 
    HorizontalCosine, 
    VerticalCosine, 
    ClockwiseCircle, 
    CounterClockwiseCircle
}


//. TURRET/REWARD RELATED
public enum RewardLevel {Common, Uncommon, Rare, Unique, Error}
public enum Stat {Health, Cost, Damage, Rest, BulletSpeed, BurstSize, Projectiles, Size, Duration, Capacity, DroneLevel, Rate, FuseTime, Efficiency, Triggers}
public enum EffectTrigger {Immediate, StartOfWave, EndOfWave, OnDestruction, None, Special, OnLevelUp, OnHit, OnTurretBuild, OnTurretSell, OnEnemyDefeat}
public enum WeaponClass {Artillery, Shotgun, Bomber, Beamer, Spreader, Spawner, Healer, Default, Enhancer}
public enum Keyword {None, Slug, Acid, Shield, Weaken, Expose, Stun}

[System.Flags]
public enum WeaponTag 
{
    none = 0,
    damageOverTime = 1, 
    areaOfEffect = 2,
    scatter = 4,
    support = 8,
    focusedAttack = 16,
    debuff = 32,
    explosive = 64,
    piercing = 128, 
}

//. GAME STATE
public enum GameState {OnWave, OnReward, OnTitle, OnPause, OnEndgame}

//. GENERAL
public enum Direction {Up, Down, Right, Left, None}