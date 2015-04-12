using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Scheduler_Simulation
{
    public class Data
    {
        // default constructor
        public Data() { }

        // create some sample data
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

        // more sample data
        public Queue<PCB> sample2 = new Queue<PCB>
        (
            new[] {
                new PCB(){PID = 1, arrivalTime = 0, serviceTime = 50},
                new PCB(){PID = 2, arrivalTime = 20, serviceTime = 20},
                new PCB(){PID = 3, arrivalTime = 40, serviceTime = 100},
                new PCB(){PID = 4, arrivalTime = 60, serviceTime = 60}
            }
        );

        // dictionary to hold algorithm names and IDs that identify their switch number
        public Dictionary<int, String> algorithms = new Dictionary<int, String>()
        {
            {0, "FIRST-COME-FIRST-SERVE"},
            {1, "SHORTEST-PROCESS-NEXT"},
            {2, "SHORTEST-REMAINING-TIME"},
            {3, "HIGHEST-RESPONSE-RATIO-NEXT"},
            {4, "ROUND ROBIN WITH"},
            {5, "PRIORITY"},
            {6, "FEEDBACK WITH QUANTUM = 1"},
            {7, "FEEDBACK WITH QUANTUM = 2^i"}
        };

        // output data string
        public String getStatisticsOutput(double avgResp, double minResp, double maxResp,
            double avgWait, double minWait, double maxWait,
            double avgTurn, double minTurn, double maxTurn,
            double avgExec, double minExec, double maxExec,
            double avgTRTS, double throughput, double speedup, double contextSwitch)
        {
            return "Average response time: " + avgResp + "\n"
                + "\tMinimum response time: " + minResp + "\n"
                + "\tMaximum response time: " + maxResp + "\n"
                + "Average wait time: " + avgWait + "\n"
                + "\tMinimum wait time: " + minWait + "\n"
                + "\tMaximum wait time: " + maxWait + "\n"
                + "Average turnaround time: " + avgTurn + "\n"
                + "\tMinimum turnaround time: " + minTurn + "\n"
                + "\tMaximum turnaround time: " + maxTurn + "\n"
                + "Average execution time: " + avgExec + "\n"
                + "\tMinimum execution time: " + minExec + "\n"
                + "\tMaximum execution time: " + maxExec + "\n"
                + "Ratio of turnaround time / service time: " + avgTRTS + "\n"
                + "Average throughput across all CPUs: "  + throughput + "\n"
                + "Average context switch time across all CPUs: " + contextSwitch + "\n"
                + "Speedup: " + speedup + "\n"
            ;
        }

        // output string that belongs to introduction of an algorithm
        public String introAlgString(String algName, int numProcesses, bool CPUburst)
        {
            return "--BEGIN " + algName +  "\n"
            + "\tNumber of processes to be serviced this round: " + numProcesses + "\n"
            + "\tCurrent burst processing: " + ((CPUburst) ? "CPU" : "I/O");
        }

        // output string that belogns to ending of an algorithm
        public String outroAlgString(String algName, int time, int throughput, int numProcessesLeft, int finishedProcesses, int totalTime)
        {
            return "\tTime spent in this round of " + algName + ": " + time + "\n"
                + "\tNumber of processes finished this round: " + throughput + "\n"
                + "\tAmount of processes left to finish/process: " + numProcessesLeft + "\n"
                + "\tAmount of processes \"done\" (no more bursts) so far: " + finishedProcesses + "\n"
                + "\tTotal time accumulated so far: " + totalTime + "\n"
                + "--END " + algName + "\n";
        }

        // output string specifically for round robin
        public String rrString(String algName, int quantum, int numProcesses)
        {
            return "--BEGIN " + algName + " " + quantum + "\n"
                + "\tNumber of processes to be serviced this round: " + numProcesses;
        }

        public String endSimOutput(Scheduler scheduler)
        {
            var str = "";
            for (int i = 0; i < scheduler.numCPUs; i++)
            {
                str += "Total time spent in CPU #" + (i + 1) + ": " + scheduler.cpus[i].algorithms.timeCounter;
                str += "\tProcessor Utilization: " + scheduler.cpus[i].utilization;
            }
            str += "---------------------------------------------------";
            str += "Total time spent across all CPUs: " + scheduler.calcTotalTime();
            return str;
        }
    }
}
