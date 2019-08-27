using System.Linq;
using SecondMonitor.DataModel.BasicProperties;
using SecondMonitor.DataModel.DriversPresentation;

namespace SecondMonitor.SimdataManagement.DriverPresentation
{
    using System;
    using System.Collections.Generic;

    public class DriverPresentationsManager
    {
        public event EventHandler<DriverCustomColorEnabledArgs> DriverCustomColorChanged;

        private readonly DriverPresentationsLoader _driverPresentationsLoader;
        private readonly Lazy<Dictionary<string, DriverPresentationDto>> _driversPresentationMapLazyLoad;

        public DriverPresentationsManager(DriverPresentationsLoader driverPresentationsLoader)
        {
            _driverPresentationsLoader = driverPresentationsLoader;
            _driversPresentationMapLazyLoad = new Lazy<Dictionary<string, DriverPresentationDto>>(LoadDriversPresentationMap);
        }

        private Dictionary<string, DriverPresentationDto> DriversPresentationsMap => _driversPresentationMapLazyLoad.Value;

        public bool TryGetOutLineColor(string driverName, out ColorDto color)
        {
            bool containsDriver = TryGetDriverPresentation(driverName, out DriverPresentationDto driverPresentationDto);
            color = containsDriver ? driverPresentationDto.OutLineColor : null;
            return containsDriver;
        }

        public bool IsCustomOutlineEnabled(string driverName)
        {
            TryGetDriverPresentation(driverName, out DriverPresentationDto driverPresentationDto);
            return driverPresentationDto?.CustomOutLineEnabled ?? false;
        }


        public void SetOutLineColorEnabled(string driverName, bool isEnabled)
        {
            DriverPresentationDto driverPresentation = GetDriverOrCreatePresentation(driverName);
            driverPresentation.CustomOutLineEnabled = isEnabled;
            DriverCustomColorChanged?.Invoke(this, new DriverCustomColorEnabledArgs(driverName, isEnabled, driverPresentation.OutLineColor));
        }

        public void SetOutLineColor(string driverName, ColorDto color)
        {
            DriverPresentationDto driverPresentation = GetDriverOrCreatePresentation(driverName);
            driverPresentation.OutLineColor = color;
            DriverCustomColorChanged?.Invoke(this, new DriverCustomColorEnabledArgs(driverName, driverPresentation.CustomOutLineEnabled, color));
        }


        public bool TryGetDriverPresentation(string driverName, out DriverPresentationDto driverPresentationDto)
        {
            return DriversPresentationsMap.TryGetValue(driverName, out driverPresentationDto);
        }

        public DriverPresentationDto GetDriverOrCreatePresentation(string driverName)
        {
            if (TryGetDriverPresentation(driverName, out DriverPresentationDto driverPresentationDto))
            {
                return driverPresentationDto;
            }

            driverPresentationDto = new DriverPresentationDto()
            {
                DriverName = driverName,
                CustomOutLineEnabled = false,
            };

            DriversPresentationsMap[driverName] = driverPresentationDto;
            return driverPresentationDto;
        }

        public void SavePresentations()
        {
            if (_driversPresentationMapLazyLoad.IsValueCreated)
            {
                _driverPresentationsLoader.Save(new DriverPresentationsDto(){DriverPresentations = DriversPresentationsMap.Values.ToList()});
            }
        }

        private DriverPresentationsDto LoadDriverPresentations()
        {
            return _driverPresentationsLoader.TryLoad(out DriverPresentationsDto driverPresentationsDto) ? driverPresentationsDto : new DriverPresentationsDto();
        }


        private Dictionary<string, DriverPresentationDto> LoadDriversPresentationMap()
        {
            return LoadDriverPresentations().DriverPresentations.Where(x => x.OutLineColor != null).ToDictionary(x => x.DriverName, x => x);
        }
    }
}