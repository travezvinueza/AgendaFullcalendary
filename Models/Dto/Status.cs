﻿namespace Agenda.Models.Dto
{

    public class Status
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public Status()
        {
            Message = string.Empty;
        }

    }
}