using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Twilight;

public partial class MainWindow : Window
{
    private Army attacker = new Army();
    private Army defender = new Army();
    
    public MainWindow()
    {
        InitializeComponent();
    }

    private void SimulateClick(object? sender, RoutedEventArgs e)
    {
        // reading ship numbers and upgrades from UI (not MVVM)
        
        // TODO do UI using Binding (this is useful)
        
        /*Army attacker = new Army((Faction)AtkFact.Value, (byte)AtkWs.Value, (bool)AtkFl.IsChecked!, (byte)AtkDn.Value, (byte)AtkC.Value, (byte)AtkD.Value, (byte)AtkCa.Value,
            (byte)AtkF.Value,(bool)AtkFlUp.IsChecked!, (bool)AtkDnUp.IsChecked!, (bool)AtkCUp.IsChecked!, (bool)AtkDUp.IsChecked!, (bool)AtkCaUp.IsChecked!,
            (bool)AtkFUp.IsChecked!);
        Army defender = new Army((Faction)DefFact.Value, (byte)DefWs.Value, (bool)DefFl.IsChecked!, (byte)DefDn.Value, (byte)DefC.Value, (byte)DefD.Value, (byte)DefCa.Value,
            (byte)DefF.Value,(bool)DefFlUp.IsChecked!, (bool)DefDnUp.IsChecked!, (bool)DefCUp.IsChecked!, (bool)DefDUp.IsChecked!, (bool)DefCaUp.IsChecked!,
            (bool)DefFUp.IsChecked!);*/
        

        Battle battle = new Battle(attacker, defender);
        uint montecarloIterations = 10000;
        
        // simulating 10000 results

        StandbyLabel.Content = "Симуляция битв...";
        StandbyLabel.IsVisible = true;
        
        //Console.Clear();
        battle.Montecarlo(montecarloIterations);
        
        // displaying results

        StandbyLabel.Content = "Результаты:";

        battle.Results.TryGetValue(Outcome.AttackerWins, out var atkWins);
        battle.Results.TryGetValue(Outcome.DefenderWins, out var defWins);
        battle.Results.TryGetValue(Outcome.Draw, out var draw);
        

        AtkWinsLabel.Content = $"Победа атакующего: {atkWins * 100.0 / montecarloIterations} %";
        DefWinsLabel.Content = $"Победа защитника: {defWins * 100.0 / montecarloIterations} %";
        DrawLabel.Content = $"Ничья: {draw * 100.0 / montecarloIterations} %";
    }

    private void RecalculateArmies()
    {
        Faction atkFact = Faction.Argent;
        Faction defFact = Faction.Hacan;
        
        attacker = new Army(atkFact, (byte)AtkWs.Value, (bool)AtkFl.IsChecked!, (byte)AtkDn.Value, (byte)AtkC.Value, (byte)AtkD.Value, (byte)AtkCa.Value,
            (byte)AtkF.Value,(bool)AtkFlUp.IsChecked!, (bool)AtkDnUp.IsChecked!, (bool)AtkCUp.IsChecked!, (bool)AtkDUp.IsChecked!, (bool)AtkCaUp.IsChecked!,
            (bool)AtkFUp.IsChecked!);
        defender = new Army(defFact, (byte)DefWs.Value, (bool)DefFl.IsChecked!, (byte)DefDn.Value, (byte)DefC.Value, (byte)DefD.Value, (byte)DefCa.Value,
            (byte)DefF.Value,(bool)DefFlUp.IsChecked!, (bool)DefDnUp.IsChecked!, (bool)DefCUp.IsChecked!, (bool)DefDUp.IsChecked!, (bool)DefCaUp.IsChecked!,
            (bool)DefFUp.IsChecked!);

        AtkResLabel.Content = attacker.GetResourceCost();
        DefResLabel.Content = defender.GetResourceCost();

        AtkHpLabel.Content = attacker.GetHitpoints();
        DefHpLabel.Content = defender.GetHitpoints();

        AtkRHitsLabel.Content = attacker.GetAverageHits();
        DefRHitsLabel.Content = defender.GetAverageHits();

        AtkAfbLabel.Content = attacker.GetAverageAFBHits();
        DefAfbLabel.Content = defender.GetAverageAFBHits();
    }

    private void NumUpDownValChangeWrapper(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        RecalculateArmies();
    }

    private void CheckBoxRecalcWrapper(object? sender, RoutedEventArgs e)
    {
        RecalculateArmies();
    }
}