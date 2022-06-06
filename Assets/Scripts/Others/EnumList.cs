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
public enum Stat {Health, Cost, Damage, Rest, BulletSpeed, BurstCount, Projectiles, Size, Duration, Capacity, DroneLevel, Rate, FuseTime, Efficiency, Triggers}
public enum EffectTrigger {Immediate, StartOfWave, EndOfWave, OnDestruction, None, Special, OnLevelUp, OnHit, OnTurretBuild, OnTurretSell, OnEnemyDefeat}
public enum Keyword {None, Slug, Acid, Shield, Weaken, Expose, Stun}
public enum ShipAttribute { MaxHealth, HealthRegen, DamageReduction, MovementSpeed, TurnSpeed, EnergyGain }

public enum WeaponClass 
{
    Artillery,
    Bomber,
    ChemicalWeapon,
    Commander,
    EnergyCaster,
    Shotgun,
    Support
}

//. GAME STATE
public enum GameState {OnWave, OnReward, OnTitle, OnPause, OnEndgame}

//. GENERAL
public enum Direction {Up, Down, Right, Left, None}