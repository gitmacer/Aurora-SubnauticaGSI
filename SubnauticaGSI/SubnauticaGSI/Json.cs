using System;
using UnityEngine;

namespace SubnauticaGSI
{

    public class ProviderNode
    {
        public string name { get; set; }
        public int appid { get; set; }

        public ProviderNode()
        {
            this.name = "subnautica"; this.appid = 264710;
        }
    }

    public class GameStateNode
    {
        public PlayerState game_state { get; set; }

        public GameStateNode()
        {
            this.game_state = AuroraController.state;
        }
    }

    public class PlayerNode
    {
        public string biom { get; set; }

        //public int depth { get; set; } //only depth
        //public int surface_depth  { get; set; } //always 0?
        public int depth_level { get; set; } //also height

        public int health { get; set; }
        public int food { get; set; }
        public int water { get; set; }

        public bool can_breathe { get; set; }
        public int oxygen_capacity { get; set; }
        public int oxygen_available { get; set; }

        //public bool can_item_be_used { get; set; } //cut scenes? does no longer work?

        public PDA.State pda_state { get; set; }

        public bool is_in_water_for_swimming { get; set; }
        //public bool is_swimming { get; set; }

        public Player.MotorMode motor_mode { get; set; }
        public Player.Mode mode { get; set; }
        public Main.GameModes game_mode { get; set; }

        public PlayerNode()
        {
            // set properties in default constructor to generate sample data

            this.biom = Player.main.GetBiomeString();

            //this.depth = Mathf.RoundToInt(Player.main.GetDepth());
            //this.surface_depth = Mathf.RoundToInt(Player.main.GetSurfaceDepth());
            this.depth_level = Mathf.RoundToInt(Player.main.depthLevel);

            this.health = Mathf.RoundToInt(Player.main.liveMixin.health);
            this.food = Mathf.RoundToInt(Player.main.gameObject.GetComponent<Survival>().food);
            this.water = Mathf.RoundToInt(Player.main.gameObject.GetComponent<Survival>().water);

            try
            {
                if (GameModeUtils.IsOptionActive(GameModeOption.Survival))
                    this.game_mode = Main.GameModes.Survival;
                else if (GameModeUtils.IsOptionActive(GameModeOption.Creative))
                    this.game_mode = Main.GameModes.Creative;
                else if (GameModeUtils.IsOptionActive(GameModeOption.Freedom))
                    this.game_mode = Main.GameModes.Freedom;
                else if (GameModeUtils.IsOptionActive(GameModeOption.Hardcore))
                    this.game_mode = Main.GameModes.Hardcore;
            }
            catch { this.game_mode = Main.GameModes.None; }

            this.can_breathe = Player.main.CanBreathe();
            this.oxygen_capacity = Mathf.RoundToInt(Player.main.GetOxygenCapacity());
            this.oxygen_available = Mathf.RoundToInt(Player.main.GetOxygenAvailable());

            this.pda_state = Player.main.GetPDA().state;

            this.is_in_water_for_swimming = Player.main.IsUnderwaterForSwimming();
            //this.is_swimming = Player.main.IsSwimming(); //


            this.motor_mode = Player.main.motorMode;
            this.mode = Player.main.GetMode();
            
        }
    }

    public class VehicleSubNode
    {
        public string type { get; set; } //Base, Cyclops, Seamoth or Prawn
        
        //General Vehicle/Sub Variables:
        public int power { get; set; }
        public int max_power { get; set; }

        public bool floodlight { get; set; }

        public int vehicle_health { get; set; }
        public float vehicle_max_health { get; set; }
        public int crushDepth { get; set; }
        //public int v_depth { get; set; }

        //General Sub Variables:
        public LightingController.LightingState lightstate { get; set; }
        //public float lightfade { get; set; } //?

        //public float get_power_rating { get; set; }

        //Cyclops Variables:
        public bool cyclops_warning { get; set; }
        public bool cyclops_fire_suppression_state { get; set; }
        public bool cyclops_silent_running { get; set; }
        public CyclopsMotorMode.CyclopsMotorModes cyclops_motor_mode { get; set; }
        public bool cyclops_engine_on { get; set; }

        //public float cyclops_Noise { get; set; }
        public float cyclops_noice_percent { get; set; }

        //Base Variables:

        //Vehicle Variables:
        //public int vehicle_lightstate { get; set; }
        //public int vehicle_max_lightstate { get; set; }
        public int temperatur { get; set; }

