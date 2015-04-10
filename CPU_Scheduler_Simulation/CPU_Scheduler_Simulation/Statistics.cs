using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Scheduler_Simulation
{
    public class Statistics
    {
        public List<PCB> finishedProcesses = new List<PCB>();

        public Statistics(List<PCB> processes)
        {
            finishedProcesses = processes;
        }

        public void runStatistics()
        {
            Console.WriteLine("Avg response time: " + determineAverageResponseTime());
            Console.WriteLine("\tMin response time: " + determineMinResponseTime());
            Console.WriteLine("\tMin response time: " + determineMaxResponseTime());
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

        public double determineAverageResponseTime()
        {
            double sum = 0;
            double average;

            foreach (var p in finishedProcesses)
            {
                sum += p.responseTime;
            }

            average = (sum > 0) ? (sum / finishedProcesses.Count) : 0;

            return average;

        }

        public double determineAverageWaitTime()
        {
            double sum = 0;
            double average;

            foreach (var p in finishedProcesses)
            {
                sum += p.waitTime;
            }

            average = (sum > 0) ? (sum / finishedProcesses.Count) : 0;

            return average;

        }

        public double determineAverageTurnaroundTime()
        {
            double sum = 0;
            double average;

            foreach (var p in finishedProcesses)
            {
                sum += p.turnaroundTime;
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

        public double determineAverageExecutionTime()
        {
            double sum = 0;
            double average;

            foreach (var p in finishedProcesses)
            {
                sum += p.executionTime;
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

        public double determineAverageTRTS() {

            double sum = 0;
            double average;
            double countTRTS = 0;

            foreach (var p in finishedProcesses)
            {
                foreach (var t in p.tr_ts)
                {
                    sum += t;
                    countTRTS++;
                }
            }

            average = (sum > 0) ? sum / countTRTS : 0;

            return average;
        }

    }
}
