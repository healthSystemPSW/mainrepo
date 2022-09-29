using Hospital.GraphicalEditor.Model;
using Hospital.Schedule.Model;
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
        public RoomPosition RoomPosition { get; set; }

        public IEnumerable<RoomInventory> RoomInventories { get; set; }

        public IEnumerable<ScheduledEvent> ScheduledEvents { get; set; }

      
        public bool AreNeighbors(Room room)
        {
            RoomPosition position = room.RoomPosition;

            if (!this.BuildingName.Equals(room.BuildingName))
                return false;
            else if (this.FloorNumber != room.FloorNumber)
                return false;
            else if (this.BuildingName.Equals("Building 1"))
            {
                if (this.RoomPosition.DimensionY + this.RoomPosition.Height == position.DimensionY && this.RoomPosition.DimensionX == position.DimensionX)
                {
                    return true;
                }
                else if (this.RoomPosition.DimensionY - this.RoomPosition.Height == position.DimensionY && this.RoomPosition.DimensionX == position.DimensionX)
                {
                    return true;
                }
            }
            else if (this.BuildingName.Equals("Building 2"))
            {
                if (this.RoomPosition.DimensionX + this.RoomPosition.Width == position.DimensionX && this.RoomPosition.DimensionY == position.DimensionY)
                {
                    return true;
                }
                else if (this.RoomPosition.DimensionX - this.RoomPosition.Width == position.DimensionX && this.RoomPosition.DimensionY == position.DimensionY)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
