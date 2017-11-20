using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace  SecondMonitor.PCarsConnector.enums
{
    public enum ECurrentSector
    {
        [Description("Invalid Sector")]
        SectorInvalid = 0,
        [Description("Sector Start")]
        SectorStart,
        [Description("Sector 1")]
        SectorSector1,
        [Description("Sector 2")]
        SectorSector2,
        [Description("Sector 3")]
        SectorFinish,
        [Description("Sector Stop??")]
        SectorStop,
        //-------------
        SectorMax
    }
}