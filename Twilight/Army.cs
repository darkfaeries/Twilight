using System;
using System.Reactive.PlatformServices;
using Avalonia.Animation;

namespace Twilight;

public class Army
{
    // Current army composition

    private byte Warsuns { get; set; } = 0;
    private bool Flagship { get; set; } = false;
    private byte Dreadnoughts { get; set;} = 0;
    private byte Cruisers { get; set;} = 0;
    private byte Destroyers { get; set;} = 0;
    private byte Carriers { get; set;} = 0;
    private byte Fighters { get; set;} = 0;
    
    // Army upgrades
    
    private bool FlagshipUp { get; set;} = false;
    private bool DreadnoughtUp { get; set;} = false;
    private bool CruiserUp { get; set;} = false;
    private bool DestroyerUp { get; set;} = false;
    private bool CarrierUp { get; set;} = false;
    private bool FighterUp { get; set;} = false;
    
    // Hit chances (-1 to what is shown in-game)

    private const int Dice = 10;

    private const int WarsunThreshold = 2;
    private int FlagshipThreshold => FlagshipUp ? 4 : 6;
    private int DreadnoughtThreshold => DreadnoughtUp ? 3 : 4;
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
    
    // Shield info
    
    private byte WarsunShields { get; set; }
    private bool FlagshipShield { get; set; }
    private byte DreadnoughtShields { get; set; }
    
    // AFB

    private int AFBThreshold => DestroyerUp ? 5 : 8;
    private byte AFBInstances => DestroyerUp ? (byte)3 : (byte)2;
    
    // constructor from fields

    public Army(byte ws, bool fl, byte dn, byte c, byte d, byte ca, byte f,
        bool flu, bool dnu, bool cu, bool du, bool cau, bool fu)
    {
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
    }
    
    // copy constructor

    public Army(Army a)
    {
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
        
        // 1.Fighters
        
        if (Fighters > hits)
        {
            Fighters -= hits;
            return false;
        }
        hits -= Fighters;
        Fighters = 0;
        
        
        // 2.Destroyers
        
        if (Destroyers > hits)
        {
            Destroyers -= hits;
            return false;
        }
        hits -= Destroyers;
        Destroyers = 0;
        
        
        // 3.Dread shields
        
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