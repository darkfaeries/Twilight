using System;
using System.Reactive.PlatformServices;
using Avalonia.Animation;

namespace Twilight;

public class Army
{
    private Faction _faction = Faction.Generic;
    
    // Current army composition

    private byte Warsuns { get; set; } = 0;
    private bool Flagship { get; set; } = false;
    private byte Dreadnoughts { get; set; } = 0;
    private byte Cruisers { get; set; } = 0;
    private byte Destroyers { get; set; } = 0;
    private byte Carriers { get; set; } = 0;
    private byte Fighters { get; set; } = 0;
    
    // Army upgrades

    private bool WarsunUp { get; } = false;
    private bool FlagshipUp { get; } = false;
    private bool DreadnoughtUp { get; } = false;
    private bool CruiserUp { get; } = false;
    private bool DestroyerUp { get; } = false;
    private bool CarrierUp { get; } = false;
    private bool FighterUp { get; } = false;
    
    // Hit chances (-1 to what is shown in-game)

    private const int Dice = 10;

    private const int WarsunThreshold = 2;
    private int FlagshipThreshold => FlagshipUp ? 4 : 6;
    private const int DreadnoughtThreshold = 4;
    private int CruiserThreshold => CruiserUp ? 5 : 6;
    private int DestroyerThreshold => DestroyerUp ? 7 : 8;
    private const int CarrierThreshold = 8;
    private int FighterThreshold => FighterUp ? 7 : 8;
    
    // Number of hits

    private const byte WarsunHits = 3;
    private const byte FlagshipHits = 2;
    private const byte DreadnoughtHits = 1;
    private const byte CruiserHits = 1;
    private const byte DestroyerHits = 1;
    private const byte CarrierHits = 1;
    private const byte FighterHits = 1;
    
    // Resource costs

    private double WarsunCost => (WarsunUp && _faction == Faction.Muaat) ? 10.0 : 12.0;
    private const double FlagshipCost = 8.0;
    private const double DreadnoughtCost = 4.0;
    private const double CruiserCost = 2.0;
    private const double DestroyerCost = 1.0;
    private const double CarrierCost = 3.0;
    private const double FighterCost = 0.5;
    
    // Capacity

    private const byte WarsunCapacity = 6;
    private const byte FlagshipCapacity = 3; // TODO every flagship is specific
    private byte DreadnoughtCapacity => _faction == Faction.L1Z1X ? (byte)2 : (byte)1;
    private byte CruiserCapacity => CruiserUp ? (byte)1 : (byte)0;

    private byte CarrierCapacity
    {
        get
        {
            if (_faction == Faction.Sol)
            {
                if (CarrierUp) return 9;
                return 6;
            }
            else
            {
                if (CarrierUp) return 6;
                return 4;
            }
        }
    }

    private byte DestroyerCapacity => _faction == Faction.Argent ? (byte)1 : (byte)0;
    
    // Shield info
    
    private byte WarsunShields { get; set; }
    private bool FlagshipShield { get; set; }
    private byte DreadnoughtShields { get; set; }
    private byte CruiserShields { get; set; }
    private byte CarrierShields { get; set; }
    
    // AFB

    private int AFBThreshold => DestroyerUp ? 5 : 8;
    private byte AFBInstances => DestroyerUp ? (byte)3 : (byte)2;
    
    // constructor from fields

    public Army()
    {
        
    }

    public Army(Faction fact, byte ws, bool fl, byte dn, byte c, byte d, byte ca, byte f,
        bool flu, bool dnu, bool cu, bool du, bool cau, bool fu)
    {
        _faction = fact;
        Warsuns = ws;
        Flagship = fl;
        Dreadnoughts = dn;
        Cruisers = c;
        Destroyers = d;
        Carriers = ca;
        Fighters = f;
        FlagshipUp = flu;
        DreadnoughtUp = dnu;
        CruiserUp = cu;
        DestroyerUp = du;
        CarrierUp = cau;
        FighterUp = fu;

        WarsunShields = Warsuns;
        FlagshipShield = Flagship;
        DreadnoughtShields = Dreadnoughts;
        if (fact == Faction.Ul && CruiserUp) CruiserShields = Cruisers;
        if (fact == Faction.Sol && CarrierUp) CarrierShields = Carriers;
    }
    
