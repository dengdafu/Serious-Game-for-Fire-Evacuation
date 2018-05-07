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
    public float HRRPUA;
    public float CO_YIELD;
    public float SOOT_YIELD;

    public string FUEL;
}