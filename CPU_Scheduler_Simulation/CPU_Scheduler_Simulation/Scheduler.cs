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

            /* TODO: for now, we can just test our independent algorithms like this. 
             * once we complete them, we will create a function to put them into combinations
             */
            //cpu1.algorithms.v1Feedback(cpu1.waitingCPU);
            //cpu1.algorithms.v2Feedback(cpu1.waitingCPU); 
            bool CPUburst = true;
            List<PCB> finishedProcesses = new List<PCB>(); // TEST
            do
            {
                if (CPUburst)
                {
                    finishedProcesses = cpu1.algorithms.fcfs(cpu1.waitingCPU, CPUburst);
                    for (int i = 0; i < finishedProcesses.Count; i++)
                    {
                        cpu1.waitingIO.Enqueue(finishedProcesses[i]);
                    }
                }
                else
                {
                    finishedProcesses = cpu1.algorithms.fcfs(cpu1.waitingIO, CPUburst);
                    for (int i = 0; i < finishedProcesses.Count; i++)
                    {
                        cpu1.waitingCPU.Enqueue(finishedProcesses[i]);
                    }
                }
                CPUburst = !CPUburst;
            } while (cpu1.waitingIO.Count != 0 || cpu1.waitingCPU.Count != 0);
            //TODO: reset clock
            //cpu2.beginAlgorithms();
        }
    }
}
