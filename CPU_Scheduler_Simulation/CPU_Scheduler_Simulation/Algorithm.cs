using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Scheduler_Simulation
{
    public class Algorithm
    {
        public List<PCB> finishedProcesses = new List<PCB>(); //global list of finished processes for data processing
        public int timeCounter = 0;
        public int contextSwitchCost = 1;   //cost of switching between processes. assumption: cost is one per switch

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
        public List<PCB> fcfs(Queue<PCB> processes, bool CPUburst) // CPUburst is bool so we know to access the IO burst or CPU burst of the process
        {
            Console.WriteLine("--BEGIN FIRST COME FIRST SERVE");
            Console.WriteLine("\tNumber of processes to be serviced this round: " + processes.Count);
            Console.WriteLine("\tCurrent burst processing: " + ((CPUburst) ? "CPU" : "I/O"));

            int counter = 0;    //'timer' since we are modeling as discrete events
            PCB process = new PCB();    //temporary holder
            List<PCB> nonEmptyProcesses = new List<PCB>();

            int beginAmount = processes.Count;

            do // assuming the processes in the queue are ordered by arrival time...
            {
                counter += contextSwitchCost; // context switch time
                while (counter < processes.Peek().arrivalTime) // just incase there are processes that arrive much later than when the first process is finished
                {
                    counter++;
                }
                process = processes.Dequeue();
                int service = (CPUburst) ? process.CPU.Dequeue() : process.IO.Dequeue();
                counter += service;
                if ((CPUburst && process.IO.Count > 0) || (!CPUburst && process.CPU.Count > 0)) // if we still have IO or CPU bursts to process...
                {
                    nonEmptyProcesses.Add(process); // add it to the process list that still needs to further processed
                }
                else
                {
                    finishedProcesses.Add(process); // add it to the list of "finished" processes (processes that don't have any more bursts)
                }
            } while (processes.Count != 0);

            timeCounter += counter; // update our total time spent in algorithms

            Console.WriteLine("\tTime spent in this round of FCFS: " + counter);
            Console.WriteLine("\tNumber of processes serviced: " + (beginAmount - nonEmptyProcesses.Count));
            Console.WriteLine("\tAmount of processes left in FCFS: " + nonEmptyProcesses.Count);
            Console.WriteLine("\tTotal time accumulated so far: " + timeCounter);
            Console.WriteLine("--END FIRST COME FIRST SERVE\n");

            return nonEmptyProcesses; 
        }

        //shortest-process-next algorithm - Wilo
        public List<PCB> spn(Queue<PCB> processes)
        { 
            //after a process has completed, observe all processes that have arrived and use shortest service time
            // first  check  list and see the arrival time of process 
            // if there a time that arrived at 0 compute that time
            // this is non preeptive so once process is picked it has to be finished
            //when the first process finishes check process with shortest service time then compute that until  the queue is compute
            Console.WriteLine("--Begin Shortest Process Next");
            int counterVar = 0;
            List<PCB> filledProcessList = new List<PCB>();
            PCB process = new PCB();
            int intialNum = sample.Count;
            do 
            { 
                contextSwitchCost+=counterVar;
                while (counterVar < sample.Peek().arrivalTime)
                {
                    counterVar++;
                }
                process = sample.Dequeue();
                if(process.serviceTime ==0)
                {
                    finishedProcesses.Add(process);
                    break;
                }
                else if (process.serviceTime != 0) 
                {
                    /// will look and find shortest burst time
                    var lowest = (from c in sample
                                  where c.serviceTime == sample.Min(i => i.serviceTime)
                                  select c).FirstOrDefault();
                    finishedProcesses.Add(process);
                }
            } while (processes.Count != 0);
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

            Console.WriteLine("--BEGIN ROUND ROBIN");
            Console.WriteLine("\tNumber of processes to be serviced this round: " + processes.Count);

            int counter = 0;    //'timer' since we are modeling as discrete events
            PCB process = new PCB();    //temporary holder
            List<PCB> nonEmptyProcesses = new List<PCB>(); // this will be the list that we return in the end.
            List<PCB> currProcesses = new List<PCB>(); // the processes we will be processing in RR
                                                        // (processes from "processes" will be pushed on once they reach their arrival time)

            int serveTimeLeftOnProcess;
            int calcTimeSpentOnProcess;
            int calcRunningTimeOnProcesses;
            int beginAmount = processes.Count;
            int queueCounter = 1;

            do // assuming the processes in the queue are ordered by arrival time...
            {
                if (currProcesses.Count == 0 || processes.Peek().arrivalTime > counter) // if no processes have arrived yet... (or we're done processing the ones with short bursts)
                {
                    counter += contextSwitchCost; // context switch time
                    while (counter < processes.Peek().arrivalTime) // just incase there are processes that arrive much later than when the first process is finished
                    {
                        counter++;
                    }
                    process = processes.Dequeue(); // take the process off and then...
                    process.serviceTime = process.CPU.Dequeue();
                    currProcesses.Add(process); // finally add that process to be processed by rr
                }

                Console.WriteLine("\t\tBeginning queue #" + queueCounter + " with " + currProcesses.Count + " processes...");
                for(int i = 0; i < currProcesses.Count; i++) {
                    counter += contextSwitchCost;
                    serveTimeLeftOnProcess = currProcesses[i].serviceTime;
                    currProcesses[i].serviceTime = ((serveTimeLeftOnProcess - quantum) > 0) ? serveTimeLeftOnProcess - quantum : 0;
                    if (currProcesses[i].serviceTime == 0)
                    {
                        if ((currProcesses[i].IO.Count > 0) || (currProcesses[i].CPU.Count > 0)) // if we still have IO or CPU bursts to process...
                        {
                            nonEmptyProcesses.Add(currProcesses[i]); // add it to the process list that still needs to further processed
                        }
                        else
                        {
                            finishedProcesses.Add(currProcesses[i]); // add it to the list of "finished" processes (processes that don't have any more bursts)
                        }
                        currProcesses.RemoveAt(i);
                        i--;
                    }
                    calcTimeSpentOnProcess = ((serveTimeLeftOnProcess - quantum) > 0) ? serveTimeLeftOnProcess - quantum : quantum;
                    counter += calcTimeSpentOnProcess;
                }
               // Console.WriteLine("\t\tEnding queue #" + queueCounter + " with " + currProcesses.Count + " processes left...");
                //queueCounter++;
           
            } while (processes.Count != 0);

            timeCounter += counter;

            Console.WriteLine("\tTime spent in this round of RR: " + counter);
            Console.WriteLine("\tNumber of processes serviced: " + (beginAmount - nonEmptyProcesses.Count));
            Console.WriteLine("\tAmount of processes left in RR: " + nonEmptyProcesses.Count);
            Console.WriteLine("\tTotal time accumulated so far: " + timeCounter);
            Console.WriteLine("--END FIRST COME FIRST SERVE\n");

            return nonEmptyProcesses; 
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
