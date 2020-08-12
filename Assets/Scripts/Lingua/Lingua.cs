using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using NReco.Csv;
using UnityEngine;

namespace Lingua
{
    public static class Lingua
    {
        public enum RegionCode
        {
            KR = 0,
            EN,
            END
        }

        private static RegionCode region;
        public static RegionCode Region
        {
            get => region;
            set
            {
                region = value;
            }
        }

        static Dictionary<string, string[]> regionTexts;

        static Lingua()
        {
            InitRegionTexts();
        }

        static void InitRegionTexts()
        {
            regionTexts = new Dictionary<string, string[]>();
            var stringTable = Resources.Load("strings") as TextAsset;
            var inputStream = new MemoryStream(stringTable.bytes);
            using (var streamRdr = new StreamReader(inputStream))
            {
                var csvReader = new CsvReader(streamRdr, ",");
                while (csvReader.Read())
                {
                    if (regionTexts.ContainsKey(csvReader[0]))
                    {
                        Debug.Log("Warning : CSV file has same key");
                        continue;
                    }
                    if (!string.IsNullOrWhiteSpace(csvReader[0]) && !csvReader[0].Contains("#"))
                    {
                        string[] strings = new string[(int)RegionCode.END];
                        for (int i = 1; i < csvReader.FieldsCount; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(csvReader[i]))
                            {
                                strings[i - 1] = csvReader[i];
                            }
                        }
                        regionTexts.Add(csvReader[0], strings);
                    }
                }
            }
            
        }

        public static string GetString(string key)
        {
            return regionTexts[key][(int)Region];
        }
    }

}
