namespace SecondMonitor.Rating.Common.DataModel.Championship.Events
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class SessionResultDto
    {
        public SessionResultDto()
        {
            DriverSessionResult = new List<DriverSessionResultDto>();
        }

        public List<DriverSessionResultDto> DriverSessionResult { get; set; }
    }
}