    // copy constructor

    public Army(Army a)
    {
        _faction = a._faction;
        Warsuns = a.Warsuns;
        Flagship = a.Flagship;
        Dreadnoughts =  a.Dreadnoughts;
        Cruisers = a.Cruisers;
        Destroyers = a.Destroyers;
        Carriers = a.Carriers;
        Fighters = a.Fighters;
        FlagshipUp = a.FlagshipUp;
        DreadnoughtUp = a.DreadnoughtUp;
        CruiserUp = a.CruiserUp;
        DestroyerUp = a.DestroyerUp;
        CarrierUp = a.CarrierUp;
        FighterUp = a.FighterUp;

        WarsunShields = Warsuns;
        FlagshipShield = Flagship;
        DreadnoughtShields = Dreadnoughts;
    }
    
    // roll hits for 1 round of combat
        
    public byte ProduceHits()
    {
        byte hits = 0;
        var rand = new Random((int)SystemClock.UtcNow.Ticks);

        hits += GetHits(Warsuns, WarsunHits, WarsunThreshold, rand);
        hits += GetHits(Flagship ? (byte)1 : (byte)0, FlagshipHits, FlagshipThreshold, rand);
        hits += GetHits(Dreadnoughts, DreadnoughtHits, DreadnoughtThreshold, rand);
        hits += GetHits(Cruisers, CruiserHits, CruiserThreshold, rand);
        hits += GetHits(Destroyers, DestroyerHits, DestroyerThreshold, rand);
        hits += GetHits(Carriers, CarrierHits, CarrierThreshold, rand);
        hits += GetHits(Fighters, FighterHits, FighterThreshold, rand);

        return hits;
    }
    
    // roll AFB

    public byte ProduceAFBHits()
    {
        var rand = new Random();
        return GetHits(Destroyers, AFBInstances, AFBThreshold, rand);
    }
    
    // assign hits only to fighters (AFB result)

    public void AssignHitsFightersOnly(byte hits)
    {
        if (Fighters < hits) Fighters = 0;
        else Fighters -= hits;
    }
    
    // assign hits to army end-of-round;
    // return value true means the army is dead after the assignment

    public bool AssignHits(byte hits)
    {
        // hit assignment priority
        
        // -1. Cruiser shields
        
        if (CruiserShields > hits)
        {
            CruiserShields -= hits;
            return false;
        }
        hits -= CruiserShields;
        CruiserShields = 0;
        
        // 0. Carrier shields
        
        if (CarrierShields > hits)
        {
            CarrierShields -= hits;
            return false;
        }
        hits -= CarrierShields;
        CarrierShields = 0;
        
        // 1. Fighters
        
        if (Fighters > hits)
        {
            Fighters -= hits;
            return false;
        }
        hits -= Fighters;
        Fighters = 0;
        
        
        // 2. Destroyers
        
        if (Destroyers > hits)
        {
            Destroyers -= hits;
            return false;
        }
        hits -= Destroyers;
        Destroyers = 0;
        
        
        // 3. Dread shields
        
        if (DreadnoughtShields > hits)
        {
            DreadnoughtShields -= hits;
            return false;
        }
        hits -= DreadnoughtShields;
        DreadnoughtShields = 0;
        
        
        // 4. Cruisers
        
        if (Cruisers > hits)
        {
            Cruisers -= hits;
            return false;
        }
        hits -= Cruisers;
        Cruisers = 0;
        
        
        // 5. Carriers
        
        if (Carriers > hits)
        {
            Carriers -= hits;
            return false;
        }
        hits -= Carriers;
        Carriers = 0;
        
        
        // 6. Dreadnoughts
        
        if (Dreadnoughts > hits)
        {
            Dreadnoughts -= hits;
            return false;
        }
        hits -= Dreadnoughts;
        Dreadnoughts = 0;
        
        
        // 7. Flagship shield
        
        if (FlagshipShield)
        {
            if (hits == 0)
            {
                return false;
            }
            hits--;
            FlagshipShield = false;
        }
        
        // 8. Warsun shields
        
        if (WarsunShields > hits)
        {
            WarsunShields -= hits;
            return false;
        }
        hits -= WarsunShields;
        WarsunShields = 0;
        
        
        // 9. Flagship
        
        if (Flagship)
        {
            if (hits == 0)
            {
                return false;
            }
            hits--;
            Flagship = false;
        }
        
        // 10. Warsuns
        
        if (Warsuns > hits)
        {
            Warsuns -= hits;
            return false;
        }

        hits -= Warsuns;
        Warsuns = 0;

        return true;
    }