        public VehicleSubNode()
        {
            var SubRoot = Player.main.GetCurrentSub();
            var Vehicle = Player.main.GetVehicle();

            if (SubRoot)
            {
                this.type = SubRoot.GetType().Equals(typeof(BaseRoot)) ? "Base" : "Cyclops";

                //General Variables:
                this.power = Mathf.RoundToInt(SubRoot.powerRelay.GetPower());
                this.max_power = Mathf.RoundToInt(SubRoot.powerRelay.GetMaxPower());
                //get_power_rating = SubRoot.GetPowerRating(); //power efficiency

                this.lightstate = SubRoot.lightControl.state; // On = 0, On with Danger = 1, Off = 2
                //this.lightfade = SubRoot.lightControl.fadeDuration; //?

                //Cyclops Variables:
                this.vehicle_health = type == "Cyclops" ? Mathf.RoundToInt(SubRoot.damageManager.subLiveMixin.health) : 0;
                this.vehicle_max_health = type == "Cyclops" ? Mathf.RoundToInt(SubRoot.damageManager.subLiveMixin.maxHealth) : 0; //Base do not have health

                this.cyclops_warning = type == "Cyclops" && SubRoot.subWarning; //Cyclops Alarm (fire alarm)
                this.cyclops_fire_suppression_state = type == "Cyclops" && SubRoot.fireSuppressionState; //fire Suppression with Cyclops module

                this.cyclops_silent_running = type == "Cyclops" && SubRoot.silentRunning; //Cyclops is silent Running

                var SubControl = SubRoot.GetComponentInParent<SubControl>();
                this.cyclops_motor_mode = type == "Cyclops" ? SubControl.cyclopsMotorMode.cyclopsMotorMode : CyclopsMotorMode.CyclopsMotorModes.Standard;
                this.cyclops_engine_on = type == "Cyclops" ? SubControl.cyclopsMotorMode.engineOn : false;

                //this.cyclops_Noise = type == "Cyclops" ? SubControl.cyclopsMotorMode.GetNoiseValue() : 0; //same as CyclopsNoise.noiseScalar //you can also easy "calculate" it out of "cyclopsMotorMode"

                var CyclopsNoise = SubRoot.GetComponentInParent<CyclopsNoiseManager>(); 

                this.floodlight = type == "Cyclops" ? CyclopsNoise.lightingPanel.floodlightsOn : false;
                this.cyclops_noice_percent = type == "Cyclops" ? CyclopsNoise.GetNoisePercent() : 0;

                //Base Variables:

            }
            else if (Vehicle)
            {
                this.type = Vehicle.GetType().Equals(typeof(SeaMoth)) ? "Seamoth" : "Prawn";

                this.vehicle_health = Mathf.RoundToInt(Vehicle.liveMixin.health);
                this.vehicle_max_health = Vehicle.liveMixin.maxHealth;

                Vehicle.GetDepth(out int Vehicle_depth, out int Vehicle_crushDepth);
                this.crushDepth = Vehicle_crushDepth;
                //this.crushDepth = Mathf.RoundToInt(Player.main.crushDepth);
                //this.v_depth = Vehicle_depth;

                var Vehicle_einterface = Vehicle.GetComponentsInParent<EnergyInterface>();
                Vehicle.GetComponentInParent<EnergyInterface>().GetValues(out float charge, out float capacity);
                this.power = Mathf.RoundToInt(charge);
                this.max_power = Mathf.RoundToInt(capacity);

                var Lights = Vehicle.GetComponentInChildren<ToggleLights>();
                //this.vehicle_lightstate = Lights.lightState;
                //this.vehicle_max_lightstate = Lights.maxLightStates;

                this.temperatur = Mathf.RoundToInt(Vehicle.GetTemperature());

                //only Seamoth:
                this.floodlight = type == "Seamoth" ? Lights.lightsActive : false;
            }
        }
    }

    public class NotificationNode
    {
        public int undefined_notification_count { get; set; }
        public int inventory_notification_count { get; set; }
        public int blueprints_notification_count { get; set; }
        public int builder_notification_count { get; set; }
        public int craft_tree_notification_count { get; set; }
        public int log_notification_count { get; set; }
        public int gallery_notification_count { get; set; }
        public int encyclopedia_notification_count { get; set; }

        public NotificationNode()
        {
            this.undefined_notification_count = NotificationManager.main.GetCount(NotificationManager.Group.Undefined);
            this.inventory_notification_count = NotificationManager.main.GetCount(NotificationManager.Group.Inventory);
            this.blueprints_notification_count = NotificationManager.main.GetCount(NotificationManager.Group.Blueprints);
            this.builder_notification_count = NotificationManager.main.GetCount(NotificationManager.Group.Builder);
            this.craft_tree_notification_count = NotificationManager.main.GetCount(NotificationManager.Group.CraftTree);
            this.log_notification_count = NotificationManager.main.GetCount(NotificationManager.Group.Log);
            this.gallery_notification_count = NotificationManager.main.GetCount(NotificationManager.Group.Gallery);
            this.encyclopedia_notification_count = NotificationManager.main.GetCount(NotificationManager.Group.Encyclopedia);

        }
    }

    public class WorldNode
    {

        public double day_scalar { get; set; }
        public double daylight_scaler { get; set; }

        //public double day_night_cycle_time { get; set; } 
        //public float light_scaler { get; set; }

        public WorldNode()
        {
            
            this.day_scalar = Math.Round(DayNightCycle.main.GetDayScalar(), 2);
            this.daylight_scaler = Math.Round(DayNightCycle.main.GetLocalLightScalar(), 2);

            //this.day_night_cycle_time = Math.Round(DayNightCycle.main.GetDayNightCycleTime(), 2);
            //this.light_scaler = DayNightCycle.main.GetLightScalar();
        }
    }


    // This will be serialized into a nested JSON object
    public class GSINode
    {
        public ProviderNode provider { get; set; }
        public GameStateNode game_state { get; set; }
        //"null" when not in Game
        public PlayerNode player { get; set; }
        public VehicleSubNode vehicle_sub { get; set; }
        public NotificationNode notification { get; set; }
        public WorldNode world { get; set; }

        public GSINode()
        {
            this.provider = new ProviderNode();
            this.game_state = new GameStateNode();

            //only get PlayerInfo when InGame
            if (AuroraController.state == PlayerState.Playing || AuroraController.state == PlayerState.Paused)
            {
                //will fail when not in Game 
                try
                {
                    this.player = new PlayerNode();
                    this.vehicle_sub = new VehicleSubNode();
                    this.notification = new NotificationNode();
                    this.world = new WorldNode();
                }
                catch { }
            }
        }
    }
}