﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Scheduler_Simulation
{
    public class Algorithm
    {
        public List<PCB> finishedProcesses = new List<PCB>(); //global list of finished processes for data processing

        public int contextSwitchCost = 0;   //cost of switching between processes. assumption: cost is one per switch

        //public int counter;           //if we wish to implement a counter age solution
        //age solution - when switching from readyIO to waitingCPU, organize the queue to put the oldest processes first

        //create some sample data
        public Queue<PCB> sample = new Queue<PCB>
        (
            new[]{ 
                    new PCB(){name = 'A', arrivalTime = 0, serviceTime = 3},
                    new PCB(){name = 'B', arrivalTime = 2, serviceTime = 6},
                    new PCB(){name = 'C', arrivalTime = 4, serviceTime = 4},
                    new PCB(){name = 'D', arrivalTime = 6, serviceTime = 5},
                    new PCB(){name = 'E', arrivalTime = 8, serviceTime = 2}
                }
        );
        public Algorithm() { }

        //TODO: beingAlgorithms(); 

        //first-come-first-serve algorithm - Hannh
        public List<PCB> fcfs(Queue<PCB> processes) 
        {
            Console.WriteLine("--BEGIN FIRST COME FIRST SERVE--");
            //foreach (var process in processes)
            //    processes.Dequeue();
            int quantum = 1;
            int contextswitch = 1;
            bool finished = false;   //when algorithm is complete
            int counter = 0;    //'timer' since we are modeling as discrete events
            PCB process = new PCB();    //temporary holder
            List<PCB> finishedProcesses = new List<PCB>();

            do // assuming the processes in the queue are ordered by arrival time...
            {
                counter += contextswitch; // context switch time
                while (counter < sample.Peek().arrivalTime) // just incase there are processes that arrive much later than when the first process is finished
                {
                    counter++;
                }
                process = sample.Dequeue();
                Console.WriteLine("Process " + process.name + " has been serviced " + process.serviceTime + ".");
                counter += process.serviceTime; // if we're in this function to serve CPU burst, then let's add the CPU burst to the counter. Else, I/O burst.
                finishedProcesses.Add(process);
            } while (sample.Count != 0);

            finished = true;

            Console.WriteLine("--END FIRST COME FIRST SERVE--");

            Console.WriteLine(counter);

            return finishedProcesses; 
        }

        //shortest-process-next algorithm - Wilo
        public List<PCB> spn(Queue<PCB> processes)
        { 
            //after a process has completed, observe all processes that have arrived and use shortest service time
            return null; 
        }

        //shortest-remaining-time algorithm - Brady
        public List<PCB> srt(Queue<PCB> processes)
        { 
            //as processes arrive, compute service time
            //after computing, observe all processes that have arrived and use the one with the shortest service time
            return null;  
        }

        //highest-response-ratio-next algorithm - Wilo
        public List<PCB> hrrn(Queue<PCB> processes)
        { 
            //ration = (wait time + service time) / service time
            return null; 
        }

        //round robin algorithm - Hannah
        public List<PCB> rr(Queue<PCB> processes, int quantum)
        { 
            //create an empty queue
            //as quantums deplete the service time, match with arrival time
            //processes get added to queue when the arrive
            //alternate processes
            return null; 
        }

        //priority algorithm - Brady
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
            var finished = false;   //when algorithm is complete
            var counter = 0;    //'timer' since we are modeling as discrete events
            var process = new PCB();    //temporary holder
            var startIndex = 0;     //start index of the ready queues
            var numProcesses = 0;

            numProcesses = sample.Count;

            //create a list of queues
            List<Queue<PCB>> rq = new List<Queue<PCB>>();
            for (int i = 0; i < 6; i++)
            {
                rq.Add(new Queue<PCB>());
            }

            //first process
            process = sample.Dequeue();
            while (counter != sample.Peek().arrivalTime)
            {
                counter++;
                Console.WriteLine("Process " + process.name + " service time is " + process.serveTime(quantum));
            }
            //assuming first process will not finish before next process comes in - not realistic of a CPU
            rq[++startIndex].Enqueue(process);

            while (!finished)
            {
                //if a process has arrived, get the process and start our index of queues back at 0
                if (sample.Count != 0)
                {
                    if (counter == sample.Peek().arrivalTime)
                    {
                        process = sample.Dequeue();
                        startIndex = 0;
                        rq[startIndex].Enqueue(process);
                    }
                }
                //if inbetween queues are empty, move along until we get to next queue that has elements
                while (rq[startIndex].Count == 0)
                    startIndex++;
                //take the process of the current queue
                process = rq[startIndex].Dequeue();
                //the process serves a certain amount of time
                Console.WriteLine("Process " + process.name + " service time is " + process.serveTime(quantum));
                //this happens in one unit of time
                counter++;
                if (process.serviceTime == 0)
                {
                    process.finished = true;                //process is finished
                    process.executionTime = counter;        //set the finished time of the process
                    process.turnaroundTime = process.executionTime - process.arrivalTime;
                    process.tr_ts = process.turnaroundTime / process.serviceTime;
                    finishedProcesses.Add(process);         //add to list of finished processes
                    //check to see if we are finished
                    if (finishedProcesses.Count == numProcesses)
                        finished = true;
                    Console.WriteLine("Process " + process.name + " finished at time " + process.executionTime);
                    continue;
                }
                //move to the next queue
                rq[++startIndex].Enqueue(process);

                //if there are no processes in this queue, then we move to the next queue
                if (rq[--startIndex].Count == 0)
                    startIndex++;

                //incorporate the context switch cost when switching between processes
                contextSwitchCost++;
            }
            Console.WriteLine("Finished feedback algorithm");
            return null;
        }    

        //version 2 feedback with quantum = 2^i - Tommy
        public List<PCB> v2Feedback(Queue<PCB> processes)
        {
            var finished = false;   
            var counter = 0;    
            var process = new PCB();    
            var startIndex = 0;     
            var numProcesses = 0;

            numProcesses = sample.Count;

            List<Queue<PCB>> rq = new List<Queue<PCB>>();
            for (int i = 0; i < 4; i++)
            {
                rq.Add(new Queue<PCB>());
            }

            process = sample.Dequeue();
            while (counter != sample.Peek().arrivalTime)
            {
                counter++;
                //quantum is only (2^0)=1 for this case
                Console.WriteLine("Process " + process.name + " service time is " + process.serveTime(1.00));
            }

            rq[++startIndex].Enqueue(process);

            while (!finished)
            {
                //if a process has arrived either on time or has been waiting, get the process and start our index of queues back at 0
                if (sample.Count != 0)
                {
                    if (counter >= sample.Peek().arrivalTime)
                    {
                        process = sample.Dequeue();
                        startIndex = 0;
                        rq[startIndex].Enqueue(process);
                    }
                }

                while (rq[startIndex].Count == 0)
                    startIndex++;

                process = rq[startIndex].Dequeue();
                //the process serves 2^1 amount of time
                var quantum = Math.Pow(2.00, (Double)startIndex);
                var processServeTime = Convert.ToInt32(process.serveTime(quantum));
                Console.WriteLine("Process " + process.name + " service time is " + processServeTime);

                //must check if the process finished before the quantum amount
                if (processServeTime < 0)
                    counter += (processServeTime * (-1));
                else
                    counter += (int)quantum;    //otherwise we just set it to the total time
                if (process.serviceTime <= 0)
                {
                    process.finished = true;                
                    process.executionTime = counter;
                    process.turnaroundTime = process.executionTime - process.arrivalTime;
                    process.tr_ts = process.turnaroundTime / process.serviceTime;
                    finishedProcesses.Add(process);         
                    //check to see if we are finished
                    if (finishedProcesses.Count == numProcesses) 
                        finished = true;
                    Console.WriteLine("Process " + process.name + " finished at time " + process.executionTime);
                    continue;
                }

                rq[++startIndex].Enqueue(process);

                if (rq[--startIndex].Count == 0)
                    startIndex++;

                contextSwitchCost++;
            }
            Console.WriteLine("Finished feedback algorithm");
            return null;
        }
    }
}
