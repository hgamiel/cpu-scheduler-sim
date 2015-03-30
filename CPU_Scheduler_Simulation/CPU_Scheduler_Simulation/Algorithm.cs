using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Scheduler_Simulation
{
    public class Algorithm
    {
        public int contextSwitchCost;
        //public int counter;           //if we wish to implement a counter age solution

        public Algorithm() { }

        //first-come-first-serve algorithm
        public List<PCB> fcfs(Queue<PCB> processes) 
        {
            //foreach (var process in processes)
            //    processes.Dequeue();
            return null; 
        }

        //shortest-process-next algorithm
        public List<PCB> spn(Queue<PCB> processes)
        { 
            //after a process has completed, observe all processes that have arrived and use shortest service time
            return null; 
        }

        //shortest-remaining-time algorithm
        public List<PCB> srt(Queue<PCB> processes)
        { 
            //as processes arrive, compute service time
            //after computing, observe all processes that have arrived and use the one with the shortest service time
            return null;  
        }

        //highest-response-radio-next algorithm
        public List<PCB> hrrn(Queue<PCB> processes)
        { 
            //ration = (wait time + service time) / service time
            return null; 
        }

        //round robin algorithm
        public List<PCB> rr(Queue<PCB> processes, int quantum)
        { 
            //create an empty queue
            //as quantums deplete the service time, match with arrival time
            //processes get added to queue when the arrive
            //alternate processes
            return null; 
        }

        //priority algorithm
        public List<PCB> priority(Queue<PCB> processes)
        { 
            //lowest interger priority number has highest priority
            return null; 
        }

        //feedback algorithms if we wish to implement a feedback age solution

        //version 1 feedback with quantum = 1
        public List<PCB> v1Feedback(Queue<PCB> processes)
        {
            int quantum = 1;
            //use multiple level queues until complete
            return null;
        }    

        //version 2 feedback with quantum = 2^i
        public List<PCB> v2Feedback(Queue<PCB> processes)
        {
            //quantum = 2^i where i is the level of the queue starting at 0
            //use multiple queues until complete
            return null;
        }
    }
}
