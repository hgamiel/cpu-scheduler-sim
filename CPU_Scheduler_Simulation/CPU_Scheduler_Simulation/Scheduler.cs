using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Scheduler_Simulation
{
    public class Scheduler
    {
        // this simulation will be using two processors
        public CPU cpu1 = new CPU();
        public CPU cpu2 = new CPU();

        public Scheduler() { }  // default contructor 

        //for each idle processor, select thread from ready queue
        //version: first come first served to whichever processor is idle
        public void loadBalancing(List<PCB> processes)
        {
            var numProcesses = processes.Count;     // number of processors
            // assigning each half of the number of processors to each processor
            for (int i = 0; i < numProcesses; i++)
            {
                if (i % 2 == 0)
                    cpu1.waitingCPU.Enqueue(processes[i]);
                else
                    cpu2.waitingCPU.Enqueue(processes[i]);
            }

            runCPUs(); // runs the algorthms on processes on each CPU

            /* TODO: for now, we can just test our independent algorithms like this. 
             * once we complete them, we will create a function to put them into combinations
             */
            //cpu1.algorithms.v1Feedback(cpu1.waitingCPU);
            //cpu1.algorithms.v2Feedback(cpu1.waitingCPU); 
            
            //TODO: reset clock
            //cpu2.beginAlgorithms();
        }

        public void runCPUs()
        {
            Console.WriteLine("~~~~ ON CPU 1 ~~~~");
            runCPU1();
            Console.WriteLine("~~~~ ON CPU 2 ~~~~");
            runCPU2();
        }
        public void runCPU1()
        {
            int switchAlg = 0;
            bool CPUburst = true;
            List<PCB> finishedProcesses = new List<PCB>(); // TEST
            do
            {
                if (CPUburst) // if it's time to process the CPU bursts of processes
                {
                    switch (switchAlg)
                    {
                        case 0: finishedProcesses = cpu1.algorithms.fcfs(cpu1.waitingCPU, CPUburst); break;
                        //case 1: finishedProcesses = cpu1.algorithms.spn(cpu1.waitingCPU); break; // uncomment when done
                        //case 2: finishedProcesses = cpu1.algorithms.srt(cpu1.waitingCPU); break; // uncomment when done
                        //case 3: finishedProcesses = cpu1.algorithms.hrrn(cpu1.waitingCPU); break; // uncomment when done
                        //case 4: finishedProcesses = cpu1.algorithms.rr(cpu1.waitingCPU, quantum); break; // uncomment when done
                        //case 6: finishedProcesses = cpu1.algorithms.priority(cpu1.waitingCPU); break; // uncomment when done
                        case 7: finishedProcesses = cpu1.algorithms.v1Feedback(cpu1.waitingCPU); break;
                        case 8: finishedProcesses = cpu1.algorithms.v2Feedback(cpu1.waitingCPU); break;
                    }
                    for (int i = 0; i < finishedProcesses.Count; i++)
                    {
                        cpu1.waitingIO.Enqueue(finishedProcesses[i]);
                    }
                    //switchAlg++; // uncomment when other algs are done! This is a counter for the switch case
                }
                else // if it's time to process the IO bursts of processes
                {
                    finishedProcesses = cpu1.algorithms.fcfs(cpu1.waitingIO, CPUburst); // will always run FCFS in IO burst
                    for (int i = 0; i < finishedProcesses.Count; i++)
                    {
                        cpu1.waitingCPU.Enqueue(finishedProcesses[i]);
                    }
                }
                CPUburst = !CPUburst;
            } while (cpu1.waitingIO.Count != 0 || cpu1.waitingCPU.Count != 0);
        }
        public void runCPU2()
        {
            bool CPUburst = true;
            List<PCB> finishedProcesses = new List<PCB>(); // TEST
            do
            {
                if (CPUburst)
                {
                    finishedProcesses = cpu2.algorithms.fcfs(cpu2.waitingCPU, CPUburst);
                    for (int i = 0; i < finishedProcesses.Count; i++)
                    {
                        cpu2.waitingIO.Enqueue(finishedProcesses[i]);
                    }
                }
                else
                {
                    finishedProcesses = cpu2.algorithms.fcfs(cpu2.waitingIO, CPUburst);
                    for (int i = 0; i < finishedProcesses.Count; i++)
                    {
                        cpu2.waitingCPU.Enqueue(finishedProcesses[i]);
                    }
                }
                CPUburst = !CPUburst;
            } while (cpu2.waitingIO.Count != 0 || cpu2.waitingCPU.Count != 0);
        }
    }
}
