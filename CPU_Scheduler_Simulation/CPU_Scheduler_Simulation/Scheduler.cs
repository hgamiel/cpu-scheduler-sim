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
        public Queue<PCB> readyThreads = new Queue<PCB>();     //global queue of ready threads
        public Scheduler() {}
        //for each idle processor, select thread from ready queue
        //using 2 processors (dual-core)
        public void loadBalance(List<PCB> processes) {
            //readyThreads = new Queue<PCB>(processes);   //take list of processes from file and put them into a queue
            ////run until there are no more processes
            ////we will be using dual-core processors
            //CPU cpu1 = new CPU();   
            //CPU cpu2 = new CPU();
            //while (readyThreads.Count != 0)
            //{
            //    //synchronous with cpu2
            //    if (cpu1.idle)
            //    {
            //        cpu1.idle = true;   //now the process is busy
            //        var thread = readyThreads.Dequeue();
            //        //CPU first
            //        cpu1.readyCPU = thread.
            //        cpu1.idle = false;  //once the process if finished, we are ready to grab the next thread
            //    }
                
            //}
        }
    }
}
