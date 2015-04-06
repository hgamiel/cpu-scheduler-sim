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

        }

        public int calcTotalTime() // returns the total time spent across both CPUs
        {
            int sum;
            sum = cpu1.algorithms.timeCounter + cpu2.algorithms.timeCounter;
            return sum;
        }

        public void runCPUs()
        {
            Console.WriteLine("~~~~ ON CPU 1 ~~~~");
            runCPU(cpu1);
            Console.WriteLine("~~~~ ON CPU 2 ~~~~");
            runCPU(cpu2);
        }

        public void runCPU(CPU currCPU)
        {
            int switchAlg = 0; // starting algorithm
            bool CPUburst = true;
            List<PCB> nonEmptyProcesses = new List<PCB>(); // TEST
            do
            {
                if (CPUburst) // if it's time to process the CPU bursts of processes
                {
                    switch (switchAlg)
                    {
                        case 0: nonEmptyProcesses = currCPU.algorithms.fcfs(currCPU.waitingCPU, CPUburst); break;
                        //case 1: nonEmptyProcesses = currCPU.algorithms.spn(currCPU.waitingCPU); break; // uncomment when done
                        //case 2: nonEmptyProcesses = currCPU.algorithms.srt(currCPU.waitingCPU); break; // uncomment when done
                        //case 3: nonEmptyProcesses = currCPU.algorithms.hrrn(currCPU.waitingCPU); break; // uncomment when done
                        //case 4: nonEmptyProcesses = currCPU.algorithms.rr(currCPU.waitingCPU, quantum); break; // uncomment when done
                        //case 6: nonEmptyProcesses = currCPU.algorithms.priority(currCPU.waitingCPU); break; // uncomment when done
                        //case 7: nonEmptyProcesses = currCPU.algorithms.v1Feedback(currCPU.waitingCPU); break; // uncomment when done
                        //case 8: nonEmptyProcesses = currCPU.algorithms.v2Feedback(currCPU.waitingCPU); break; // uncomment when done
                        default: Console.WriteLine("Algorithm " + switchAlg + " does not exist (yet); Skipping algorithm...\n");
                                    switchAlg = (switchAlg + 1) % 8;
                                    continue; // will ignore rest of do/while and go through again with updated value for switch statement
                    }
                    for (int i = 0; i < nonEmptyProcesses.Count; i++)
                    {
                        currCPU.waitingIO.Enqueue(nonEmptyProcesses[i]);
                    }
                    switchAlg = (switchAlg + 1) % 8;
                }
                else // if it's time to process the IO bursts of processes
                {
                    nonEmptyProcesses = currCPU.algorithms.fcfs(currCPU.waitingIO, CPUburst); // will always run FCFS in IO burst
                    for (int i = 0; i < nonEmptyProcesses.Count; i++)
                    {
                        currCPU.waitingCPU.Enqueue(nonEmptyProcesses[i]);
                    }
                }
                CPUburst = !CPUburst;
            } while (currCPU.waitingIO.Count != 0 || currCPU.waitingCPU.Count != 0);
        }
    }
}
