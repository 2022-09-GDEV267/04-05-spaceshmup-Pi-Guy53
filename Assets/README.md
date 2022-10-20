To extend the game I have a few ideas. Additional ship types, such as a boss, or a support ship
(one that heals, or boosts the damage of other ships)
and I would like to add some addition weapons including missiles, and/or some form of AOE weapon.
Phasers/Lasers also seem like they would be intersting to have.

Remaking the environment so that the player can fly around in any direction rather than constantly 'moving forward'
would be fun gameplay, but may take too long to implement. I have a working concept, but will wait until our mash up
to impliment.

I added 2 additional shots to the spread weapon, bringing it up to 5 per shot.
The blaster now shoots 20 shots per second, each dealing .5 damage. DPS = 10
The spread weapon shots 12.5 shots per second, each dealing 1 damage. DPS = 12.5
I balanced it as such because the spread weapon often misses most of its shots, and so by giving it a higher DPS than the blaster
it stays competative late game. The spread really only deals max damage at extreme close range, which is dangerous for the player
also added a new Cannon weapon. Massive single shot damage, low fire rate. Currently does 8 damage per shot, with a fire delay of .8
Cannon DPS = 10 While unweildly in low quantites, this weapon can become OP with 2+ weapons equipped.

//Final extension//
Weapons: Phaser, Missile, Laser
    Phaser, twin lasers that move on a sin wave, DPS = 10
    Laser, heavier version of hte default blaster, DPS = 10
    Missile, tracks a random enemy ship, DPS = 4

Turrets: turrets auto track the player, slowly rotating to face them. There are two modes, one is where the turret uses the Ships
    weapon information (weapon type, rate of fire, and jam chance), the other is where the turret uses its own weapon information,
    becoming an auto turret

Enemies:

    Most enemy ships are now equipped with weapons to shoot at the player.

    Added Enemy_5 a boss/dreadnought enemy
    Enemy_5 has 5 destroyable sections before its main hull can be damaged. It has two mounted laser turrets, two mounted blaster turrets
    and 1 fixed spread gun. At random intervals it will shoot out a ring of lasers in a sort of 'explosion'

    After the players average score, and current score have reached a threashhold, then boss will spawn. When the boss is deplyed
    no other enemy ships, besides enemy 2 will spawn (enemy 2 has no weapons, and a 50% powerup drop chance) and enemy 2 will spawn
    at half the normal rate. 

Player:
    The player has a shield strength which is depleted as the player takes damge from enemy weapons, crashing into an enemy will still
    drop one full level of shields. This allows the player ship to take some fire from without being destroyed immediatly.
    This shield strength will regenerate after a short duration of not taking any damage, meaning a smart player can keep their shields up