    // for statistics: gets total resource cost of the army
    public double GetResourceCost()
    {
        double resources = 0;
        resources += WarsunCost * Warsuns;
        if (Flagship) resources += FlagshipCost;
        resources += DreadnoughtCost * Dreadnoughts;
        resources += CruiserCost * Cruisers;
        resources += DestroyerCost * Destroyers;
        resources += CarrierCost * Carriers;
        resources += FighterCost * Fighters;
        return resources;
    }
    
    // for statistics: gets the army's average number of hits per round

    public double GetAverageHits()
    {
        double average = 0;
        average += Warsuns * WarsunHits * ((Dice - WarsunThreshold) / (double)Dice);
        if (Flagship) average += FlagshipHits * (Dice - FlagshipThreshold) / (double)Dice;
        average += Dreadnoughts * DreadnoughtHits * (Dice - DreadnoughtThreshold) / (double)Dice;
        average += Cruisers * CruiserHits * (Dice - CruiserThreshold) / (double)Dice;
        average += Destroyers * DestroyerHits * (Dice - DestroyerThreshold) / (double)Dice;
        average += Carriers * CarrierHits * (Dice - CarrierThreshold) / (double)Dice;
        average += Fighters * FighterHits * (Dice - FighterThreshold) / (double)Dice;
        return average;
    }
    
    // for statistics: gets the army's average number of AFB hits

    public double GetAverageAFBHits()
    {
        return Destroyers * AFBInstances * (Dice - AFBThreshold) / (double)Dice;
    }
    
    // for statistics: gets the army's maximum and actual capacity

    public byte GetMaxCapacity()
    {
        byte capacity = 0;
        capacity += (byte)(Warsuns * WarsunCapacity);
        if (Flagship) capacity += FlagshipCapacity;
        capacity += (byte)(Dreadnoughts * DreadnoughtCapacity);
        capacity += (byte)(Cruisers * CruiserCapacity);
        capacity += (byte)(Carriers * CarrierCapacity);
        capacity += (byte)(Destroyers * DestroyerCapacity);
        return capacity;
    }
    
    // for statistics: gets the army's effective HP

    public byte GetHitpoints()
    {
        byte hp = 0;
        if (Flagship) hp++;
        if (FlagshipShield) hp++;
        return (byte)(Warsuns + WarsunShields +
                      Dreadnoughts + DreadnoughtShields +
                      Cruisers + CruiserShields +
                      Carriers + CarrierShields +
                      Destroyers + Fighters + hp);
    }
    
    // debug toString Method

    public override string ToString()
    {
        return
            $"WS: {Warsuns} FL: {Flagship} DN:{Dreadnoughts} C:{Cruisers} D:{Destroyers} CA:{Carriers} F:{Fighters}\n" +
            $"WS Shields: {WarsunShields} FL Shield: {FlagshipShield} DN Shields: {DreadnoughtShields}";
    }

    // helper static method

    private static byte GetHits(byte number, byte shipHits, int threshold, Random rand)
    {
        byte hits = 0;

        for (int i = 0; i < number * shipHits; i++)
        {
            if (rand.Next(Dice) >= threshold) hits++;
        }

        return hits;
    }
}