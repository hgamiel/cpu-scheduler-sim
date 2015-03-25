using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Scheduler_Simulation
{
    public struct PCB
    {
        public int PID;                 //unique id to a PCB
        public int serviceTime;         //time required by a process
        public int priorityNumber;      //lowest interger -> higher priority
        public double arrivalTime;      //when processes arrive
        public double turnaroundTime;   //executionTime - arrivalTime
        public double waitTime;         //time process must wait during it execution - accumulated
        public double responseTime;     //time from submission utnil response begins to be received - accumulated
        public double executionTime;    //time of completion
        public int[] CPU;               //array of CPU 
        public int[] IO;                //array of IOs
    }
}
