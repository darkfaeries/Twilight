namespace Twilight;

public enum Faction
{
    Generic,
    
    /*
    If the user wants a generic faction with no benefits.
    Used by the default Army constructor.
     */
 
 
    Sol,
    
    
    /*
    1. Carrier unique unit and upgrade
    Carrier 1 - 6 capacity instead of 4
    Carrier 2 - 9 capacity instead of 6 and shields
    2. Infantry unique unit and upgrade
    Infantry 1 - 7 combat instead of 8
    Infantry 2 - 6 combat instead of 7
    3. Commander - places 1 infantry on a planet that is attacked
    */
    
    
    Sardakk,
    
    /*
    1. +1 combat for all units (including GF)
    2. +1 combat for all ships if flagship is in the system
    3. Tech - Valkyrie Particle Weave (???)
    4. Dreadnought unique unit and upgrade
    Dread 1 - 2 bombardments on 5 instead of 1
    Dread 2 - 2 bombardments on 4 instead of 1, also:
    Can destroy a Dread end of round to take out 2 enemy ships of your choice
    5. Mech - when uses shield, produces 1 additional hit
    6. Promissory - Tekklar Legion:
    Used in combat to grant +1 to all rolls (works only in space)
    If used against Sardakk, their faction bonus is not applied
    */
    
    
    Letnev,
    
    /*
    1. +2 fleet pool bonus (Armada)
    2. Can spend 2 TGs to reroll all dice in this round (maybe?)
    3. Tech: L4 disruptors. Opponents can't use space cannon against you?
    4. Tech: Non-Euclidean shielding. Shield cancels 2 hits instead of 1.
    5. Promissory: Letnev spends TGs so that the promissory controller can reroll dice.
    6. Flagship: rolls 2 on 5, bombardment 2 on 5, cancels Planetary Shield, heals every round.
    */
    
    
    Xxcha,
    
    /*
    1. Flagship: has 3 space cannons on 5 and can fire to adjacent systems.
    2. Mech: has 1 space cannon on 8 that can fire to adjacent systems. (?)
    */
    
    
    Hacan,
    
    /*
    1. Flagship: can spend TGs to do *something*
    */
    
    
    JolNar,
    
    /*
    1. Faction disadvantage: -1 to all dice rolls.
    2. Flagship: produces 3 hits instead of 1 if it rolls 9 or 10.
    3. Mech: if it's on the planet, infantry does't have the faction disadvantage.
     */
    
    L1Z1X,
    
    /*
    1. Faction ability: Harrow. Units with Bombardment can bombardment in
    the end of each ground combat round.
    2. Unique dread and upgrade.
    Dread 1 - has 2 capacity instead of 1
    Dread 2 - has 2 capacity instead of 1, combat 4 instead of 5 (maybe bombards better also?)
     */
    
    Creuss,
    
    /*
    it seems that they have no combat bonuses but i'm obviosly wrong.
     */
    
    Yssaril,
    
    /*
    1. Agent - can be any agent on the board (potentially Titans, Sardakk, Nomad etc)
    2. Flagship - has 2 movement and lightwave. (hardly relevant)
     */
    
    Nekro,
    
    /*
    1. Can assimilate others' faction techs to get some advantage.
    2. Flagship - is really cool (you can assign hits to infantry in space maybe?)
     */
    
    Naalu,
    
    /*
    1. Fighter unique upgrade and unit.
    Fighter 1 - 8 combat instead of 9.
    Fighter 2 - 7 combat instead of 8, uses up only 1/2 of fleet pool when not carried.
    2. Flagship - lets you use fighters to invade planets.
    You must still have infantry survived to get tha planet.
     */
    
    Arborec,
    
    /*
    1. Unique infantry unit and upgrade.
    It just has production (irrelevant now).
    2. Commander - can produce 1 ship in a system with production when it's activated.
    */
    
    Mentak,
    
    /*
    1. Ambush - some extra hits from cruisers or destroyers at the beginning of combat. (clarification needed)
    2. Flagship - cancels enemy space cannon.
    3. Hero - opponent's dead ships are resurrected as yours mid-combat.
     */
    
    Yin,
    
    /*
    1. Can detonate a cruiser or destroyer end-of-round to get 2 hits (or kill an enemy ship?)
    2. Indoctrination - can spend 2 influence at the start of ground combat to add 1 infantry and remove 1 from opponents.
     */
    
    Muaat,
    
    /*
    1. Warsun can be upgraded to cost only 10 (and improved speed).
    2. Hero - can annihilate any system with a warsun. (how to implement?)
     */
    
    Winnu,
    
    /*
    1. Commander - adds +2 to combat in your HS, Mecatol Rex or sys with legendary planets.
    2. Flagship - number of hits is equal to the number of enemy non-fighter ships in the system.
    */
    
    Saar,
    
    /*
    1. Flaghip - has AFB (4 on 6 or smth)
    2. Base - does it have a combat value?
     */
    
    NaazRokha,
    
    /*
    1. Mech - can be used in space combat (2 on 8).
    2. Flaghip - mechs in this system roll an additional die.
    3. Faction tech - 1-round morale boost (bio-stims in mind)
     */
    
    Ul,
    
    /*
    1. Unique Cruiser unit and tech.
    Cruiser 1 - has Capacity 1 and combat 6 instead of 7 (?)
    Cruiser 2 - has shields. (?)
    2. Unique PDS unit and tech.
    PDS 1 - has Production, shield and participates in ground combat (1 on 8).
    PDS 2 - has Production, shield and participates in ground combat (1 on 7)?
    3. Agent - cancels 1 hit in a battle (ground or space-only?)
    4. Hero - HS gets 3 space cannon on 5 abilities.
     */
    
    Cabal,
    
    /*
    1. Capture enemy units. (irrelevant?)
    2. SDs have 6 capacity (12 with upgrade) instead of the usual 3. (?)
     */
    
    Empyrean,
    
    /*
    1. has no combat-related abilities?
     */
    
    Nomad,
    
    /*
    1. Flaghip - unique unit and tech.
    Flagship 1 - pretty much usual.
    Flagship 2 - carry space increased to 6 (?), has 3 AFB on 6 (?), combat increased to 5 (?).
    2. Promissory - during combat, treat a random non-fighter ship as Nomad flagships in terms of stats and abilities(?).
    3. Agent - if used after a round of combat, both sides reroll all dice.
    4. Faction tech to ready any agent (irrelevant mid-fight).
     */
    
    Mahact,
    
    /*
    1. Can have abilities of other players' commanders. (hard to implement)
    2. Flagship: rolls 2 on 5, if the enemy is not edicted yet, gets additional +2.
    3. Mech: can stop the opponent's army (to not forget about it.)
     */
    
    Argent
    
    /*
    1. Destroyer unique unit and upgrade.
    Destroyer 1 - has capacity 1, combat on 8 instead of 9.
    Destroyer 2 - has capacity 1, combat on 7 instead of 8, ???
    2. Commander - +1 die for unit ability rolls (bombardment, space cannon and AFB)
     */
}