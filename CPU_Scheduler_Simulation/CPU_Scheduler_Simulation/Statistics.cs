using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CPU_Scheduler_Simulation
{
    public class Statistics
    {
        public List<PCB> finishedProcesses = new List<PCB>();
        int avgResponseTime;
        int avgWaitTime;
        int avgTurnaroundTime;
        int avgExecutionTime;
        int avgTRTS;

        public Statistics(List<PCB> processes)
        {
            finishedProcesses = processes;
        }

        public void runStatistics()
        {
            Console.WriteLine("Avg response time: " + determineAverageResponseTime());
            Console.WriteLine("\tMin response time: " + determineMinResponseTime());
            Console.WriteLine("\tMax response time: " + determineMaxResponseTime());
            Console.WriteLine("Avg wait time: " + determineAverageWaitTime());
            Console.WriteLine("\tMin wait time: " + determineMinWaitTime());
            Console.WriteLine("\tMax wait time: " + determineMaxWaitTime());
            Console.WriteLine("Avg turnaround time: " + determineAverageTurnaroundTime());
            Console.WriteLine("\tMin turnaround time: " + determineMinTurnaroundTime());
            Console.WriteLine("\tMax turnaround time: " + determineMaxTurnaroundTime());
            Console.WriteLine("Avg execution time: " + determineAverageExecutionTime());
            Console.WriteLine("\tMin execution time: " + determineMinExecutionTime());
            Console.WriteLine("\tMax execution time: " + determineMaxExecutionTime());
            Console.WriteLine("Avg trts: " + determineAverageTRTS());

            avgResponseTime = determineAverageResponseTime();
            avgTurnaroundTime = determineAverageTurnaroundTime();
            avgExecutionTime = determineAverageExecutionTime();
            avgTRTS = determineAverageTRTS();
            avgWaitTime = determineAverageWaitTime();
            
        }

        public double determineMinResponseTime()
        {
            double min = finishedProcesses[0].responseTime;
            foreach (var p in finishedProcesses)
            {
                if (p.responseTime < min)
                {
                    min = p.responseTime;
                }
            }

            return min;
        }

        public double determineMaxResponseTime()
        {
            double max = 0;
            foreach (var p in finishedProcesses)
            {
                if (p.responseTime > max)
                {
                    max = p.responseTime;
                }
            }

            return max;
        }

        public double determineMinWaitTime()
        {
            double min = finishedProcesses[0].waitTime;
            foreach (var p in finishedProcesses)
            {
                if (p.waitTime < min)
                {
                    min = p.waitTime;
                }
            }

            return min;
        }

        public double determineMaxWaitTime()
        {
            double max = 0;
            foreach (var p in finishedProcesses)
            {
                if (p.waitTime > max)
                {
                    max = p.waitTime;
                }
            }

            return max;
        }

        public int determineAverageResponseTime()
        {
            int sum = 0;
            int average;

            foreach (var p in finishedProcesses)
            {
                sum += Convert.ToInt32(p.responseTime);
            }

            average = (sum > 0) ? (sum / finishedProcesses.Count) : 0;

            return average;

        }

        public int determineAverageWaitTime()
        {
            int sum = 0;
            int average;

            foreach (var p in finishedProcesses)
            {
                sum += Convert.ToInt32(p.waitTime);
            }

            average = (sum > 0) ? (sum / finishedProcesses.Count) : 0;

            return average;

        }

        public int determineAverageTurnaroundTime()
        {
            int sum = 0;
            int average;

            foreach (var p in finishedProcesses)
            {
                sum += Convert.ToInt32(p.turnaroundTime);
            }

            average = (sum > 0) ? (sum / finishedProcesses.Count) : 0;

            return average;
        }

        public double determineMinTurnaroundTime()
        {
            double min = finishedProcesses[0].turnaroundTime;
            foreach (var p in finishedProcesses)
            {
                if (p.turnaroundTime < min)
                {
                    min = p.turnaroundTime;
                }
            }

            return min;
        }

        public double determineMaxTurnaroundTime()
        {
            double max = 0;
            foreach (var p in finishedProcesses)
            {
                if (p.turnaroundTime > max)
                {
                    max = p.turnaroundTime;
                }
            }

            return max;
        }

        public int determineAverageExecutionTime()
        {
            int sum = 0;
            int average;

            foreach (var p in finishedProcesses)
            {
                sum += Convert.ToInt32(p.executionTime);
            }

            average = (sum > 0) ? (sum / finishedProcesses.Count) : 0;

            return average;
        }

        public double determineMinExecutionTime()
        {
            double min = finishedProcesses[0].executionTime;
            foreach (var p in finishedProcesses)
            {
                if (p.executionTime < min)
                {
                    min = p.executionTime;
                }
            }

            return min;
        }

        public double determineMaxExecutionTime()
        {
            double max = 0;
            foreach (var p in finishedProcesses)
            {
                if (p.executionTime > max)
                {
                    max = p.executionTime;
                }
            }

            return max;
        }

        public int determineAverageTRTS() {

            int sum = 0;
            int average;
            int countTRTS = 0;

            foreach (var p in finishedProcesses)
            {
                foreach (var t in p.tr_ts)
                {
                    sum += Convert.ToInt32(t);
                    countTRTS++;
                }
            }

            average = (sum > 0) ? sum / countTRTS : 0;

            return average;
        }

    }
}
