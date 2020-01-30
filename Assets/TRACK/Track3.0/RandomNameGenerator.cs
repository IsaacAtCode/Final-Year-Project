using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IsaacFagg.Track3
{
    public class RandomNameGenerator : MonoBehaviour
    {
        public List<string> locationNames;
        public List<string> roadNames;


        //public List<string> StringToWords(string nameString)
        //{
        //    string[] strings = nameString.Split(' ');
        //    return strings.ToList<string>();
        //}

        public string GenerateName()
        {
            string firstName = locationNames[Random.Range(0, locationNames.Count)];
            string lastName = roadNames[Random.Range(0, roadNames.Count)];

            return "The " + firstName + " " + lastName;
        }
    }
}
