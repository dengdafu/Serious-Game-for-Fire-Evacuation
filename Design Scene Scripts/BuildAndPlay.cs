using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class BuildAndPlay : MonoBehaviour {

    public InputField ScenarioName;

    public void BuildAndPlayOnClick()
    {
        // 1. Set WorkingDirectory
        string WorkingDirectory = Application.persistentDataPath + "/" + ScenarioName.text;

        // 2. Get a string[] contains all the subdirectories in the scenario (all the scenes)
        DirectoryInfo dir = new DirectoryInfo(WorkingDirectory);
        DirectoryInfo[] subdirs = dir.GetDirectories();

        // 3. Go into each subdirectory to do the simulation
        foreach (DirectoryInfo subdir in subdirs)
        {
            // 4. Create fds input file
            WorkingDirectory = Application.persistentDataPath + "/" + ScenarioName.text + "/" + subdir.Name;
            // 4.1 Read in all object names
            List<string> AllObjectsNames = new List<string>();
            string name;
            if (File.Exists(WorkingDirectory + "/AllNames.txt"))
            {
                StreamReader theReader = new StreamReader(WorkingDirectory + "/AllNames.txt");
                using (theReader)
                {
                    do
                    {
                        name = theReader.ReadLine();
                        if (name != null)
                        {
                            AllObjectsNames.Add(name);
                        }
                    }
                    while (name != null);
                }
                theReader.Close();
            }
            // 4.2 Read in SceneInfo
            SceneDetails sceneDetails = new SceneDetails();
            if (File.Exists(WorkingDirectory + "/SceneDetails.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(WorkingDirectory + "/SceneDetails.dat", FileMode.Open);
                sceneDetails = (SceneDetails)bf.Deserialize(file);
                file.Close();
            }
            // 4.3 Write the fds input file
            if (sceneDetails.SimTime != 0)
            {
                StreamWriter writer = new StreamWriter(WorkingDirectory + "/" + subdir.Name + ".fds");
                // 4.3.1 Write the header
                string HEAD_LINE = "&HEAD CHID='" + subdir.Name + "', TITLE='" + subdir.Name + "'/";
                writer.WriteLine(HEAD_LINE);
                // 4.3.2 Setup the dimensions. For the moment, for simplicity, 
                // let's just assume there is only one 'Floor' element, and all
                // walls have same height
                float FDS_x0; float FDS_x1; float FDS_y0; float FDS_y1; float FDS_z0; float FDS_z1;
                floor mainfloor = sceneDetails.Floors[0];
                wall typicalwall = sceneDetails.Walls[0];
                FDS_x0 = mainfloor.xpos - (mainfloor.Width / 2);
                FDS_x1 = mainfloor.xpos + (mainfloor.Width / 2);
                FDS_y0 = mainfloor.ypos - (mainfloor.Length / 2);
                FDS_y1 = mainfloor.ypos + (mainfloor.Length / 2);
                FDS_z0 = 0;
                FDS_z1 = typicalwall.Height;
                string XB = "XB=" + FDS_x0 + "," + FDS_x1 + "," + FDS_y0 + "," + FDS_y1 +
                            "," + FDS_z0 + "," + FDS_z1;
                float MESH_I = ((FDS_x1 - FDS_x0) / sceneDetails.GridSize);
                float MESH_J = ((FDS_y1 - FDS_y0) / sceneDetails.GridSize);
                float MESH_K = ((FDS_z1 - FDS_z0) / sceneDetails.GridSize);
                string MESH = "&MESH " + "IJK=" + MESH_I + "," + MESH_J + "," + MESH_K + ", " +
                              XB + "/";
                writer.WriteLine(MESH);
                // 4.3.3 Write simulation time in .fds
                string FDS_Time_Line = "&TIME T_END=" + sceneDetails.SimTime + "/";
                writer.WriteLine(FDS_Time_Line);

                // 4.3.4 Write all the obstacles in .fds
                float obs_x0; float obs_x1; float obs_y0; float obs_y1; float obs_z0; float obs_z1; string obs_line;
                foreach (obstacle OBSTACLE in sceneDetails.Obstacles)
                {
                    obs_x0 = OBSTACLE.xpos - (OBSTACLE.Width / 2);
                    obs_x1 = OBSTACLE.xpos + (OBSTACLE.Width / 2);
                    obs_y0 = OBSTACLE.ypos - (OBSTACLE.Length / 2);
                    obs_y1 = OBSTACLE.ypos + (OBSTACLE.Length / 2);
                    obs_z0 = 0;
                    obs_z1 = OBSTACLE.Height;
                    obs_line = "&OBST XB=" + obs_x0 + "," + obs_x1 + "," +
                               obs_y0 + "," + obs_y1 + "," +
                               obs_z0 + "," + obs_z1 + "/";
                    writer.WriteLine(obs_line);
                }

                // 4.3.5 Write fires in .fds
                float fire_x0; float fire_x1; float fire_y0; float fire_y1; float fire_z; string fire_line;
                string[] Fuels = {"ACETONE", "ACETYLENE", "ACROLEIN", "AMMONIA", "ARGON",
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
                foreach (fire FIRE in sceneDetails.Fires)
                {
                    // Write the reaction line
                    string REAC_line = "&REAC ID=" + "'" + AllObjectsNames[FIRE.NameIndex] + "'" + System.Environment.NewLine +
                            "FUEL=" + "'" + Fuels[FIRE.Fuel] + "'" + System.Environment.NewLine +
                            "CO_YIELD=" + FIRE.CO_YIELD + System.Environment.NewLine +
                            "SOOT_YIELD=" + FIRE.CO_YIELD + "/";
                    writer.WriteLine(REAC_line);

                    // Set fire position and HRRPUA, for 'VENT' fire
                    // Write the surface line
                    string SURF_LINE = "&SURF ID=" + "'" + AllObjectsNames[FIRE.NameIndex] + "', " + "HRRPUA=" + FIRE.HRRPUA + "/";
                    writer.WriteLine(SURF_LINE);
                    // Write the vent line for "vent" fire
                    fire_x0 = FIRE.xpos - (FIRE.Width / 2);
                    fire_x1 = FIRE.xpos + (FIRE.Width / 2);
                    fire_y0 = FIRE.ypos - (FIRE.Length / 2);
                    fire_y1 = FIRE.ypos + (FIRE.Length / 2);
                    fire_z = FIRE.zpos;
                    string FIRE_XB = "XB=" + fire_x0 + "," + fire_x1 + "," +
                                 fire_y0 + "," + fire_y1 + "," +
                                 fire_z + "," + fire_z;
                    string FIRE_VENT_LINE = "&VENT " + FIRE_XB + ", " + "SURF_ID=" + "'" + AllObjectsNames[FIRE.NameIndex] + "'/";
                    writer.WriteLine(FIRE_VENT_LINE);
                }

                // 4.3.6 Put in slices to be later used to extract soot density data
                string SLCF_line;
                int num_of_slices = 0;

                for (float slice_z = FDS_z0 + sceneDetails.GridSize;
                    slice_z < FDS_z1 + 0.5 * sceneDetails.GridSize;
                    slice_z += sceneDetails.GridSize)
                {
                    SLCF_line = "&SLCF PBZ=" + slice_z + ", " + "QUANTITY='DENSITY', SPEC_ID='SOOT'/";
                    writer.WriteLine(SLCF_line);
                    num_of_slices += 1;
                }

                // 4.3.7 End writing the FDS input file.
                writer.WriteLine("&TAIL/");
                writer.Close();

                // 5. Run the fds input file
                Process myProcess = new Process();
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.FileName = "cmd.exe";
                string cmdpath = WorkingDirectory.Replace("/", "\\");
                string path = "/c" + "cd " + cmdpath + "& fds " + subdir.Name + ".fds";
                myProcess.StartInfo.Arguments = path;
                myProcess.EnableRaisingEvents = true;
                myProcess.Start();
                myProcess.WaitForExit();

                // 6. Extract useful data (smoke density) from fds output file, out it
                // into a matrix form and store it in a binary file
                string head_name = subdir.Name;
                string output_name;
                string directory_location = WorkingDirectory.Replace("/", "\\");
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardInput = true;
                startInfo.FileName = "cmd.exe";
                startInfo.WorkingDirectory = directory_location;
                //startInfo.WindowStyle = ProcessWindowStyle.Hidden;

                Process process = new Process();
                process.StartInfo = startInfo;

                process.Start();

                process.StandardInput.WriteLine("& md TimeDensity");

                // Loop to output n t_n.txt files. For the moment, time step is set to be TimeStep
                float time_index = 0; float timestep = sceneDetails.TimeStep; float simtime = sceneDetails.SimTime;
                while (time_index * timestep < simtime)
                {
                    process.StandardInput.WriteLine("fds2ascii");
                    process.StandardInput.WriteLine(head_name);
                    process.StandardInput.WriteLine("2");
                    process.StandardInput.WriteLine("1");
                    process.StandardInput.WriteLine("n");
                    if ((time_index + 1) * timestep <= simtime)
                    {
                        process.StandardInput.WriteLine(timestep * time_index + " " + timestep * (time_index + 1));
                    }
                    else
                    {
                        process.StandardInput.WriteLine(timestep * time_index + " " + simtime);
                    }
                    process.StandardInput.WriteLine(num_of_slices);

                    for (int i = 1; i <= num_of_slices; i += 1)
                    {
                        process.StandardInput.WriteLine(i);
                    }
                    output_name = "t" + time_index;
                    process.StandardInput.WriteLine(output_name);
                    process.StandardInput.WriteLine("MOVE " + output_name + " " + "TimeDensity");
                    time_index += 1;
                }
                process.StandardInput.WriteLine("exit");
                process.WaitForExit();

                // Summarize the result by putting it into a list, AllData (A).
                // A[a] = density(t,x,y,z) where:
                // let d be the grid length, T simulation time, ts time step
                // i be the number of points on x axis: i = x/d + 1 (eg. 0, 0.5, 1)
                // j be the number of points on y axis: j = y/d + 1
                // k be the number of points on z axis: k = z/d (eg 0.5, 1; no 0)
                // l be the number of time points: SimTime/TimeStep (0.5, 1.5, ... 29.5, SimTime = 30, TimeStep = 1)
                // =>
                // *** t = quotient(a, i*j*k) + 0.5ts
                // o = remainder(a, i*j*k)
                // *** x = x0 + quotient(o, j*k)*d ***
                // p = remainder(o, j*k)
                // *** y = y0 + quotient(p, k)*d ***
                // *** z = z0 + remainder(p, k)*d ***
                List<float> AllData = new List<float>();
                string[] DataAtT;
                string Data;
                StreamReader reader;
                if (Directory.Exists(WorkingDirectory + "/TimeDensity"))
                {
                    for (int n = 0; n < simtime / timestep; n++)
                    {
                        reader = new StreamReader(WorkingDirectory + "/TimeDensity/t" + n);
                        // Skip the first two lines
                        reader.ReadLine();
                        reader.ReadLine();
                        Data = reader.ReadToEnd();
                        reader.Close();

                        DataAtT = Data.Split(',', '\n');

                        foreach (string str in DataAtT)
                        {
                            if (str != "")
                            {
                                AllData.Add(float.Parse(str));
                            }
                        }
                    }
                }
                BinaryFormatter BF = new BinaryFormatter();
                FileStream alldatafile = File.Open(WorkingDirectory + "/TimeDensity/AllData.dat", FileMode.OpenOrCreate);
                BF.Serialize(alldatafile, AllData);
                alldatafile.Close();
            }
        }

        // Set ScenarioName in GeneralGameManager.cs
        ScenarioAtHand.ScenarioName = ScenarioName.text;
    }
}
