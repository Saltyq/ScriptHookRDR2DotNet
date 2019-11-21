using System;
using System.Windows.Forms;
using RDR2;
using RDR2.UI;
using RDR2.Native;
using RDR2.Math;

namespace ExampleScript
{
    public class Main : Script
    {
        public Main()
        {
            KeyDown += OnKeyDown;
            Tick += OnTick;

            Interval = 1;
        }

        bool ragdoll = false;

        private void OnTick(object sender, EventArgs e)
        {
            Player player = Game.Player;
            Ped playerPed = player.Character;
            
            if (ragdoll)
            {
                Function.Call(Hash.SET_PED_TO_RAGDOLL, playerPed, 5000, 5000, 0, false, false, false);
            }
            
        } 

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C)
            {
                ragdoll = !ragdoll;
            }
        }
        
    }
}
