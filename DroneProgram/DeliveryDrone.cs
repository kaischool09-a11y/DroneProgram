using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneProgram
{
    internal class DeliveryDrone
    {
        public string Id { get; }
        public double MaxWeightCapacityKg { get; }
        private double batteryPercentage;
        public int CurrentAltitudeMeters { get; set; }

        public double BatteryPercentage
        {
            get { return BatteryPercentage; ; }

            set
            {
                if (value >= 0 && value <= 100)
                    batteryPercentage = value;
            }
        }

        private string status;
        public string Status
        {
            get;
            set
            {
                if (value != "ReturningHome" || value != "InFlight" || value != "Grounded")
                {
                    throw new ArgumentException("Invalid status value");
                }
            }

        }
        public DeliveryDrone(string id, double maxWeightCapacity)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Id cannot be null or empty");
            }
            this.Id = id;
            if (maxWeightCapacity <= 0)
            {
                throw new ArgumentOutOfRangeException(" The weight cannot be 0 or smaller ");
            }
            this.MaxWeightCapacityKg = maxWeightCapacity;
            this.batteryPercentage = 100;
            this.CurrentAltitudeMeters = 0;
            this.status = "Grounded";
        }
        public DroneResult TakeOff()
        {

            if (this.BatteryPercentage < 30)
            {
                return new DroneResult
                {
                    Success = false,
                    Message = "Battery too low for takeoff",

                };
            }
            if (this.Status == "Grounded")
            {

                this.Status = "InFlight";
                this.CurrentAltitudeMeters = 50;

                return new DroneResult
                {
                    Success = true,
                    Message = "Takeoff successful",
                };


            }
            throw new InvalidOperationException("Drone is already in flight or returning home");



        }
        public DroneResult AssignDelivery (double packageWeight, int distanceKm)
        {
            if(MaxWeightCapacityKg<packageWeight)
            {
                return new DroneResult
                {
                    Success = false,
                    Message = "Package too heavy for the drone",
                 
                };
            }
            if(this.BatteryPercentage<distanceKm*5)
            {
                return new DroneResult
                {
                    Success = false,
                    Message = "Battery too low for delivery",
                 
                };
            }
            if(this.Status!="InFlight")
            {
                return new DroneResult
                {
                    Success = false,
                    Message = "Drone must be in flight to assign delivery",
                 
                };
            }

            this.BatteryPercentage -= distanceKm * 5;
            this.Status = "ReturningHome";

        }
        public void Land()
        {
            if(this.Status!="ReturningHome")
            {
                throw new InvalidOperationException("Drone must be returning home to land");
            }
            CurrentAltitudeMeters = 0;
            this.Status = "Grounded";
        }
    }
}

