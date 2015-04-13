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
        public int timeCounter = 0;         // total time to process all of the processes
        public int timeIO = 0;              // time spent in IO 
        public int contextSwitchCost = 1;   // cost of switching between processes. assumption: cost is one per switch
        public int totalContextSwitch = 0;
        int counter = 0;                    // local time for an algorithm
        int numProcesses = 0;               // the number of processes in the beginning of an algorithm
        bool debugStatements = true;       // if debugging is needed
        public Data data = new Data();      // data object to use for string output
        PCB process = new PCB();            // temporary holder for a process
        public List<PCB> finishedProcesses = new List<PCB>();   // global list of finished processes for data processing
        List<PCB> nonEmptyProcesses = new List<PCB>();          // the list to return if there are still processes left to process
        public List<double> throughputList = new List<double>
            {
                0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0
            };    // throughput for each algorithm

        // default constructor
        public Algorithm() { }

        // first-come-first-serve algorithm - non-preemptive
        public List<PCB> fcfs(Queue<PCB> processes, bool CPUburst) // CPUburst is bool so we know to access the IO burst or CPU burst of the process
        {
            Console.WriteLine(data.introAlgString(data.algorithms[8], processes.Count, CPUburst));   // output a line at the beginning of the algorithm
            counter = 0;
            numProcesses = processes.Count;
            int service;                        // gives the initial service of a process
            nonEmptyProcesses.Clear();          // clear the list if there are any processes inside

            do
            {
                counter += contextSwitchCost;   // when switching processes, we have to account for a small cost in time for context switching
                totalContextSwitch += contextSwitchCost;
                // get the time if the first process' arrival time does not start at 0
                while (counter < processes.Peek().arrivalTime)
                    counter++;

                process = processes.Dequeue();                                          // dequeue from the list of processes
                service = (CPUburst) ? process.CPU.Dequeue() : process.IO.Dequeue();    // depends on whether we are in CPU or IO burst phase
                    
                // check if response time is not already calculated
                if (process.responseTime == -1)
                    process.responseTime = counter;

                process.waitTime += counter - process.arrivalTime;                      // time when process is not active
                counter += service;                                                     // time spent of process gets added to local time

                // if there are any IO or CPU service times left to compute
                if ((CPUburst && process.IO.Count > 0) || (!CPUburst && process.CPU.Count > 0))
                    nonEmptyProcesses.Add(process);
                else
                {
                    process.finished = true;
                    process.executionTime = timeCounter + counter;      // set the final time of process by the total time plus the local time
                    process.determineTurnaroundTime();
                    process.determineTRTS(service);
                    finishedProcesses.Add(process);                     //process is finished, so add to the queue
                }
            } while (processes.Count != 0);

            timeCounter += counter;             // add local time to total time
            if (CPUburst) timeIO += counter;    // if this was in IO phase, we need to calculate the time processor spent in IO

            var throughput = numProcesses - nonEmptyProcesses.Count;
            throughputList[8] += throughput;
            // output ending of algorithm
            Console.WriteLine(data.outroAlgString(data.algorithms[8], counter, throughput, nonEmptyProcesses.Count, finishedProcesses.Count, timeCounter));

            // return the processes that need to be computed
            return nonEmptyProcesses;
        }

        // shortest-process-next algorithm - non-preemptive
        public List<PCB> spn(Queue<PCB> processes, bool CPUburst)
        {
            Console.WriteLine(data.introAlgString(data.algorithms[0], processes.Count, CPUburst));
            nonEmptyProcesses.Clear();
            var list = processes.ToList();  // convert queue to list
            counter = 0;
            numProcesses = processes.Count;
            Collection<PCB> collection = new Collection<PCB>(list);     // create collection from converted list
            List<PCB> temp = new List<PCB>();                           // temporary list that holds the processes so it is easier to query the minimum service time

            while (counter < processes.Peek().arrivalTime)
                counter++;

            while (collection.Count != 0)
            {
                // add the processes that have arrived or have arrived for some time
                for (int i = 0; i < collection.Count; i++)
                {
                    if (collection[i].arrivalTime <= counter)
                        temp.Add(collection[i]);
                }

                var min = temp.Min(x => x.serviceTime);             // calculate the minimum
                process = temp.First(x => x.serviceTime == min);    // choose the process that has the minimum
                process.serviceTime = process.CPU.Dequeue();
                process.beginServiceTime = process.serviceTime;
                if (process.responseTime == -1)
                    process.responseTime = counter;
                counter += process.serviceTime;
                counter += contextSwitchCost;
                totalContextSwitch += contextSwitchCost;
                process.serviceTime -= process.serviceTime;
                process.waitTime = counter - process.arrivalTime;
                // Console.WriteLine("Process " + process.PID + " has finished");
                if ((CPUburst && process.IO.Count > 0) || (!CPUburst && process.CPU.Count > 0))
                    nonEmptyProcesses.Add(process);
                else
                {
                    process.executionTime = counter + timeCounter;
                    process.determineTurnaroundTime();
                    process.determineTRTS(process.beginServiceTime);
                    finishedProcesses.Add(process);
                }
                collection.Remove(process);     // remove the process we have already computed
                temp.Clear();
            }
            timeCounter += counter;
            var throughput = numProcesses - nonEmptyProcesses.Count;
            throughputList[0] += throughput;
            Console.WriteLine(data.outroAlgString(data.algorithms[0], counter, throughput, nonEmptyProcesses.Count, finishedProcesses.Count, timeCounter));
            return nonEmptyProcesses;
        }

        // shortest-remaining-time algorithm - preemptive
        public List<PCB> srt(Queue<PCB> processes, bool CPUburst)
        {
            Console.WriteLine(data.introAlgString(data.algorithms[1], processes.Count, CPUburst));
            counter = 0;
            numProcesses = processes.Count;
            var List = processes.ToList();
            Collection<PCB> collection = new Collection<PCB>(List);
            nonEmptyProcesses.Clear();
            List<PCB> tempList = new List<PCB>();

            while (counter < processes.Peek().arrivalTime)
                counter++;
            process = collection[0];
            if (process.responseTime == -1)
            {
                process.responseTime = counter;
            }
            process.waitTime += counter;
            counter++;
            process.serviceTime = process.CPU.Dequeue();
            process.beginServiceTime = process.serviceTime;
            process.serviceTime--;

            while (collection.Count != 0)
            {
                process.stop = counter;      // process has stopped computing

                for (int i = 0; i < collection.Count; ++i)
                {
                    if (collection.ElementAt(i).arrivalTime <= counter)
                        tempList.Add(collection.ElementAt(i));
                }

                var min = tempList.Min(p => p.serviceTime);
                var tempProcess = tempList.First(p => p.serviceTime == min);

                // compare the queried process with the current process
                if (process.serviceTime >= tempProcess.serviceTime)
                {
                    process.alreadyProcessed = true;
                    counter += contextSwitchCost;
                    totalContextSwitch += contextSwitchCost;
                    process = tempProcess;
                    
                    // if the process has arrived again, we need to check so we do not dequeue all of its cpu bursts
                    if (!tempProcess.alreadyProcessed)
                    {
                        tempProcess.alreadyProcessed = true;
                        process.serviceTime = process.CPU.Dequeue();
                        process.beginServiceTime = process.serviceTime;
                    }
                }

                if (process.responseTime == -1)
                    process.responseTime = counter;

                // if the process has processed, we can begin to calculate the wait time
                if (process.stop != 0)
                {
                    process.start = counter;
                    process.waitTime += (process.start - process.stop);
                }
                counter++;
                process.serviceTime--;

                if (process.serviceTime == 0)
                {
                    // Console.WriteLine("Process " + process.PID + " has finished");
                    if ((CPUburst && process.IO.Count > 0) || (!CPUburst && process.CPU.Count > 0))
                    {
                        nonEmptyProcesses.Add(process);
                    }
                    else
                    {
                        process.executionTime = timeCounter + counter;
                        process.determineTurnaroundTime();
                        process.determineTRTS(process.beginServiceTime);
                        finishedProcesses.Add(process);
                    }
                    collection.Remove(process);

                    if (collection.Count != 0)
                    {
                        counter += contextSwitchCost;
                        totalContextSwitch += contextSwitchCost;
                        process = collection[0];

                        if (!tempProcess.alreadyProcessed)
                        {
                            process.alreadyProcessed = true;
                            tempProcess.alreadyProcessed = true;
                            process.serviceTime = process.CPU.Dequeue();
                            process.beginServiceTime = process.serviceTime;
                        }
                    }
                }
                tempList.Clear();   // clear the list so we can use it again
            }
            timeCounter += counter;
            var throughput = numProcesses - nonEmptyProcesses.Count;
            throughputList[1] += throughput;
            Console.WriteLine(data.outroAlgString(data.algorithms[1], counter, throughput, nonEmptyProcesses.Count, finishedProcesses.Count, timeCounter));
            return nonEmptyProcesses;
        }

        // highest-response-ratio-next algorithm - non-preemptive
        public List<PCB> hrrn(Queue<PCB> processes, bool CPUburst)
        {
            Console.WriteLine(data.introAlgString(data.algorithms[2], processes.Count, CPUburst));
            nonEmptyProcesses.Clear();
            var list = processes.ToList();
            counter = 0;
            numProcesses = processes.Count;
            Collection<PCB> collection = new Collection<PCB>(list);
            List<PCB> temp = new List<PCB>();

            while (counter < processes.Peek().arrivalTime)
                counter++;

            // since this is a nonpreemptive approach, we can just assign all of the processes' service time
            foreach (var p in collection)
            {
                p.serviceTime = p.CPU.Dequeue();
                p.beginServiceTime = p.serviceTime;
            }

            while (collection.Count != 0)
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    if (collection[i].arrivalTime <= counter)
                        temp.Add(collection[i]);
                }

                // compute the ratios on each round of processes
                foreach (var p in temp)
                {
                    p.waitTime = counter - p.arrivalTime;
                    p.computeRatio();
                }

                var highest = temp.Max(x => x.ratio);               // get the highest ratio
                process = temp.First(x => x.ratio == highest);      // choose the process with the highest ratio

                if (process.responseTime == -1)
                    process.responseTime = counter;
                counter += process.serviceTime;
                counter += contextSwitchCost;
                totalContextSwitch += contextSwitchCost;
                process.serviceTime -= process.serviceTime;
                // Console.WriteLine("Process " + process.PID + " has finished");
                if ((CPUburst && process.IO.Count > 0) || (!CPUburst && process.CPU.Count > 0))
                    nonEmptyProcesses.Add(process);
                else
                {
                    process.executionTime = timeCounter + counter;
                    process.determineTurnaroundTime();
                    process.determineTRTS(process.beginServiceTime);
                    finishedProcesses.Add(process);
                }
                collection.Remove(process);
                temp.Clear();
            }
            timeCounter += counter;
            var throughput = numProcesses - nonEmptyProcesses.Count;
            throughputList[2] += throughput;
            Console.WriteLine(data.outroAlgString(data.algorithms[2], counter, throughput, nonEmptyProcesses.Count, finishedProcesses.Count, timeCounter));
            return nonEmptyProcesses;
        }

        // round robin algorithm - preemptive
        public List<PCB> rr(Queue<PCB> processes, int quantum, int index)
        {
            Console.WriteLine(data.rrString(data.algorithms[3], quantum, processes.Count));
            counter = 0;
            int serveTimeLeftOnProcess;
            int calcTimeSpentOnProcess;
            numProcesses = processes.Count;
            int queueCounter = 1;
            nonEmptyProcesses.Clear();
            List<PCB> currProcesses = new List<PCB>();  // so we can differentiate between IO and CPU lists

            do
            {
                if (processes.Count != 0 && (currProcesses.Count == 0 || processes.Peek().arrivalTime < counter))
                {
                    while (counter < processes.Peek().arrivalTime)
                        counter++;
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
                    totalContextSwitch += contextSwitchCost;
                    if (currProcesses[i].responseTime == -1)
                        currProcesses[i].responseTime = counter;
                    currProcesses[i].waitTime += ((currProcesses[i].lastTimeProcessed == 0) ? (counter - currProcesses[i].arrivalTime) : (counter - currProcesses[i].lastTimeProcessed));
                    serveTimeLeftOnProcess = currProcesses[i].serviceTime;
                    currProcesses[i].serviceTime = ((serveTimeLeftOnProcess - quantum) > 0) ? serveTimeLeftOnProcess - quantum : 0;
                    calcTimeSpentOnProcess = ((serveTimeLeftOnProcess - quantum) > 0) ? quantum : serveTimeLeftOnProcess;
                    counter += calcTimeSpentOnProcess;
                    currProcesses[i].lastTimeProcessed = counter;

                    if (currProcesses[i].serviceTime == 0)
                    {
                        if (currProcesses[i].IO.Count > 0)
                            nonEmptyProcesses.Add(currProcesses[i]);
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
            var throughput = numProcesses - nonEmptyProcesses.Count;
            if (index == 4) throughputList[3] += throughput;        // if we are in the first version of round robin
            else throughputList[4] += throughput;                   // if we are in the second version of round robin
            Console.WriteLine(data.outroAlgString(data.algorithms[3], counter, throughput, nonEmptyProcesses.Count, finishedProcesses.Count, timeCounter));

            return nonEmptyProcesses;
        }

        // priority algorithm - non-preemptive - aging solution
        public List<PCB> priority(Queue<PCB> processes, bool CPUburst)
        {
            Console.WriteLine(data.introAlgString(data.algorithms[5], processes.Count, CPUburst));
            List<PCB> temp = new List<PCB>();
            var list = processes.ToList();
            Collection<PCB> collection = new Collection<PCB>(list);
            nonEmptyProcesses.Clear();
            counter = 0;
            numProcesses = processes.Count;

            //temp = temp.OrderBy(x => x.priorityNumber).ToList();    // we only care about the priority number of the list

            while (counter < processes.Peek().arrivalTime)
                counter++;

            while (collection.Count != 0)
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    if (collection[i].arrivalTime <= counter)
                        temp.Add(collection[i]);
                }

                // get the lowest priority from the query
                var lowest = temp.Min(x => x.priorityNumber);
                process = temp.First(x => x.priorityNumber == lowest);
                process.serviceTime = process.CPU.Dequeue();
                process.beginServiceTime = process.serviceTime;
                if (process.responseTime == -1)
                    process.responseTime = counter;
                counter += process.serviceTime;
                counter += contextSwitchCost;
                totalContextSwitch += contextSwitchCost;
                process.waitTime = counter - process.arrivalTime;
                process.serviceTime -= process.serviceTime;
                if ((CPUburst && process.IO.Count > 0) || (!CPUburst && process.CPU.Count > 0))
                    nonEmptyProcesses.Add(process);
                else
                {
                    process.executionTime = timeCounter + counter;
                    process.determineTurnaroundTime();
                    process.determineTRTS(process.beginServiceTime);
                    finishedProcesses.Add(process);
                }
                collection.Remove(process);
                // in helping with starvation, we just lower the priority number so it comes up faster
                foreach (var p in collection)
                {
                    if (p.priorityNumber - 5 <= 0)
                        p.priorityNumber = 0;
                    else
                        p.priorityNumber -= 5;
                }
                temp.Clear();
            }
            timeCounter += counter;
            var throughput = numProcesses - nonEmptyProcesses.Count;
            throughputList[5] += throughput;
            Console.WriteLine(data.outroAlgString(data.algorithms[5], counter, throughput, nonEmptyProcesses.Count, finishedProcesses.Count, timeCounter));
            return nonEmptyProcesses;
        }

        // version 1 feedback with quantum = 1 - preemptive
        public List<PCB> v1Feedback(Queue<PCB> processes, bool CPUburst)
        {
            //Queue<PCB> processes = data.sample;
            Console.WriteLine(data.introAlgString(data.algorithms[6], processes.Count, CPUburst));
            
            int quantum = 1;
            var finished = false;           // flag to tell when algorithm is complete
            counter = 0;            
            var startIndex = 0;             // start index of the ready queues
            numProcesses = processes.Count;
            var localFinishedProcesses = 0; // to help us decide whether or not the algorithm is finished
            nonEmptyProcesses.Clear();

            //create a list of queues
            List<Queue<PCB>> rq = new List<Queue<PCB>>();
            for (int i = 0; i < 1000; i++)
            {
                rq.Add(new Queue<PCB>());
            }

            //first process
            while (counter != processes.Peek().arrivalTime)
                counter++;
            process = processes.Dequeue();
            process.serviceTime = process.CPU.Dequeue();
            process.beginServiceTime = process.serviceTime;
            if (process.responseTime == -1)
                process.responseTime = counter;
            process.waitTime += counter;

            while (counter != processes.Peek().arrivalTime)
            {
                counter++;
                if (debugStatements)
                    Console.WriteLine("Process " + process.name + " service time is " + process.serveTime(quantum) + " at time " + counter);
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
                    if (counter >= processes.Peek().arrivalTime)
                    {
                        process = processes.Dequeue();
                        process.serviceTime = process.CPU.Dequeue();
                        process.beginServiceTime = process.serviceTime;
                        if (process.responseTime == -1)
                        {
                            process.responseTime = counter;
                        }
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
                counter++;

                if (debugStatements)
                    Console.WriteLine("Process " + process.name + " service time is " + process.serveTime(quantum) + " at time " + counter + " and wait time is " + process.waitTime);
                else
                    process.serveTime(quantum);

                if (process.serviceTime == 0)
                {
                    localFinishedProcesses++;       // count the number of processes that have finished
                    if ((CPUburst && process.IO.Count > 0) || (!CPUburst && process.CPU.Count > 0)) // if we still have IO or CPU bursts to process...
                        nonEmptyProcesses.Add(process); // add it to the process list that still needs to further processed
                    else
                    {
                        process.executionTime = timeCounter + counter;
                        process.determineTurnaroundTime();
                        process.determineTRTS(process.beginServiceTime);
                        finishedProcesses.Add(process); // add it to the list of "finished" processes (processes that don't have any more bursts)
                    }

                    // we have finished once all the processes have finished
                    if (localFinishedProcesses == numProcesses)
                        finished = true;

                    if (debugStatements) Console.WriteLine("Process " + process.name + " finished at time " + process.executionTime);

                    continue;
                }

                //move to the next queue
                if ((startIndex + 1) == rq.Count)
                    rq.Add(new Queue<PCB>());

                rq[startIndex+1].Enqueue(process);

                //if there are no processes in this queue, then we move to the next queue
                if (rq[startIndex].Count == 0)
                    startIndex++;

                //incorporate the context switch cost when switching between processes
                counter += contextSwitchCost;
                totalContextSwitch += contextSwitchCost;
            }
            timeCounter += counter;
            var throughput = numProcesses - nonEmptyProcesses.Count;
            throughputList[6] += throughput;
            Console.WriteLine(data.outroAlgString(data.algorithms[6], counter, throughput, nonEmptyProcesses.Count, finishedProcesses.Count, timeCounter));
            return nonEmptyProcesses;
        }

        //version 2 feedback with quantum = 2^i - aging solution
        public List<PCB> v2Feedback(Queue<PCB> processes, bool CPUburst)
        {
            Console.WriteLine(data.introAlgString(data.algorithms[7], processes.Count, CPUburst));
            var finished = false;
            counter = 0;
            var startIndex = 0;
            numProcesses = processes.Count;
            var localFinishedProcesses = 0;
            nonEmptyProcesses.Clear();

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
            if (process.responseTime == -1)
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
                        if (process.responseTime == -1)
                        {
                            process.responseTime = counter;
                        }
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

                    if ((CPUburst && process.IO.Count > 0) || (!CPUburst && process.CPU.Count > 0)) // if we still have IO or CPU bursts to process...
                        nonEmptyProcesses.Add(process); // add it to the process list that still needs to further processed
                    else
                    {
                        process.executionTime = timeCounter + counter;
                        process.determineTurnaroundTime();
                        process.determineTRTS(process.beginServiceTime);
                        finishedProcesses.Add(process); // add it to the list of "finished" processes (processes that don't have any more bursts)
                    }

                    if (localFinishedProcesses == numProcesses)
                        finished = true;

                    if (debugStatements) Console.WriteLine("Process " + process.PID + " finished at time " + process.executionTime);
                    continue;
                }

                if ((startIndex + 1) == rq.Count)
                    rq.Add(new Queue<PCB>());

                rq[++startIndex].Enqueue(process);

                if (rq[--startIndex].Count == 0)
                    startIndex++;

                counter += contextSwitchCost;
                totalContextSwitch += contextSwitchCost;
            }
            timeCounter += counter;
            var throughput = numProcesses - nonEmptyProcesses.Count;
            throughputList[7] += throughput;
            Console.WriteLine(data.outroAlgString(data.algorithms[7], counter, throughput, nonEmptyProcesses.Count, finishedProcesses.Count, timeCounter));
            return nonEmptyProcesses;
        }
    }
}
