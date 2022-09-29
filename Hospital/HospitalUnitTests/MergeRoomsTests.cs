using Hospital.GraphicalEditor.Model;
using Hospital.RoomsAndEquipment.Model;
using Hospital.RoomsAndEquipment.Repository;
using Hospital.RoomsAndEquipment.Service;
using Hospital.SharedModel.Model.Enumerations;
using HospitalUnitTests.Base;
using Shouldly;
using System.Linq;
using Xunit;

namespace HospitalUnitTests
{
    public class MergeRoomsTests : BaseTest
    {
        private readonly RenovatingRoomsService _renovatingRoomsService;
        private readonly IRoomRenovationEventReadRepository _roomRenovationEventRead;
        private readonly IRoomReadRepository _roomReadRepository;
        public MergeRoomsTests(BaseFixture baseFixture) : base(baseFixture)
        {
            _renovatingRoomsService = new(UoW);
            _roomRenovationEventRead = UoW.GetRepository<IRoomRenovationEventReadRepository>();
            _roomReadRepository = UoW.GetRepository<IRoomReadRepository>();
        }

        [Fact]
        public void Room_position_should_change_comparer()
        {
            //Arrange
            PrepareData();
            var roomRenovationEvent = _roomRenovationEventRead.GetById(1);
            var newRoom = UoW.GetRepository<IRoomReadRepository>().GetById(1);
            //Act
            _renovatingRoomsService.MergeRooms(roomRenovationEvent);
           
           /*newRoom.RoomPosition.Height.ShouldBe(244);
            newRoom.RoomPosition.Width.ShouldBe(244);
            newRoom.RoomPosition.DimensionX.ShouldBe(0);
            newRoom.RoomPosition.DimensionY.ShouldBe(0);*/
           //Assert
            Assert.Equal(newRoom.RoomPosition, new RoomPosition(0, 0, 150, 244), new RoomPositionComparer());

        }

        [Fact]
        public void Room_position_should_change_x()
        {
            //Arrange
            PrepareData();
            var roomRenovationEvent = _roomRenovationEventRead.GetById(1);
            var newRoom = UoW.GetRepository<IRoomReadRepository>().GetById(1);

            //Act
            _renovatingRoomsService.MergeRooms(roomRenovationEvent);
           //Assert
            Assert.True(newRoom.RoomPosition.Equals(new RoomPosition(0, 0, 150, 244)));

        }

        [Fact]
        public void Two_rooms_should_be_merged()
        {
            PrepareData();
            
            var roomRenovationEvent = _roomRenovationEventRead.GetById(1);
            _renovatingRoomsService.MergeRooms(roomRenovationEvent);

            var rooms = _roomReadRepository.GetAll();
            rooms.Count().ShouldBe(1);
        }

        [Fact]
        public void Inventory_should_be_moved()
        {
            PrepareData();
   
            var roomRenovationEvent = _roomRenovationEventRead.GetById(1);
            _renovatingRoomsService.MergeRooms(roomRenovationEvent);
            var roomInventory = UoW.GetRepository<IRoomInventoryReadRepository>().GetById(2);
            roomInventory.RoomId.ShouldBe(1);
            var roomInventoryNew = UoW.GetRepository<IRoomInventoryReadRepository>().GetById(1);
            roomInventoryNew.Amount.ShouldBe(4);
        }

        [Fact]
        public void Room_size_should_change()
        {
            PrepareData();
   
            var roomRenovationEvent = UoW.GetRepository<IRoomRenovationEventReadRepository>().GetById(1);
            _renovatingRoomsService.MergeRooms(roomRenovationEvent);
            var newRoom = UoW.GetRepository<IRoomReadRepository>().GetById(1);
            newRoom.Width.ShouldBe(5);
            newRoom.Height.ShouldBe(12);
        }

        private void PrepareData() {

            ClearDbContext();

            Context.Rooms.Add(new Room()
            {
                Id = 1,
                Name = "Test room 1",
                Description = "Room description",
                BuildingName = "Building 1",
                FloorNumber = 1,
                Width = 5,
                Height = 6,
                RoomType = RoomType.AppointmentRoom,
                RoomPosition = new RoomPosition(0, 0, 150, 122)
            });

            Context.Rooms.Add(new Room()
            {
                Id = 2,
                Name = "Test room 2",
                Description = "Room description",
                BuildingName = "Building 1",
                FloorNumber = 1,
                Width = 5,
                Height = 6,
                RoomType = RoomType.AppointmentRoom,
                RoomPosition = new RoomPosition(0, 122, 150, 122)
            });

            Context.RoomRenovationEvents.Add(new RoomRenovationEvent()
            {
                Id = 1,
                RoomId = 1,
                IsMerge = true,
                MergeRoomId = 2
            });

            Context.InventoryItems.Add(new InventoryItem()
            {
                Id = 1,
                Name = "Test item 1"
            });

            Context.InventoryItems.Add(new InventoryItem()
            {
                Id = 2,
                Name = "Test item 2"
            });

            Context.RoomInventories.Add(new RoomInventory(1, 1, 1, 2));
            Context.RoomInventories.Add(new RoomInventory(2, 2, 2, 2));
            Context.RoomInventories.Add(new RoomInventory(3, 2, 1, 2));
            Context.SaveChanges();
        }
    }
}
