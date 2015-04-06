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
        public int quantum;     // quantum for the simulation
        public List<PCB> processTable = new List<PCB>();    // contains list of PCBs
        public Scheduler SimScheduler = new Scheduler();    // create a scheduler object
        public string filename;     // name of file containing information of PCBs
        public string filepath;     // in the same folder as the solution file

        public Simulation() { }     // default constructor

        //pushes info from lines on .dat files into processTable as a PCB
        public void readDataFiles()
        {
            filename = "processes.dat";
            Directory.SetCurrentDirectory(@"..\..\..\"); // default is \bin\Debug -> this sets the current directory up a few folders
            filepath = Path.Combine(Environment.CurrentDirectory, filename);

            Console.WriteLine("File + Path: {0}", filepath);

            if (File.Exists(filepath))
            {
                Console.WriteLine("{0} exists. File will be processed.", filepath); // debugging purposes

                var reader = new StreamReader(File.OpenRead(filepath)); // opens the read stream

                while (!reader.EndOfStream) // loops until end of file
                {
                    var line = reader.ReadLine(); // reads in line
                    var values = line.Split(new string[] {"\t"}, StringSplitOptions.RemoveEmptyEntries); // creates array of values in line
                    addProcess(values); // calls function that adds process to process table
                }

                Console.WriteLine("File read completed.\n");
            }
            else
            {
                Console.WriteLine("File {0} not found.", filename);
            }
        }

        // adds a process onto the process table. called from readDataFiles()
        public void addProcess(string [] values) {
            PCB process = new PCB(); // the new process we will add to our list
            process.PID = Convert.ToInt32(values[0]); // process PID
            process.priorityNumber = Convert.ToInt32(values[1]); // process priority number
            process.arrivalTime = Convert.ToInt32(values[2]); // process arrival time

            for (int i = 3; i < values.Length; i++) // since bursts start at the 4th column (index 3 in an array), we start reading in bursts there
            {
                var burst = Convert.ToInt32(values[i]); // burst time
                if (i % 2 != 0) // if in an odd column, then it's a CPU burst
                {
                    process.CPU.Enqueue(burst); // add CPU burst to the process' CPU burst list
                }
                else // then it's in an even column, so it's an I/O burst
                {
                    process.IO.Enqueue(burst); // add I/O burst to the process' I/O burst list
                }
            }
            processTable.Add(process); // adds process to process table
            Console.WriteLine("Process with PID {0} added.", process.PID);
        }

        //sets up the scheduler and runs the simulation
        public void startSim(int quantum) {
            this.quantum = quantum; // sets quantum for the simulation (should be 1 or 2; some very small number)
            readDataFiles(); // reads in processes in the .dat file
            processTable = processTable.OrderBy(p => p.arrivalTime).ToList();
            SimScheduler.loadBalancing(processTable); // load balances the processes. Will uncomment when it's ready
            endSim();
        }

        //ends the simulation
        public void endSim() {
            Console.WriteLine("Simulation complete.");
            Console.WriteLine("Total time spent across both CPUs: " + SimScheduler.calcTotalTime());
            Console.ReadKey();
        }               
    }
}
