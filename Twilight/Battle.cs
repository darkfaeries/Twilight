using System;
using System.Collections.Generic;

namespace Twilight;

public enum Outcome
{
    AttackerWins,
    DefenderWins,
    Draw
}

public class Battle
{
    private readonly Army _attacker;
    private readonly Army _defender;

    public Dictionary<Outcome, uint> Results { get; set; }

    public Battle(Army atk, Army def)
    {
        _attacker = atk;
        _defender = def;
        Results = new Dictionary<Outcome, uint>();
    }

    private Outcome Simulate()
    {
        var a = new Army(_attacker);
        var d = new Army(_defender);
        
        // AFB phase
        
        a.AssignHitsFightersOnly(d.ProduceAFBHits());
        d.AssignHitsFightersOnly(a.ProduceAFBHits());
        
        
        /*
        Console.WriteLine("=================== AFB =======================");
        Console.WriteLine("Attacker army:");
        Console.WriteLine(a);
        Console.WriteLine("Defender army:");
        Console.WriteLine(d);*/

        
        // TODO remove debug and set montecarlo to 10000

        //var i = 1;
        
        while (true)
        {
            // debug
            
            //Console.WriteLine($"================ Round {i} ====================");
            
            // producing hits independently
            
            var aHits = a.ProduceHits();
            //Console.WriteLine($"attacker got {aHits}");
            var dHits = d.ProduceHits();
            //Console.WriteLine($"defender got {dHits}");
            
            // assigning hits

            var aDead = a.AssignHits(dHits);
            var dDead = d.AssignHits(aHits);
            
            // debug
            
            //if (aDead) Console.WriteLine("atk is dead");
            //if (dDead) Console.WriteLine("def is dead");
            
            /*Console.WriteLine("Attacker army:");
            Console.WriteLine(a);
            Console.WriteLine("Defender army:");
            Console.WriteLine(d);*/
            //i += 1;
            
            // returning outcome (or continuing)

            if (aDead && dDead)
            {
                //Console.WriteLine("draw");
                return Outcome.Draw;
            }
            if (aDead)
            {
                //Console.WriteLine("defender wins");
                return Outcome.DefenderWins;
            }

            if (dDead)
            {
                //Console.WriteLine("attacker wins");
                return Outcome.AttackerWins;
            }
        }
    }

    public void Montecarlo(uint iterations)
    {
        for (uint i = 0; i < iterations; i++)
        {
            Outcome result = Simulate();
            if (!Results.TryAdd(result, 1)) Results[result]++;
        }
    }
}