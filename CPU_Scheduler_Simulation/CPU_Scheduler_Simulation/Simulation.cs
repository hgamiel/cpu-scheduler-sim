using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Combinatorics.Collections;

namespace CPU_Scheduler_Simulation
{
    public class Simulation
    {
        public int quantum;                                 // quantum for the simulation
        List<PCB> processTable = new List<PCB>();           // contains list of PCBs
        public Scheduler SimScheduler = new Scheduler();    // create a scheduler object
        public string filename;                             // name of file containing information of PCBs
        public string filepath;                             // in the same folder as the solution file
        public bool debugStatements = false;
        List<PCB> copy = new List<PCB>();                   // holds a copy of the original list, processTable
        Data data = new Data();
        
        public Simulation() { }     // default constructor

        //pushes info from lines on .dat files into processTable as a PCB
        public void readDataFiles()
        {
            Console.WriteLine("--BEGIN FILE I/O");
            filename = "processes.dat";
            Directory.SetCurrentDirectory(@"..\..\..\"); // default is \bin\Debug -> this sets the current directory up a few folders
            filepath = Path.Combine(Environment.CurrentDirectory, filename);

            Console.WriteLine("\tFile + Path: {0}", filepath);

            if (File.Exists(filepath))
            {
                Console.WriteLine("\t{0} exists. File will be processed.", filepath); // debugging purposes

                var reader = new StreamReader(File.OpenRead(filepath)); // opens the read stream

                while (!reader.EndOfStream) // loops until end of file
                {
                    var line = reader.ReadLine(); // reads in line
                    var values = line.Split(new string[] {"\t"}, StringSplitOptions.RemoveEmptyEntries); // creates array of values in line
                    addProcess(values); // calls function that adds process to process table
                }
            
                Console.WriteLine("\tFile read completed.");
                Console.WriteLine("\tThere were " + processTable.Count + " processes added.");
                Console.WriteLine("--END FILE I/O\n");
                reader.Close();
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
            //copy.Add(process);
            if(debugStatements)Console.WriteLine("Process with PID {0} added.", process.PID);
        }

        //sets up the scheduler and runs the simulation
        public void startSim(List<int> quantum1, List<int> quantum2) {
            Console.WriteLine("Simulation beginning.\n");
            readDataFiles(); // reads in processes in the .dat file
            processTable = processTable.OrderBy(p => p.arrivalTime).ToList();           // order the processes by arrival time
            
            var integers = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8};                 // for all test cases

            //var x = new Permutations<int>(integers, GenerateOption.WithoutRepetition);  // create all permutations of the integers list - will run through 9! runs of the simulation
            List<PCB> copy = new List<PCB>();
            List<int> numCPU = new List<int> { 2, 3, 4 };
            //foreach (var v in integers)
            for (int i = 0; i < quantum1.Count; i++)
            {
                Console.WriteLine(data.currentRun(integers));
                // create a new scheduler every time we start the scheduler
                Scheduler simScheduler = new Scheduler();
                simScheduler.setQuantums(quantum1[i], quantum2[i]);                       // set the quantums
                simScheduler.numCPUs = numCPU[0];                                           // set the number of CPUs
                copy = processTable.ConvertAll(pcb => (PCB)pcb.Clone()).ToList();   // create a copy of the original list
                simScheduler.loadBalancing(processTable, integers);                        // load balances the processes
                endSim(processTable, simScheduler, integers);                                               // output the data
                processTable = copy;                                                // reset the processTable with original list in copy
                Console.ReadKey();
            }
            // data.writeStatsToFile();
        }

        //ends the simulation
        public void endSim(List<PCB> list, Scheduler scheduler, IList<int> v) {
            Console.WriteLine("Simulation complete.\n");
            Console.WriteLine(data.endSimOutput(scheduler));
            Statistics stats = new Statistics(list, scheduler);        // object that holds all of the stats
            stats.runStatistics();
            Console.ReadKey();
            data.addToLists(stats, v);
        }
    }
}
