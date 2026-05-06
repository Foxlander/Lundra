## Systèmes terminés
- GameManager + SceneLoader
- PlayerController (ZQSD)
- CameraController (persistant)
- StatSystem complet
- DamageSystem (critiques + types)
- EnemyBase avec IA simple
- Projectile de base
- SkillManager (4 skills)
- Nova avec VFX
- DoT (Poison, Brûlure, Saignement)
- Hub + Portails
- Mort joueur + respawn
- Persistance joueur/caméra entre scènes

## Prochaines étapes
- HUD (vie, mana, cooldowns)
- Système d'items et loot
- Arbre de talents
- Ennemis variés
- Génération de salles

## Architecture
- _Project/Scripts/Core → GameManager, SceneLoader, BootLoader
- _Project/Scripts/Character/Player → PlayerController, PlayerStats, PlayerDeath, PlayerManager
- _Project/Scripts/Character/Enemy → EnemyBase
- _Project/Scripts/Combat → DamageSystem, DotSystem, DotEffect, DotType
- _Project/Scripts/Combat/Skills → SkillBase, SkillManager, ProjectileSkill, NovaSkill, DotSkill
- _Project/Scripts/Combat/Projectiles → ProjectileBase, DotProjectile
- _Project/Scripts/Stats → StatType, StatModifier, Stat, CharacterStats