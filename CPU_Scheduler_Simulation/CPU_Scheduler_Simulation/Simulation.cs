using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CPU_Scheduler_Simulation
{
    public class Simulation
    {
        public int quantum;
        public List<PCB> processTable = new List<PCB>();
        public Scheduler SimScheduler = new Scheduler();
        public string filename;
        public string filepath;

        public Simulation() { }
        public void readDataFiles()
        {

            filename = "processes.dat";

            filepath = "C:\\Users\\hgamiel15\\Documents\\cpu-scheduler-sim\\";

            Console.WriteLine("File + Path: {0}", filepath + filename);

            if (File.Exists(filepath + filename))
            {
                Console.WriteLine("{0} exists. File will be processed.", filepath + filename);

                var reader = new StreamReader(File.OpenRead(filepath + filename));

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split('\t');
                    PCB process = new PCB();
                    process.PID = Convert.ToInt32(values[0]);
                    process.priorityNumber = Convert.ToInt32(values[1]);
                    process.arrivalTime = Convert.ToDouble(values[2]);
                    for (int i = 3; i < values.Length; i++)
                    {
                        //var burst = Convert.ToInt32(values[i]);
                        //if (i % 2 != 0)
                        //{
                        //    process.CPU.Add(burst);
                        //}
                        //else
                        //{
                        //    process.IO.Add(burst);
                        //}
                    }
                    processTable.Add(process);
                    Console.WriteLine("Process with PID {0} added.", process.PID);
                }
            }
            else
            {
                Console.WriteLine("File {0} not found.", filename);
            }



        }        //pushes info from lines on .dat files into processTable as a PCB

        public void startSim(int quantum) {
            readDataFiles();
        }  //sets up the scheduler and runs the simulation
        public void endSim() { }               //ends the simulation
    }
}
