using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace CPU_Scheduler_Simulation
{
    public class Algorithm
    {
        public List<PCB> finishedProcesses = new List<PCB>(); //global list of finished processes for data processing
        public int timeCounter = 0;
        public int contextSwitchCost = 1;   //cost of switching between processes. assumption: cost is one per switch
        bool debugStatements = false;

        //public int counter;           //if we wish to implement a counter age solution
        //age solution - when switching from readyIO to waitingCPU, organize the queue to put the oldest processes first

        //create some sample data
        public Queue<PCB> sample = new Queue<PCB>
               (
                   new[]{ 
                    new PCB(){name = 'A', arrivalTime = 0, serviceTime = 3, priorityNumber = 1},
                    new PCB(){name = 'B', arrivalTime = 2, serviceTime = 6, priorityNumber = 4},
                    new PCB(){name = 'C', arrivalTime = 4, serviceTime = 4, priorityNumber = 5},
                    new PCB(){name = 'D', arrivalTime = 6, serviceTime = 5, priorityNumber = 2},
                    new PCB(){name = 'E', arrivalTime = 8, serviceTime = 2, priorityNumber = 3}
                    }
               );

        public Queue<PCB> sample2 = new Queue<PCB>
            (
                new[] {
                    new PCB(){PID = 1, arrivalTime = 0, serviceTime = 50},
                    new PCB(){PID = 2, arrivalTime = 20, serviceTime = 20},
                    new PCB(){PID = 3, arrivalTime = 40, serviceTime = 100},
                    new PCB(){PID = 4, arrivalTime = 60, serviceTime = 60}
                }
            );
        public Algorithm() { }

        //first-come-first-serve algorithm - Hannah
        public List<PCB> fcfs(Queue<PCB> processes, bool CPUburst) // CPUburst is bool so we know to access the IO burst or CPU burst of the process
        {
            Console.WriteLine("--BEGIN FIRST COME FIRST SERVE");
            Console.WriteLine("\tNumber of processes to be serviced this round: " + processes.Count);
            Console.WriteLine("\tCurrent burst processing: " + ((CPUburst) ? "CPU" : "I/O"));

            int counter = 0;
            int beginAmount = processes.Count;
            int service;
            PCB process = new PCB();
            List<PCB> nonEmptyProcesses = new List<PCB>();

            do
            {
                counter += contextSwitchCost;
                while (counter < processes.Peek().arrivalTime)
                {
                    counter++;
                }
                process = processes.Dequeue();
                service = (CPUburst) ? process.CPU.Dequeue() : process.IO.Dequeue();
                if (process.responseTime == -1)
                {
                    process.responseTime = counter;
                }
                process.waitTime += counter - process.arrivalTime;
                counter += service;
                if ((CPUburst && process.IO.Count > 0) || (!CPUburst && process.CPU.Count > 0))
                {
                    nonEmptyProcesses.Add(process);
                }
                else
                {
                    process.finished = true;
                    process.executionTime = timeCounter + counter;
                    process.determineTurnaroundTime();
                    process.determineTRTS(service);
                    finishedProcesses.Add(process);
                }
            } while (processes.Count != 0);

            timeCounter += counter;

            Console.WriteLine("\tTime spent in this round of FCFS: " + counter);
            Console.WriteLine("\tNumber of processes finished this round: " + (beginAmount - nonEmptyProcesses.Count));
            Console.WriteLine("\tAmount of processes left to finish/process: " + nonEmptyProcesses.Count);
            Console.WriteLine("\tAmount of processes \"done\" (no more bursts) so far: " + finishedProcesses.Count);
            Console.WriteLine("\tTotal time accumulated so far: " + timeCounter);
            Console.WriteLine("--END FIRST COME FIRST SERVE\n");

            return nonEmptyProcesses;
        }

        //shortest-process-next algorithm - Wilo
        public List<PCB> spn(Queue<PCB> processes ,bool CPUburst)
        {
            List<PCB> nonEmptyProcesses = new List<PCB>();
            var list = processes.ToList();
            int counter = 0;
            Collection<PCB> collection = new Collection<PCB>(list);
            List<PCB> temp = new List<PCB>();
            var process = new PCB();
            while (counter < processes.Peek().arrivalTime)
                counter++;
            while (collection.Count != 0)
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    if (collection[i].arrivalTime <= counter)
                        temp.Add(collection[i]);
                }
                var min = temp.Min(x => x.serviceTime);
                process = temp.First(x => x.serviceTime == min);
                process.serviceTime = process.CPU.Dequeue();
                counter += process.serviceTime;
                process.serviceTime -= process.serviceTime;
                Console.WriteLine("Process " + process.PID + " has finished");
                if ((CPUburst && process.IO.Count > 0) || (!CPUburst && process.CPU.Count > 0))
                {
                    nonEmptyProcesses.Add(process);
                }
                else
                {
                    finishedProcesses.Add(process);
                }
                collection.Remove(process);
                temp.Clear();
            }
            return nonEmptyProcesses;
        }

        //shortest-remaining-time algorithm - Brady
        public List<PCB> srt(Queue<PCB> processes, bool CPUburst)
        {
            //as processes arrive, compute service time
            Console.WriteLine("====START OF SRTF====");
            Console.WriteLine("\tNumber of processes to be serviced this round: " + processes.Count);
            int counter = 0;
            var List = processes.ToList();
            Collection<PCB> collection = new Collection<PCB>(List);
            List<PCB> nonEmptyProcesses = new List<PCB>();
            List<PCB> tempList = new List<PCB>();
            PCB process = new PCB();

            while (counter < processes.Peek().arrivalTime)
            {
                counter++;
            }
            process = collection[0];
            counter++;
            process.serviceTime = process.CPU.Dequeue();
            process.serviceTime--;

            while (collection.Count != 0)
            {
                for (int i = 0; i < collection.Count; ++i)
                {
                    if (collection.ElementAt(i).arrivalTime <= counter)
                        tempList.Add(collection.ElementAt(i));
                }
                var min = tempList.Min(p => p.serviceTime);
                var tempProcess = tempList.First(p => p.serviceTime == min);
                if (process.serviceTime > tempProcess.serviceTime)
                {
                    process = tempProcess;
                    if (!tempProcess.alreadyProcessed)
                    {
                        tempProcess.alreadyProcessed = true;
                        process.serviceTime = process.CPU.Dequeue();
                    }
                }
                counter++;
                process.serviceTime -= 1;

                if (process.serviceTime == 0)
                {
                    Console.WriteLine("Process " + process.PID + " has finished");
                    if ((CPUburst && process.IO.Count > 0) || (!CPUburst && process.CPU.Count > 0))
                    {
                        nonEmptyProcesses.Add(process);
                    }
                    else
                    {
                        finishedProcesses.Add(process);
                    }
                    collection.Remove(process);
                    if (collection.Count != 0)
                    {
                        process = collection[0];
                        if (!tempProcess.alreadyProcessed)
                        {
                            tempProcess.alreadyProcessed = true;
                            process.serviceTime = process.CPU.Dequeue();
                        }
                    }
                }
                tempList.Clear();
            }

            Console.WriteLine("====END OF SRTF====");

            return nonEmptyProcesses;
        }

        //highest-response-ratio-next algorithm - Wilo
        public List<PCB> hrrn(Queue<PCB> processes)
        {
            Console.WriteLine("-- Begin Highest Response Ratio Next");
            Console.WriteLine("\tNummber of processes in this round " + sample.Count);
            int counter = 0;
            List<PCB> nonEmptyProcesses = new List<PCB>();
            List<PCB> tempList = new List<PCB>();
            PCB process = new PCB();
          
            while (sample.Count !=0)
            {
                while (counter < sample.Peek().arrivalTime)
                    counter++;
                for (int i = 0; i < sample.Count; i++)
                {
                    if (sample.ElementAt(i).arrivalTime <= counter)
                    {
                        process = sample.Dequeue();
                        finishedProcesses.Add(process);
                        Console.WriteLine("Process " + process.PID + " has finished");
                    }
                    else
                    {

                    }
                }

            }
            
            //ration = (wait time + service time) / service time
            return null;
        }

        //round robin algorithm - Hannah
        public List<PCB> rr(Queue<PCB> processes, int quantum)
        {

            Console.WriteLine("--BEGIN ROUND ROBIN");
            Console.WriteLine("--Quantum: " + quantum);
            Console.WriteLine("\tNumber of processes to be serviced this round: " + processes.Count);

            int counter = 0;
            int serveTimeLeftOnProcess;
            int calcTimeSpentOnProcess;
            int beginAmount = processes.Count;
            int queueCounter = 1;
            PCB process = new PCB();
            List<PCB> nonEmptyProcesses = new List<PCB>();
            List<PCB> currProcesses = new List<PCB>();

            do
            {
                if (processes.Count != 0 && (currProcesses.Count == 0 || processes.Peek().arrivalTime < counter))
                {
                    while (counter < processes.Peek().arrivalTime)
                    {
                        counter++;
                    }
                    do
                    {
                        process = processes.Dequeue();
                        process.serviceTime = process.CPU.Dequeue();
                        process.beginServiceTime = process.serviceTime;
                        currProcesses.Add(process);
                        if (processes.Count == 0)
                        {
                            break;
                        }
                    } while (counter >= processes.Peek().arrivalTime);
                }

                if (debugStatements) Console.WriteLine("\t\tBeginning queue #" + queueCounter + " with " + currProcesses.Count + " processes...");
                for (int i = 0; i < currProcesses.Count; i++)
                {
                    counter += contextSwitchCost;
                    if (currProcesses[i].responseTime == -1)
                    {
                        currProcesses[i].responseTime = counter;
                    }
                    currProcesses[i].waitTime += ((currProcesses[i].lastTimeProcessed == 0) ? (counter - currProcesses[i].arrivalTime) : (counter - currProcesses[i].lastTimeProcessed));
                    serveTimeLeftOnProcess = currProcesses[i].serviceTime;
                    currProcesses[i].serviceTime = ((serveTimeLeftOnProcess - quantum) > 0) ? serveTimeLeftOnProcess - quantum : 0;
                    calcTimeSpentOnProcess = ((serveTimeLeftOnProcess - quantum) > 0) ? quantum : serveTimeLeftOnProcess;
                    counter += calcTimeSpentOnProcess;
                    currProcesses[i].lastTimeProcessed = counter;

                    if (currProcesses[i].serviceTime == 0)
                    {
                        if (currProcesses[i].IO.Count > 0)
                        {
                            nonEmptyProcesses.Add(currProcesses[i]);
                        }
                        else
                        {
                            currProcesses[i].finished = true;
                            currProcesses[i].executionTime = timeCounter + counter;
                            currProcesses[i].determineTurnaroundTime();
                            currProcesses[i].determineTRTS(currProcesses[i].beginServiceTime);
                            finishedProcesses.Add(currProcesses[i]);
                        }
                        currProcesses.RemoveAt(i);
                        i--;
                    }
                }
                if (debugStatements) Console.WriteLine("\t\t   Ending queue #" + queueCounter + " with " + currProcesses.Count + " processes left...");
                queueCounter++;

            } while ((processes.Count != 0 && currProcesses.Count == 0) || (processes.Count == 0 && currProcesses.Count != 0) || (processes.Count != 0 && currProcesses.Count != 0));

            timeCounter += counter;

            Console.WriteLine("\tTime spent in this round of RR: " + counter);
            Console.WriteLine("\tNumber of processes finished this round: " + (beginAmount - nonEmptyProcesses.Count));
            Console.WriteLine("\tAmount of processes left to process/finish: " + nonEmptyProcesses.Count);
            Console.WriteLine("\tAmount of processes \"done\" (no more bursts) so far: " + finishedProcesses.Count);
            Console.WriteLine("\tTotal time accumulated so far: " + timeCounter);
            Console.WriteLine("--END ROUND ROBIN\n");

            return nonEmptyProcesses;
        }

        //priority algorithm - Brady
        public List<PCB> priority(Queue<PCB> processes, bool CPUburst)
        {
            Console.WriteLine("Number of processes to be serviced: " + processes.Count());
            List<PCB> temp = new List<PCB>(processes);
            List<PCB> nonEmptyProcesses = new List<PCB>();
            int counter = 0;
            temp = temp.OrderBy(x => x.priorityNumber).ToList();
            for (int i = 0; i < processes.Count(); ++i)
            {
                var process = temp[i];
                process.serviceTime = process.CPU.Dequeue();
                counter += process.serviceTime;
                process.serviceTime -= process.serviceTime;
                if ((CPUburst && process.IO.Count > 0) || (!CPUburst && process.CPU.Count > 0))
                {
                    nonEmptyProcesses.Add(process);
                }
                else
                {
                    finishedProcesses.Add(process);
                }
            }
            timeCounter += counter;

            //lowest interger priority number has highest priority
            return nonEmptyProcesses;
        }

        //feedback algorithms if we wish to implement a feedback age solution

        //version 1 feedback with quantum = 1 
        public List<PCB> v1Feedback(Queue<PCB> processes, bool CPUburst)
        {
            Console.WriteLine("--BEGIN Feedback with q = 1");
            Console.WriteLine("\tNumber of processes to be serviced this round: " + processes.Count);
            Console.WriteLine("\tCurrent burst processing: " + ((CPUburst) ? "CPU" : "I/O"));

            int quantum = 1;
            var finished = false;   //when algorithm is complete
            var counter = 0;    //'timer' since we are modeling as discrete events
            var process = new PCB();    //temporary holder
            var startIndex = 0;     //start index of the ready queues
            var numProcesses = processes.Count;
            var localFinishedProcesses = 0;
            List<PCB> nonEmptyProcesses = new List<PCB>();

            //create a list of queues
            List<Queue<PCB>> rq = new List<Queue<PCB>>();
            for (int i = 0; i < 20; i++)
            {
                rq.Add(new Queue<PCB>());
            }
            //first process

            while (counter != processes.Peek().arrivalTime)
                counter++;
            process = processes.Dequeue();
            process.serviceTime = process.CPU.Dequeue();
            process.beginServiceTime = process.serviceTime;
            process.responseTime = counter;
            process.waitTime += counter;

            while (counter != processes.Peek().arrivalTime)
            {
                counter++;
                if (debugStatements)
                    Console.WriteLine("Process " + process.PID + " service time is " + process.serveTime(quantum) + " at time " + counter);
                else
                    process.serveTime(quantum);
            }
            //assuming first process will not finish before next process comes in - not realistic of a CPU
            rq[++startIndex].Enqueue(process);

            while (!finished)
            {
                process.stop = counter;
                //if a process has arrived, get the process and start our index of queues back at 0
                if (processes.Count != 0)
                {
                    if (counter == processes.Peek().arrivalTime)
                    {
                        process = processes.Dequeue();
                        process.serviceTime = process.CPU.Dequeue();
                        process.beginServiceTime = process.serviceTime;
                        process.responseTime = counter;
                        process.waitTime += counter;
                        startIndex = 0;
                        rq[startIndex].Enqueue(process);
                    }
                }
                //if inbetween queues are empty, move along until we get to next queue that has elements
                while (rq[startIndex].Count == 0)
                    startIndex++;
                //take the process of the current queue
                process = rq[startIndex].Dequeue();
                if (process.stop != 0)
                {
                    process.start = counter;
                    process.waitTime += (process.start - process.stop);
                }
                

                //this happens in one unit of time
                counter++;
                //the process serves a certain amount of time
                if (debugStatements)
                    Console.WriteLine("Process " + process.PID + " service time is " + process.serveTime(quantum) + " at time " + counter + " and wait time is " + process.waitTime);
                else
                    process.serveTime(quantum);
                if (process.serviceTime == 0)
                {
                    localFinishedProcesses++;
                    process.executionTime = counter;        //set the finished time of the process
                    process.determineTurnaroundTime();
                    process.determineTRTS(process.beginServiceTime);
                    //process.tr_ts = process.turnaroundTime / process.serviceTime;
                    if ((CPUburst && process.IO.Count > 0) || (!CPUburst && process.CPU.Count > 0)) // if we still have IO or CPU bursts to process...
                    {
                        nonEmptyProcesses.Add(process); // add it to the process list that still needs to further processed
                    }
                    else
                    {
                        finishedProcesses.Add(process); // add it to the list of "finished" processes (processes that don't have any more bursts)
                    }

                    if (localFinishedProcesses == numProcesses)
                    {
                        finished = true;
                        timeCounter += counter;
                    }
                    if(debugStatements) Console.WriteLine("Process " + process.PID + " finished at time " + process.executionTime);

                    continue;
                }
                //move to the next queue
                if ((startIndex + 1) == rq.Count)
                    rq.Add(new Queue<PCB>());
                
                rq[++startIndex].Enqueue(process);

                //if there are no processes in this queue, then we move to the next queue
                if (rq[--startIndex].Count == 0)
                    startIndex++;

                //incorporate the context switch cost when switching between processes
                contextSwitchCost++;
            }
            Console.WriteLine("\tTime spent in this round of v1Feedback: " + counter);
            Console.WriteLine("\tNumber of processes finished this round: " + (numProcesses - nonEmptyProcesses.Count));
            Console.WriteLine("\tAmount of processes left to finish/process: " + nonEmptyProcesses.Count);
            Console.WriteLine("\tAmount of processes \"done\" (no more bursts) so far: " + finishedProcesses.Count);
            Console.WriteLine("\tTotal time accumulated so far: " + timeCounter);
            Console.WriteLine("--END FEEDBACK WITH QUANTUM = 1\n");
            return nonEmptyProcesses;
        }

        //version 2 feedback with quantum = 2^i - Tommy
        public List<PCB> v2Feedback(Queue<PCB> processes, bool CPUburst)
        {
            Console.WriteLine("--BEGIN Feedback with quantum = 2^i");
            Console.WriteLine("\tNumber of processes to be serviced this round: " + processes.Count);
            Console.WriteLine("\tCurrent burst processing: " + ((CPUburst) ? "CPU" : "I/O"));

            var finished = false;
            var counter = 0;
            var process = new PCB();
            var startIndex = 0;
            var numProcesses = 0;
            var localFinishedProcesses = 0;
            List<PCB> nonEmptyProcesses = new List<PCB>();

            numProcesses = processes.Count;

            List<Queue<PCB>> rq = new List<Queue<PCB>>();
            for (int i = 0; i < 20; i++)
            {
                rq.Add(new Queue<PCB>());
            }

            while (counter != processes.Peek().arrivalTime)
                counter++;

            process = processes.Dequeue();
            process.serviceTime = process.CPU.Dequeue();
            process.beginServiceTime = process.serviceTime;
            process.responseTime = counter;
            process.waitTime += counter;

            while (counter != processes.Peek().arrivalTime)
            {
                counter++;
                //quantum is only (2^0)=1 for this case
                if (debugStatements)
                    Console.WriteLine("Process " + process.PID + " service time is " + process.serveTime(1.00));
                else
                    process.serveTime(1.00);
            }

            rq[++startIndex].Enqueue(process);

            while (!finished)
            {
                process.stop = counter;
                //if a process has arrived either on time or has been waiting, get the process and start our index of queues back at 0
                if (processes.Count != 0)
                {
                    if (counter >= processes.Peek().arrivalTime)
                    {
                        process = processes.Dequeue();
                        process.serviceTime = process.CPU.Dequeue();
                        process.beginServiceTime = process.serviceTime;
                        process.responseTime = counter;
                        process.waitTime += counter;
                        startIndex = 0;
                        rq[startIndex].Enqueue(process);

                    }
                }

                while (rq[startIndex].Count == 0)
                    startIndex++;

                process = rq[startIndex].Dequeue();
                if (process.stop != 0)
                {
                    process.start = counter;
                    process.waitTime += (process.start - process.stop);
                }
                //the process serves 2^1 amount of time
                var quantum = Math.Pow(2.00, (Double)startIndex);
                var processServeTime = Convert.ToInt32(process.serveTime(quantum));
                

                //must check if the process finished before the quantum amount
                if (processServeTime < 0)
                    counter += (processServeTime * (-1));
                else
                    counter += (int)quantum;    //otherwise we just set it to the total time
                if (debugStatements)
                    Console.WriteLine("Process " + process.PID + " service time is " + process.serveTime(quantum) + " at time " + counter + " and wait time is " + process.waitTime);
                else
                    process.serveTime(quantum);
                if (process.serviceTime <= 0)
                {
                    process.finished = true;
                    localFinishedProcesses++;
                    process.executionTime = counter;
                    process.turnaroundTime = process.executionTime - process.arrivalTime;
                    //process.tr_ts = process.turnaroundTime / process.serviceTime;
                    if ((CPUburst && process.IO.Count > 0) || (!CPUburst && process.CPU.Count > 0)) // if we still have IO or CPU bursts to process...
                    {
                        nonEmptyProcesses.Add(process); // add it to the process list that still needs to further processed
                    }
                    else
                    {
                        finishedProcesses.Add(process); // add it to the list of "finished" processes (processes that don't have any more bursts)
                    }
                    if (localFinishedProcesses == numProcesses)
                    {
                        finished = true;
                        timeCounter += counter;
                    }
                    if(debugStatements)Console.WriteLine("Process " + process.PID + " finished at time " + process.executionTime);
                    continue;
                }
                if ((startIndex + 1) == rq.Count)
                    rq.Add(new Queue<PCB>());
                rq[++startIndex].Enqueue(process);

                if (rq[--startIndex].Count == 0)
                    startIndex++;

                contextSwitchCost++;
            }
            Console.WriteLine("\tTime spent in this round of v2Feedback: " + counter);
            Console.WriteLine("\tNumber of processes finished this round: " + (numProcesses - nonEmptyProcesses.Count));
            Console.WriteLine("\tAmount of processes left to finish/process: " + nonEmptyProcesses.Count);
            Console.WriteLine("\tAmount of processes \"done\" (no more bursts) so far: " + finishedProcesses.Count);
            Console.WriteLine("\tTotal time accumulated so far: " + timeCounter);
            Console.WriteLine("--END FEEDBACK WITH QUANTUM = 2^I \n");
            return nonEmptyProcesses;
        }
    }
}
