using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Scheduler_Simulation
{
    public class Scheduler
    {
        public CPU cpu1 = new CPU();
        public CPU cpu2 = new CPU();
        //public Queue<PCB> readyThreads;     //global queue of ready threads
        public Scheduler() {}
        //for each idle processor, select thread from ready queue
        //version: first come first served to whichever processor is idle
        public void loadBalance(List<PCB> processes) {
            var numProcesses = processes.Count;
            for (int i = 0; i < numProcesses; i++)
            {
                if (i % 2 == 0)
                {
                    cpu1.waitingCPU.Enqueue(processes[i]);
                } else
                {
                    cpu2.waitingCPU.Enqueue(processes[i]);
                }
            }
            //cpu1.beginAlgorithms(); //reset clock
            //cpu2.beginAlgorithms();
        }
    }
}
