using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;
using System.Windows.Forms;

namespace NoClipNoVirus
{
    public class Main : Script
    {
        public Main()
        {
            Tick += this.UpdateThis;
            KeyDown += this.OnKey;
            Globals.InitGlobals();
        }

        #region Variables
        bool Activated = false;
        float RangeANDSpeed = 1.0f;
        Ped MyPed { get { return Game.Player.Character; } }
        Vehicle MyVehicle { get { return MyPed.CurrentVehicle; } }
        #endregion

        #region Handlers
        public void OnKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Globals.ActivateKey)
                SwitchActivation();
            if (e.KeyCode == Globals.SpeedUpKey && Activated)
                SwitchSpeed(true);
            if (e.KeyCode == Globals.SpeedDownKey && Activated)
               SwitchSpeed(false);
        }
        void UpdateThis(object sender, EventArgs e)
        {
            if (Function.Call<bool>(Hash.IS_DISABLED_CONTROL_PRESSED, 2, 203) && Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 2, 209))
                SwitchActivation();
            if (Activated)
            {
                UpdateNoClip();
            }
        }
        #endregion

        #region Methods
        void UpdateNoClip()
        {
            //Low level function by ap ii intense

            Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 26, true);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 79, true);
            Function.Call(Hash.DISABLE_CONTROL_ACTION, 2, 37, true);

            if (Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 2, 227))
                SwitchSpeed(true);
            else if (Function.Call<bool>(Hash.IS_DISABLED_CONTROL_JUST_PRESSED, 2, 226))
                SwitchSpeed(false);


            bool UsingVehicle = false;
            int IDToFly = 0;
            if (UsingVehicle = MyPed.IsInVehicle())
                IDToFly = MyVehicle.Handle;
            else
                IDToFly = MyPed.Handle;
           

            if (!UsingVehicle)
            {
                if (Function.Call<bool>(Hash.GET_PED_STEALTH_MOVEMENT, IDToFly))
                    Function.Call(Hash.SET_PED_STEALTH_MOVEMENT, IDToFly, 0, 0);
                if (Function.Call<bool>(Hash.GET_PED_COMBAT_MOVEMENT, IDToFly))
                    Function.Call(Hash.SET_PED_COMBAT_MOVEMENT, IDToFly, 0);
            }

            Function.Call(Hash.SET_ENTITY_HEADING, IDToFly, GameplayCamera.Rotation.Z);
            GameplayCamera.RelativeHeading = 0f;
            Function.Call(Hash.SET_GAMEPLAY_CAM_RELATIVE_PITCH, GameplayCamera.RelativePitch, 0f);
            Function.Call(Hash.FREEZE_ENTITY_POSITION, IDToFly, true);

            float LeftAxisX = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 218);
            float LeftAxisY = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 219);
            float lt = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 207);
            float rt = Function.Call<float>(Hash.GET_CONTROL_NORMAL, 2, 208);

            float newLeftAxisY = LeftAxisY * -1.0f * RangeANDSpeed;
            float newLt = lt * -1.0f * RangeANDSpeed;
            float newRt = rt * RangeANDSpeed + newLt;

            if (!Keyboard.IsKeyDown(Globals.MoveUpKey) && !Keyboard.IsKeyDown(Globals.MoveDownKey))
            {
                if (LeftAxisX == 0 && LeftAxisY == 0 && lt == 0 && rt == 0)
                    return;
            }
            else
            {
                if (Keyboard.IsKeyDown(Globals.MoveUpKey))
                    newRt = 1.0f * RangeANDSpeed;
                if (Keyboard.IsKeyDown(Globals.MoveDownKey))
                    newRt = -1.0f * RangeANDSpeed;
            }

            Vector3 unitCircle = Function.Call<Vector3>(Hash.GET_OFFSET_FROM_ENTITY_IN_WORLD_COORDS, IDToFly, LeftAxisX * RangeANDSpeed, newLeftAxisY, newRt);
            Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, IDToFly, unitCircle.X, unitCircle.Y, unitCircle.Z, 0, 0, 0);
        }

        void SwitchActivation()
        {
            GTA.UI.ShowSubtitle(string.Format("No Clip {0}!", ((Activated = !Activated) ? "~g~Activated" : "~r~Deactivated")));
            Function.Call(Hash.SET_PED_STEALTH_MOVEMENT, MyPed.Handle, 0, 0);
            Entity EntityToMod = MyPed.IsInVehicle() ? (Entity)MyPed.CurrentVehicle : (Entity)MyPed;
            if (!Activated)
            {
                EntityToMod.FreezePosition = false;
                if (MyPed.IsInVehicle())
                    EntityToMod.ApplyForce(Vector3.Zero);
            }
            else
                EntityToMod.Position = EntityToMod.Position;
                
        }
        void SwitchSpeed(bool inc)
        {
            float[] speeds = { 1.0f, 4.0f, 8.0f, 16.0f, 64.0f };
            string[] speedNames = { "Normal", "Fast", "Super", "Extreme", "Beyond Extreme" };
            int currentSpeed = Array.IndexOf(speeds, RangeANDSpeed);
            if (inc)
            {
                if (currentSpeed == speeds.Length - 1)
                    currentSpeed = 0;
                else
                    currentSpeed++;
            }
            else
            {
                if (currentSpeed == 0)
                    currentSpeed = speeds.Length - 1;
                else
                    currentSpeed--;
            }
            RangeANDSpeed = speeds[currentSpeed];
            GTA.UI.ShowSubtitle(string.Format("Speed: ~g~{0}", speedNames[currentSpeed]));
        }
        #endregion
    }
}
