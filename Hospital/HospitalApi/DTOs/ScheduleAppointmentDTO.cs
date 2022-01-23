﻿using System;

namespace HospitalApi.DTOs
{
    public class ScheduleAppointmentDTO
    {
        public DateTime StartDate { get; set; }
        public int DoctorId { get; set; }
        public int DoctorsRoomId { get; set; }
        public int PatientId { get; set; }
        public string PatientUsername { get; set; }
    }
}
