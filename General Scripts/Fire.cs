using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

    // An array containing all the fuels
    public string[] Fuels = {"ACETONE", "ACETYLENE", "ACROLEIN", "AMMONIA", "ARGON",
                             "BENZENE", "BUTANE",
                             "CARBON", "CARBON DIOXIDE", "CARBON MONOXIDE", "CHLORINE",
                             "DODECANE",
                             "ETHANE", "ETHANOL", "ETHYLENE",
                             "FORMALDEHYDE",
                             "HELIUM", "HYDROGEN", "HYDROGEN ATOM", "HYDROGEN BROMIDE",
                             "HYDROGEN CHLORIDE", "HYDROGEN CYANIDE", "HYDROGEN FLUORIDE",
                             "HYDROGEN PEROXIDE", "HYDROPEROXY RADICAL", "HYDROXYL RADICAL",
                             "ISOPROPANOL",
                             "LJ AIR",
                             "METHANE", "METHANOL",
                             "N-DECANE", "N-HEPTANE", "N-HEXANE", "N-OCTANE", "NITRIC OXIDE",
                             "NITROGEN", "NITROGEN ATOM", "NITROGEN DIOXIDE", "NITROUS OXIDE",
                             "OXYGEN", "OXYGEN ATOM",
                             "PROPANE", "PROPYLENE",
                             "SOOT", "SULFUR DDIOXIDE", "SULFUR HEXAFLUORIDE",
                             "TOLUENE",
                             "WATER VAPOR"};

    // Some variables that charactorize the fire.
    public string Name;
    public float x_pos;
    public float y_pos;
    public float z_pos;
    public float Width;
    public float Length;
    public float HRRPUA;
    public float CO_YIELD;
    public float SOOT_YIELD;
    public string FUEL;

    public void FillInfo(string name, float xpos, float ypos, float zpos, float width, float length,
        float hrrpua, float co_yield, float soot_yield, string fuel)
    {
        Name = name; x_pos = xpos; y_pos = ypos; z_pos = zpos; Width = width; Length = length;
        HRRPUA = hrrpua; CO_YIELD = co_yield; SOOT_YIELD = soot_yield; FUEL = fuel;
    }
}