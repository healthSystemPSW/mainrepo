using System;
using System.Threading;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.Services.Manager;

namespace Model
{
    public class TransferRequest
    {
        private int _senderRoomId;
        private int _recipientRoomId;
        private string _InventoryId;
        private int _quantity;
        private DateTime _timeOfExecution;

        public int SenderRoom
        {
            get { return _senderRoomId; }
            set { _senderRoomId = value; }
        }

        public int RecipientRoom
        {
            get { return _recipientRoomId; }
            set { _recipientRoomId = value; }
        }

        public string InventoryId
        {
            get { return _InventoryId; }
            set { _InventoryId = value; }
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public DateTime TimeOfExecution
        {
            get { return _timeOfExecution; }
            set { _timeOfExecution = value; }
        }

        public TransferRequest(int sender, int reciever, string invId, int quantity, DateTime whenToDo)
        {
            this.SenderRoom = sender;
            this.RecipientRoom = reciever;
            this.InventoryId = invId;
            this.Quantity = quantity;
            this.TimeOfExecution = whenToDo;
        }

        public void DoWork(InjectorDTO injector)
        {
            TimeSpan ts = TimeOfExecution.Subtract(DateTime.Now);

            if (ts > new TimeSpan(0,0,0))
                Thread.Sleep(ts);

            TransferRequestService transferRequestService = new TransferRequestService(injector);

            transferRequestService.ExecuteRequest(this);
        }
    }
}