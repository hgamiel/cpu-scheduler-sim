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
            cpu1.algorithms.v1Feedback(cpu1.waitingCPU); 
            //TODO: reset clock
            //cpu2.beginAlgorithms();
        }
    }
}
