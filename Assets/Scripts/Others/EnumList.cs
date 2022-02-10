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
public enum Stat {Health, Cost, Damage, Rest, BulletSpeed, BurstSize, Projectiles, BulletSize, Duration, Capacity, DroneLevel, Rate, FuseTime}
public enum BaseEffectTrigger {Immediate, StartOfWave, EndOfWave, OnDestruction, None, Special, OnLevelUp, OnHit}
public enum WeaponClass {Artillery, Shotgun, Bomber, Beamer, Spreader, Spawner, Healer, Default, Enhancer}
public enum Keyword {None, Slug, Burn, Shield}

//. GAME STATE
public enum GameState {OnWave, OnReward, OnTitle, OnPause}

//. GENERAL
public enum Direction {Up, Down, Right, Left, None}