namespace AcTyresExtractor.Extractor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using IniParser;
    using IniParser.Exceptions;
    using IniParser.Model;
    using SecondMonitor.DataModel.BasicProperties;
    using SecondMonitor.DataModel.OperationalRange;

    public class TyreOperationalRangeExtractor
    {
        private const string TyresIniFile = "tyres.ini";
        private string _currentFolder;

        public List<TyreCompoundProperties> ExtractFromFolder(string folderPath)
        {
            string tyresIniPath = Path.Combine(folderPath, TyresIniFile);
            _currentFolder = folderPath;
            if (!File.Exists(tyresIniPath))
            {
                return new List<TyreCompoundProperties>();
            }

            try
            {
                IniData data = RearTyresIni(tyresIniPath);
                List<TyreCompoundProperties> compounds = GetTyresPropertiesFromIni(data);
                return compounds;
            }
            catch (ParsingException ex)
            {
                Console.Write($"Unable to Parse {tyresIniPath}.\nMessage{ex.Message}\nLine{ex.LineNumber}\n{ex.LineValue}\n\n\n");
            }
            return new List<TyreCompoundProperties>();
        }

        private List<TyreCompoundProperties> GetTyresPropertiesFromIni(IniData data)
        {
            List<TyreCompoundProperties> compounds = new List<TyreCompoundProperties>();

            int i = 0;
            string sectionNameFront = i > 0 ? $"FRONT_{i}" : "FRONT";
            string sectionNameRear = i > 0 ? $"REAR_{i}" : "REAR";

            while (data.Sections.ContainsSection(sectionNameFront))
            {
                var sectionData = data.Sections.GetSectionData(sectionNameFront);
                var sectionRearData = data.Sections.GetSectionData(sectionNameRear);
                var tyreThermalDataFront = data.Sections.GetSectionData("THERMAL_" + sectionNameFront);
                var tyreThermalDataRear = data.Sections.GetSectionData("THERMAL_" + sectionNameFront);
                compounds.Add(CreateTyreCompoundProperties(sectionData, tyreThermalDataFront, sectionRearData, tyreThermalDataRear));
                i++;
                sectionNameFront = i > 0 ? $"FRONT_{i}" : "FRONT";
                sectionNameRear = i > 0 ? $"REAR_{i}" : "REAR";
            }

            return compounds;
        }

        private List<KeyValuePair<double, double>> GetTemperatureCurve(string curveFileName)
        {
            string curveFileNameParsed = curveFileName.Split('.')[0] + ".lut";
            string fileFullPath = Path.Combine(_currentFolder, curveFileNameParsed);
            if (!File.Exists(fileFullPath))
            {
                Console.WriteLine($"Unable to Find lut file {curveFileNameParsed}, for directory {_currentFolder}");
                return new List<KeyValuePair<double, double>>();
            }
            string[] curveRaw = File.ReadAllLines(fileFullPath);
            return curveRaw.Select(x => x.Split('|')).Where(x => x.Length == 2).Select(x => new KeyValuePair<double, double>(double.Parse(x[0]), double.Parse(x[1]))).OrderBy(x => x.Key).ToList();
        }

        private TyreCompoundProperties CreateTyreCompoundProperties(SectionData tyreSectionDataFront, SectionData tyreThermalDataFront, SectionData tyreSectionDataRear, SectionData tyreThermalDataRear)
        {
            TyreCompoundProperties newCompoundProperties = new TyreCompoundProperties();
            string shortName = tyreSectionDataFront.Keys.ContainsKey("SHORT_NAME") ? tyreSectionDataFront.Keys.GetKeyData("SHORT_NAME").Value : string.Empty;
            newCompoundProperties.CompoundName = Trim($"{tyreSectionDataFront.Keys.GetKeyData("NAME").Value}");
            if (!string.IsNullOrEmpty(shortName))
            {
                newCompoundProperties.CompoundName += $" ({shortName})";
            }
            newCompoundProperties.FrontIdealPressureWindow = Pressure.FromKiloPascals(10);
            newCompoundProperties.FrontIdealPressure = ExtractPressureFromKeyData(tyreSectionDataFront.Keys.GetKeyData("PRESSURE_IDEAL"));

            newCompoundProperties.RearIdealPressureWindow = Pressure.FromKiloPascals(10);
            newCompoundProperties.RearIdealPressure = ExtractPressureFromKeyData(tyreSectionDataRear.Keys.GetKeyData("PRESSURE_IDEAL"));

            var temperatureCurveFront = GetTemperatureCurve(tyreThermalDataFront.Keys.GetKeyData("PERFORMANCE_CURVE").Value);
            var temperatureCurveRear = GetTemperatureCurve(tyreThermalDataRear.Keys.GetKeyData("PERFORMANCE_CURVE").Value);

            (Temperature idealTemperature, Temperature window) temperatureRange = GetTemperatureProperties(temperatureCurveFront);
            if (temperatureRange.idealTemperature != null &&temperatureRange.window != null)
            {
                newCompoundProperties.FrontIdealTemperature = temperatureRange.idealTemperature;
                newCompoundProperties.FrontIdealTemperatureWindow = temperatureRange.window;
            }

            temperatureRange = GetTemperatureProperties(temperatureCurveRear );
            if (temperatureRange.idealTemperature != null && temperatureRange.window != null)
            {
                newCompoundProperties.RearIdealTemperature = temperatureRange.idealTemperature;
                newCompoundProperties.RearIdealTemperatureWindow = temperatureRange.window;
            }

            return newCompoundProperties;
        }

        private (Temperature idealTemperature, Temperature window) GetTemperatureProperties(List<KeyValuePair<double, double>> temperatureCurve)
        {
            if (temperatureCurve.Count < 2)
            {
                return (null, null);
            }

            double minimalTemperature = temperatureCurve.FirstOrDefault(x => x.Value == 1).Key;

            if (minimalTemperature == 0)
            {
                return (null, null);
            }

            double maximumTemperature = temperatureCurve.Last(x => x.Value == 1).Key;
            double window = (maximumTemperature - minimalTemperature) / 2;
            return (Temperature.FromCelsius(minimalTemperature + window), Temperature.FromCelsius(window));
        }

        private Pressure ExtractPressureFromKeyData(KeyData pressureData)
        {
            string keyValue = pressureData.Value;
            return Pressure.FromPsi(Regex.Split(keyValue, @"[^0-9\.]+")
                .Where(c => c != "." && c.Trim() != "").Select(double.Parse).First());
        }

        private static string Trim(string s)
        {
            return s.Split('\t')[0].Split(';')[0].Trim();
        }

        private static IniData RearTyresIni(string tyresIniPath)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(tyresIniPath);
            return data;
        }
    }
}