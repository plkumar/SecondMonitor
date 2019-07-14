namespace R3ECarTelemetryPropertiesExtractor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using SecondMonitor.R3EConnector;
    using SecondMonitor.Telemetry.TelemetryApplication.Settings.DTO.DefaultCarProperties;

    public class SuspensionDataFiller
    {
        private readonly R3RDatabase _reDatabase;

        private readonly List<(string find, string replace)> _replaceMap = new List<(string find, string replace)>()
        {
            ("hyundaii30tcr", "hyundaii30ntcr")
        };

        private enum LineType
        {
            None, AiEntry, CarName, Data, NonSupportedData
        }

        private enum State
        {
            WaitingForName, WaitingForData, Front1, Front2, Rear1,
        }

        public SuspensionDataFiller()
        {
            _reDatabase = new R3RDatabase();
        }

        protected DefaultR3ECarsProperties CarsProperties { get; set; }

        public DefaultR3ECarsProperties FillData(string[] bumpData, string[] reboundData)
        {
            _reDatabase.Load(@"c:\Users\Darkman\Documents\Visual Studio 2015\Projects\SecondMonitor\Artifacts\data.json");
            InitReplaceMap();
            CarsProperties = new DefaultR3ECarsProperties()
            {
                CarsProperties = new List<DefaultR3ECarProperties>()
            };

            FillBumpData(bumpData);
            FillReboundData(reboundData);
            string[] allR3ECars = _reDatabase.GetAllCars().Select(x => x.Replace(" ", "").ToLower()).ToArray();
            string[] knownCars = CarsProperties.CarsProperties.Select(x => x.CarName).Where(x => allR3ECars.Contains(x)).ToArray();
            string[] unknownCars = CarsProperties.CarsProperties.Select(x => x.CarName).Where(x => !allR3ECars.Contains(x)).ToArray();
            string fileName = @"c:\Users\Darkman\Documents\Visual Studio 2015\Projects\SecondMonitor\Artifacts\Data\R3E\unknownCars.txt";
            File.WriteAllLines(fileName, unknownCars);
            File.WriteAllLines(@"c:\Users\Darkman\Documents\Visual Studio 2015\Projects\SecondMonitor\Artifacts\Data\R3E\AllR3ECars.txt", allR3ECars.OrderBy(x => x));
            File.WriteAllLines(@"c:\Users\Darkman\Documents\Visual Studio 2015\Projects\SecondMonitor\Artifacts\Data\R3E\MappedCars.txt", knownCars.OrderBy(x => x));
            return CarsProperties;
        }

        private void InitReplaceMap()
        {
            string[] carMap = File.ReadAllLines(@"c:\Users\Darkman\Documents\Visual Studio 2015\Projects\SecondMonitor\Artifacts\Data\R3E\carMap.txt");
            foreach (string carMapEntry in carMap)
            {
                var split = carMapEntry.Split(' ');
                _replaceMap.Add((split[0], split[1]));
            }
        }

        private void FillReboundData(string[] bumpData)
        {
            State currentState = State.WaitingForName;
            DefaultR3ECarProperties currentCarProperties = new DefaultR3ECarProperties();
            for(int i = 0; i < bumpData.Length; i++)
            {
                string line = bumpData[i].ToLower();
                LineType currentLineType = GetLineType(line);
                if (currentLineType == LineType.NonSupportedData)
                {
                    continue;
                }

                if (currentState == State.WaitingForName && currentLineType == LineType.AiEntry)
                {
                    continue;
                }

                if (currentState == State.WaitingForName && currentLineType == LineType.CarName)
                {
                    string carName = ExtractCarName(line);
                    currentCarProperties = GetCarProperties(carName);
                    currentState = State.WaitingForData;
                    continue;
                }

                if (currentState == State.WaitingForData && currentLineType == LineType.Data)
                {
                    double frontRebound = ExtractData(line);
                    currentCarProperties.ReboundTransitionFront = frontRebound;
                    currentState = State.Front1;
                    continue;
                }

                if (currentState == State.Front1 && currentLineType == LineType.Data)
                {
                    currentState = State.Front2;
                    continue;
                }

                if (currentState == State.Front2 && currentLineType == LineType.Data)
                {
                    double rearRebound = ExtractData(line);
                    currentCarProperties.ReboundTransitionRear = rearRebound;
                    currentState = State.Rear1;
                    continue;
                }

                if (currentState == State.Rear1 && currentLineType == LineType.Data)
                {
                    currentState = State.WaitingForName;
                    continue;
                }

                currentState = State.WaitingForName;
            }
        }
        private void FillBumpData(string[] bumpData)
        {
            State currentState = State.WaitingForName;
            DefaultR3ECarProperties currentCarProperties = new DefaultR3ECarProperties();
            for (int i = 0; i < bumpData.Length; i++)
            {
                string line = bumpData[i].ToLower();
                LineType currentLineType = GetLineType(line);
                if (currentLineType == LineType.NonSupportedData)
                {
                    continue;
                }

                if (currentState == State.WaitingForName && currentLineType == LineType.AiEntry)
                {
                    continue;
                }

                if (currentState == State.WaitingForName && currentLineType == LineType.CarName)
                {
                    string carName = ExtractCarName(line);
                    currentCarProperties = GetCarProperties(carName);
                    currentState = State.WaitingForData;
                    continue;
                }

                if (currentState == State.WaitingForData && currentLineType == LineType.Data)
                {
                    double frontBump = ExtractData(line);
                    currentCarProperties.BumpTransitionFront = frontBump;
                    currentState = State.Front1;
                    continue;
                }

                if (currentState == State.Front1 && currentLineType == LineType.Data)
                {
                    currentState = State.Front2;
                    continue;
                }

                if (currentState == State.Front2 && currentLineType == LineType.Data)
                {
                    double rearBump = ExtractData(line);
                    currentCarProperties.BumpTransitionRear = rearBump;
                    currentState = State.Rear1;
                    continue;
                }

                if (currentState == State.Rear1 && currentLineType == LineType.Data)
                {
                    currentState = State.WaitingForName;
                    continue;
                }

                currentState = State.WaitingForName;
            }
        }


        private DefaultR3ECarProperties GetCarProperties(string carName)
        {
            var carProperties = CarsProperties.CarsProperties.FirstOrDefault(x => x.CarName == carName);
            if (carProperties != null)
            {
                return carProperties;
            }

            carProperties = new DefaultR3ECarProperties()
            {
                CarName = carName,
                BumpTransitionFront = 0.03,
                BumpTransitionRear = 0.03,
                ReboundTransitionFront = 0.03,
                ReboundTransitionRear = 0.03,
            };

            CarsProperties.CarsProperties.Add(carProperties);
            return carProperties;
        }

        private string ExtractCarName(string line)
        {
            var lineSpan = line.AsSpan();
            int slashIndex =  lineSpan.LastIndexOf('\\') +1;
            int dotIndex = lineSpan.IndexOf('.');
            string carName = line.Substring(slashIndex, dotIndex - slashIndex);
            carName = carName.Replace("_","").ToLower();
            var replacement = _replaceMap.FirstOrDefault(x => carName == x.find);
            if (string.IsNullOrWhiteSpace(replacement.find))
            {
                return carName;
            }

            return replacement.replace;
        }

        private double ExtractData(string line)
        {
            var lineSpan = line.AsSpan();
            int equalIndex = lineSpan.LastIndexOf('=') + 1;
            var numberSpan = lineSpan.Slice(equalIndex);
            string rawValue = Regex.Match(numberSpan.ToString(), "[-+]?[0-9]*\\.?[0-9]*").Value;
            return double.Parse(rawValue, CultureInfo.InvariantCulture);
        }

        private static LineType GetLineType(string line)
        {
            if (line.Contains("_ai.") || line.Contains("_ama."))
            {
                return LineType.AiEntry;
            }

            if (line.Contains("Front3rdBumpStage2") || line.Contains("Rear3rdBumpStage2"))
            {
                return LineType.NonSupportedData;
            }

            if (line.Contains("="))
            {
                return LineType.Data;
            }

            if (line.Contains(".hdc"))
            {
                return LineType.CarName;
            }
            return LineType.None;
        }

    }
}