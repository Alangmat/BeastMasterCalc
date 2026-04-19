using System;
using System.Threading;
using System.Threading.Tasks;

namespace ViewModel
{
    public class RecommendationSystem
    {
        public Task<double[]> RunDEAsync(
            double[] initial,
            double[] weights,
            double[] baseStats,
            double[] mins,
            double[] maxs,
            double targetBudget,
            Func<double[], int> evaluate,
            Action<string> reportStatus,
            int populationSize = 100,
            int maxGenerations = 200,
            double mutationFactor = 0.8,
            double crossoverRate = 0.9,
            double budgetTolerance = 0.2,
            CancellationToken ct = default)
        {
            return Task.Run(() => RunDE(
                initial, weights, baseStats, mins, maxs, targetBudget,
                evaluate, reportStatus,
                populationSize, maxGenerations, mutationFactor, crossoverRate, budgetTolerance, ct), ct);
        }

        private double[] RunDE(
            double[] initial,
            double[] weights,
            double[] baseStats,
            double[] mins,
            double[] maxs,
            double targetBudget,
            Func<double[], int> evaluate,
            Action<string> reportStatus,
            int populationSize,
            int maxGenerations,
            double F,
            double CR,
            double budgetTolerance,
            CancellationToken ct)
        {
            int n = initial.Length;
            var rand = new Random();

            double Clamp(double val, double min, double max) => Math.Max(min, Math.Min(max, val));
            double Round01(double val) => Math.Round(val * 10) / 10.0;

            double ComputeBudget(double[] x)
            {
                double sum = 0;
                for (int i = 0; i < n; i++)
                    sum += weights[i] * (x[i] - baseStats[i]);
                return sum;
            }

            void ProjectToBudget(double[] x, double target)
            {
                double lo = -100, hi = 100;
                for (int iter = 0; iter < 30; iter++)
                {
                    double lam = (lo + hi) / 2.0;
                    double[] xp = new double[n];
                    for (int i = 0; i < n; i++)
                    {
                        double vp = Clamp(
                            x[i] - baseStats[i] - lam * weights[i],
                            mins[i] - baseStats[i],
                            maxs[i] - baseStats[i]);
                        xp[i] = baseStats[i] + vp;
                    }
                    double b = ComputeBudget(xp);
                    if (Math.Abs(b - target) < 0.01) { Array.Copy(xp, x, n); return; }
                    if (b > target) lo = lam; else hi = lam;
                }
                double lam2 = (lo + hi) / 2.0;
                for (int i = 0; i < n; i++)
                {
                    double vp = Clamp(
                        x[i] - baseStats[i] - lam2 * weights[i],
                        mins[i] - baseStats[i],
                        maxs[i] - baseStats[i]);
                    x[i] = baseStats[i] + vp;
                }
            }

            double Fitness(double[] x)
            {
                double diff = Math.Abs(ComputeBudget(x) - targetBudget);
                double penalty = diff > budgetTolerance ? diff * 100000 : 0;
                return evaluate(x) - penalty;
            }

            // Инициализация популяции
            double[][] pop = new double[populationSize][];
            double[] fit = new double[populationSize];

            for (int i = 0; i < populationSize; i++)
            {
                ct.ThrowIfCancellationRequested();
                pop[i] = new double[n];
                if (i == 0)
                    Array.Copy(initial, pop[i], n);
                else
                    for (int j = 0; j < n; j++)
                        pop[i][j] = mins[j] + rand.NextDouble() * (maxs[j] - mins[j]);
                ProjectToBudget(pop[i], targetBudget);
                fit[i] = Fitness(pop[i]);
            }

            int bestIdx = 0;
            for (int i = 1; i < populationSize; i++)
                if (fit[i] > fit[bestIdx]) bestIdx = i;

            // Эволюция
            for (int gen = 0; gen < maxGenerations; gen++)
            {
                ct.ThrowIfCancellationRequested();
                for (int i = 0; i < populationSize; i++)
                {
                    int a, b, c;
                    do { a = rand.Next(populationSize); } while (a == i);
                    do { b = rand.Next(populationSize); } while (b == i || b == a);
                    do { c = rand.Next(populationSize); } while (c == i || c == a || c == b);

                    double[] mutant = new double[n];
                    for (int j = 0; j < n; j++)
                        mutant[j] = Clamp(pop[a][j] + F * (pop[b][j] - pop[c][j]), mins[j], maxs[j]);
                    ProjectToBudget(mutant, targetBudget);

                    int jRand = rand.Next(n);
                    double[] trial = new double[n];
                    for (int j = 0; j < n; j++)
                        trial[j] = rand.NextDouble() < CR || j == jRand ? mutant[j] : pop[i][j];
                    ProjectToBudget(trial, targetBudget);

                    double trialFit = Fitness(trial);
                    if (trialFit > fit[i])
                    {
                        pop[i] = trial;
                        fit[i] = trialFit;
                        if (trialFit > fit[bestIdx])
                        {
                            bestIdx = i;
                            reportStatus?.Invoke($"DE: gen={gen}, DD={(int)trialFit}");
                        }
                    }
                }
            }

            // Финализация
            double[] best = new double[n];
            Array.Copy(pop[bestIdx], best, n);
            for (int i = 0; i < n; i++) best[i] = Round01(best[i]);
            ProjectToBudget(best, targetBudget);
            for (int i = 0; i < n; i++) best[i] = Clamp(Round01(best[i]), mins[i], maxs[i]);

            return best;
        }
    }
}
