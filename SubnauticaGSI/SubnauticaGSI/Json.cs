using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    public class PlayerNode
    {
        public PlayerState player_state { get; set; }

        public string biom { get; set; }

        public int depth { get; set; } //only depth
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

        public Player.MotorMode motor_mode { get; set; }
        public Player.Mode mode { get; set; }

        public PlayerNode()
        {
            // set properties in default constructor to generate sample data
            this.player_state = AuroraController.state;

            this.biom = Player.main.GetBiomeString();

            this.depth = Mathf.RoundToInt(Player.main.GetDepth());
            //this.surface_depth = Mathf.RoundToInt(Player.main.GetSurfaceDepth());
            this.depth_level = Mathf.RoundToInt(Player.main.depthLevel);

            this.health = Mathf.RoundToInt(Player.main.liveMixin.health);
            this.food = Mathf.RoundToInt(Player.main.gameObject.GetComponent<Survival>().food);
            this.water = Mathf.RoundToInt(Player.main.gameObject.GetComponent<Survival>().water);

            this.can_breathe = Player.main.CanBreathe();
            this.oxygen_capacity = Mathf.RoundToInt(Player.main.GetOxygenCapacity());
            this.oxygen_available = Mathf.RoundToInt(Player.main.GetOxygenAvailable());

            this.pda_state = Player.main.GetPDA().state;

            this.motor_mode = Player.main.motorMode;
            this.mode = Player.main.GetMode();
            
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
        //public double day_night_cycle_time { get; set; } 
        public double day_scalar { get; set; }

        public WorldNode()
        {
            //this.day_night_cycle_time = Math.Round(DayNightCycle.main.GetDayNightCycleTime(), 2);
            this.day_scalar = Math.Round(DayNightCycle.main.GetDayScalar(), 2);
        }
    }


    // This will be serialized into a nested JSON object
    public class GSINode
    {
        public ProviderNode provider { get; set; }
        public PlayerNode player { get; set; }
        public NotificationNode notification { get; set; }
        public WorldNode world { get; set; }

        public GSINode()
        {
            this.provider = new ProviderNode();
            this.player = new PlayerNode();
            this.notification = new NotificationNode();
            this.world = new WorldNode();
        }
    }
}