﻿using Hospital.Schedule.Model;
using Hospital.SharedModel.Model;
using Hospital.SharedModel.Model.Enumerations;
using System.Collections.Generic;

namespace Hospital.RoomsAndEquipment.Model
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public int FloorNumber { get; set; }
        public string BuildingName { get; set; }
        public RoomType RoomType { get; set; }
        public IEnumerable<Doctor> Doctors { get; set; }

        public IEnumerable<RoomInventory> RoomInventories { get; set; }

        public IEnumerable<ScheduledEvent> ScheduledEvents { get; set; }
    }
}
