using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ViewModel
{
    public enum RecommendationAlgorithm
    {
        DE,
        MCTS
    }

    public class RecommendationInput
    {
        public RecommendationAlgorithm Algorithm = RecommendationAlgorithm.DE;
        public int DePopulationSize = RecommendationSystem.DefaultDePopulationSize;
        public int DeMaxGenerations = RecommendationSystem.DefaultDeMaxGenerations;
        public double DeMutationFactor = RecommendationSystem.DefaultDeMutationFactor;
        public double DeCrossoverRate = RecommendationSystem.DefaultDeCrossoverRate;
        public int MctsMaxIterations = RecommendationSystem.DefaultMctsMaxIterations;
        public int MctsTop = RecommendationSystem.DefaultMctsTop;
        public int MctsMinSteps = RecommendationSystem.DefaultMctsMinSteps;
        public int MctsMaxSteps = RecommendationSystem.DefaultMctsMaxSteps;
        public double[] Initial;
        public double[] Weights;
        public double[] BaseStats;
        public double[] Mins;
        public double[] Maxs;
        public double TargetBudget;
        public double BudgetTolerance = 0.2;
        public Func<double[], int> Evaluate;
        public Action<string> ReportStatus;
        public CancellationToken CancellationToken;
        public long EvalCallCount = 0;
    }

    public class RecommendationSystem
    {
        public const int DefaultDePopulationSize = 100;
        public const int DefaultDeMaxGenerations = 200;
        public const double DefaultDeMutationFactor = 0.8;
        public const double DefaultDeCrossoverRate = 0.9;
        public const int DefaultMctsMaxIterations = 16150;
        public const int DefaultMctsTop = 20;
        public const int DefaultMctsMinSteps = 5;
        public const int DefaultMctsMaxSteps = 15;

        // ── Выбор алгоритма (хардкод) ─────────────────────────────────────────
        // Альтернативы:
        //private Task<double[]> RunSelected(RecommendationInput inp) => RunBruteForceAsync(inp);
        //private Task<double[]> RunSelected(RecommendationInput inp) => RunMCTSAsync(inp);
        // ──────────────────────────────────────────────────────────────────────

        public Task<double[]> GetRecommendationAsync(RecommendationInput inp)
        {
            switch (inp.Algorithm)
            {
                case RecommendationAlgorithm.MCTS:
                    return RunMCTSAsync(inp);
                case RecommendationAlgorithm.DE:
                default:
                    return RunDEAsync(inp);
            }
        }

        public static string GetAlgorithmName(RecommendationAlgorithm algorithm)
            => algorithm == RecommendationAlgorithm.MCTS ? "Monte Carlo Tree Search" : "Differential Evolution";

        public static string GetAlgorithmCode(RecommendationAlgorithm algorithm)
            => algorithm == RecommendationAlgorithm.MCTS ? "mcts" : "de";

        // =====================================================================
        // АЛГОРИТМ 1: Полный перебор (branch-and-bound по бюджету)
        //
        // Стат с наибольшим весом вычисляется из бюджета аналитически.
        // Остальные n-1 параметров перебираются рекурсивно с шагом BfStep.
        // Отсечение: если оставшийся бюджет < 0 (перерасход) — break;
        //            если оставшийся бюджет > max достижимого далее — continue.
        // Это делает перебор ИСТИННО ПОЛНЫМ при данном шаге.
        // =====================================================================
        private const double BfStep = 0.1; // шаг перебора (уменьшить = точнее, но дольше)

        private Task<double[]> RunBruteForceAsync(RecommendationInput inp)
            => Task.Run(() => RunBruteForce(inp), inp.CancellationToken);

        private double[] RunBruteForce(RecommendationInput inp)
        {
            int n = inp.Initial.Length;

            int[] order = Enumerable.Range(0, n)
                .OrderByDescending(i => inp.Weights[i])
                .ToArray();

            double[] maxFrom = new double[n + 1];
            for (int k = n - 1; k >= 0; k--)
            {
                int idx = order[k];
                maxFrom[k] = inp.Weights[idx] * (inp.Maxs[idx] - inp.BaseStats[idx])
                           + maxFrom[k + 1];
            }

            double[] candidate = (double[])inp.Mins.Clone();
            double[] best = (double[])inp.Initial.Clone();

            int bestScore = int.MinValue;
            long evalCount = 0;

            void Search(int k, double used)
            {
                if (k == n)
                {
                    double remain = inp.TargetBudget - used;

                    if (Math.Abs(remain) > inp.BudgetTolerance)
                        return;

                    evalCount++;

                    int score = inp.Evaluate(candidate);

                    if (score > bestScore)
                    {
                        bestScore = score;
                        Array.Copy(candidate, best, n);

                        inp.ReportStatus?.Invoke(
                            $"BruteForce: {evalCount:N0} комбинаций, DD={score}");
                    }

                    return;
                }

                int idx = order[k];

                double lo = inp.Mins[idx];
                double hi = inp.Maxs[idx];

                int steps = (int)Math.Ceiling((hi - lo) / BfStep);

                for (int j = 0; j <= steps; j++)
                {
                    inp.CancellationToken.ThrowIfCancellationRequested();

                    double val = Math.Min(
                        hi,
                        Math.Round((lo + j * BfStep) * 10) / 10.0
                    );

                    double newUsed = used + inp.Weights[idx] * (val - inp.BaseStats[idx]);
                    double remain = inp.TargetBudget - newUsed;

                    if (remain < -inp.BudgetTolerance)
                        break;

                    if (remain > maxFrom[k + 1] + inp.BudgetTolerance)
                        continue;

                    candidate[idx] = val;
                    Search(k + 1, newUsed);
                }
            }

            Search(0, 0.0);

            Finalize(best, inp);
            return best;
        }

        // =====================================================================
        // АЛГОРИТМ 2: Monte Carlo по дереву (MCTS, упрощённый)
        // =====================================================================
        private Task<double[]> RunMCTSAsync(RecommendationInput inp)
            => Task.Run(() => RunMCTS(inp), inp.CancellationToken);

        private double[] RunMCTS(RecommendationInput inp)
        {
            int n = inp.Initial.Length;
            int maxIterations = Math.Max(1, inp.MctsMaxIterations);
            int top = Math.Max(1, inp.MctsTop);
            int minSteps = Math.Max(1, inp.MctsMinSteps);
            int maxSteps = Math.Max(minSteps + 1, inp.MctsMaxSteps);
            double[] stepDeltas = { -5.0, -2.0, -1.0, -0.5, 0.5, 1.0, 2.0, 5.0 };
            int actionsCount = n * stepDeltas.Length;

            int[] actionStat  = new int[actionsCount];
            double[] actionDelta = new double[actionsCount];
            int idx = 0;
            for (int i = 0; i < n; i++)
                for (int d = 0; d < stepDeltas.Length; d++)
                {
                    actionStat[idx]  = i;
                    actionDelta[idx] = stepDeltas[d];
                    idx++;
                }

            double[] ApplyAction(double[] state, int a)
            {
                double[] ns = (double[])state.Clone();
                int si = actionStat[a];
                ns[si] = Math.Max(inp.Mins[si], Math.Min(inp.Maxs[si],
                    Math.Round((ns[si] + actionDelta[a]) * 10) / 10.0));
                ProjectToBudget(ns, inp);
                return ns;
            }

            var rand = new Random();
            double[] current = (double[])inp.Initial.Clone();
            ProjectToBudget(current, inp);

            double[][] topSolutions = new double[top][];
            double[] topScores = new double[top];
            for (int i = 0; i < top; i++)
            {
                topSolutions[i] = (double[])current.Clone();
                topScores[i] = double.MinValue;
            }

            int bestDD = int.MinValue;
            double[] bestSolution = (double[])current.Clone();

            for (int iter = 0; iter < maxIterations; iter++)
            {
                inp.CancellationToken.ThrowIfCancellationRequested();

                int topIdx = iter % 5 == 0 && iter > 0
                    ? rand.Next(Math.Min(5, top))
                    : rand.Next(top);
                double[] simState = (double[])topSolutions[topIdx].Clone();

                int steps = rand.Next(minSteps, maxSteps);
                for (int step = 0; step < steps; step++)
                    simState = ApplyAction(simState, rand.Next(actionsCount));

                double reward = Fitness(simState, inp);
                if ((int)reward > bestDD)
                {
                    bestDD = (int)reward;
                    bestSolution = (double[])simState.Clone();
                    inp.ReportStatus?.Invoke($"MCTS: iter={iter}, DD={bestDD}");
                }

                for (int i = 0; i < top; i++)
                {
                    if (reward > topScores[i])
                    {
                        for (int j = top - 1; j > i; j--)
                        {
                            topSolutions[j] = topSolutions[j - 1];
                            topScores[j]    = topScores[j - 1];
                        }
                        topSolutions[i] = (double[])simState.Clone();
                        topScores[i]    = reward;
                        break;
                    }
                }

                if (iter % 100 == 0 && iter > 0)
                {
                    for (int t = 0; t < Math.Min(3, top); t++)
                    {
                        double[] state = (double[])topSolutions[t].Clone();
                        for (int attempt = 0; attempt < 10; attempt++)
                        {
                            double[] ns = ApplyAction(state, rand.Next(actionsCount));
                            double nr = Fitness(ns, inp);
                            if (nr > topScores[t])
                            {
                                state = ns;
                                topScores[t] = nr;
                                if ((int)nr > bestDD)
                                {
                                    bestDD = (int)nr;
                                    bestSolution = (double[])ns.Clone();
                                }
                            }
                        }
                    }
                }
            }

            Finalize(bestSolution, inp);
            return bestSolution;
        }

        // =====================================================================
        // АЛГОРИТМ 3: Дифференциальная эволюция (DE)
        // =====================================================================
        private Task<double[]> RunDEAsync(RecommendationInput inp)
            => Task.Run(() => RunDE(inp), inp.CancellationToken);

        private double[] RunDE(RecommendationInput inp,
            int populationSize = 0, int maxGenerations = 0,
            double F = 0, double CR = 0)
        {
            populationSize = Math.Max(4, populationSize > 0 ? populationSize : inp.DePopulationSize);
            maxGenerations = Math.Max(1, maxGenerations > 0 ? maxGenerations : inp.DeMaxGenerations);
            F = F > 0 ? F : inp.DeMutationFactor;
            CR = CR > 0 ? CR : inp.DeCrossoverRate;
            CR = Math.Max(0, Math.Min(1, CR));

            int n = inp.Initial.Length;
            var rand = new Random();

            double[][] pop = new double[populationSize][];
            double[] fit  = new double[populationSize];

            for (int i = 0; i < populationSize; i++)
            {
                inp.CancellationToken.ThrowIfCancellationRequested();
                pop[i] = new double[n];
                if (i == 0)
                    Array.Copy(inp.Initial, pop[i], n);
                else
                    for (int j = 0; j < n; j++)
                        pop[i][j] = inp.Mins[j] + rand.NextDouble() * (inp.Maxs[j] - inp.Mins[j]);
                ProjectToBudget(pop[i], inp);
                fit[i] = Fitness(pop[i], inp);
            }

            int bestIdx = 0;
            for (int i = 1; i < populationSize; i++)
                if (fit[i] > fit[bestIdx]) bestIdx = i;

            for (int gen = 0; gen < maxGenerations; gen++)
            {
                inp.CancellationToken.ThrowIfCancellationRequested();
                for (int i = 0; i < populationSize; i++)
                {
                    int a, b, c;
                    do { a = rand.Next(populationSize); } while (a == i);
                    do { b = rand.Next(populationSize); } while (b == i || b == a);
                    do { c = rand.Next(populationSize); } while (c == i || c == a || c == b);

                    double[] mutant = new double[n];
                    for (int j = 0; j < n; j++)
                        mutant[j] = Math.Max(inp.Mins[j], Math.Min(inp.Maxs[j],
                            pop[a][j] + F * (pop[b][j] - pop[c][j])));
                    ProjectToBudget(mutant, inp);

                    int jRand = rand.Next(n);
                    double[] trial = new double[n];
                    for (int j = 0; j < n; j++)
                        trial[j] = rand.NextDouble() < CR || j == jRand ? mutant[j] : pop[i][j];
                    ProjectToBudget(trial, inp);

                    double trialFit = Fitness(trial, inp);
                    if (trialFit > fit[i])
                    {
                        pop[i] = trial;
                        fit[i] = trialFit;
                        if (trialFit > fit[bestIdx])
                        {
                            bestIdx = i;
                            inp.ReportStatus?.Invoke($"DE: gen={gen}, DD={(int)trialFit}");
                        }
                    }
                }
            }

            double[] best = (double[])pop[bestIdx].Clone();
            Finalize(best, inp);
            return best;
        }

        // =====================================================================
        // Общие утилиты
        // =====================================================================
        private static double Fitness(double[] x, RecommendationInput inp)
        {
            inp.EvalCallCount++;
            double diff = Math.Abs(ComputeBudget(x, inp) - inp.TargetBudget);
            double penalty = diff > inp.BudgetTolerance ? diff * 100_000 : 0;
            return inp.Evaluate(x) - penalty;
        }

        private static double ComputeBudget(double[] x, RecommendationInput inp)
        {
            double sum = 0;
            for (int i = 0; i < x.Length; i++)
                sum += inp.Weights[i] * (x[i] - inp.BaseStats[i]);
            return sum;
        }

        private static void ProjectToBudget(double[] x, RecommendationInput inp)
        {
            int n = x.Length;
            double lo = -100, hi = 100;
            for (int iter = 0; iter < 30; iter++)
            {
                double lam = (lo + hi) / 2.0;
                double[] xp = new double[n];
                for (int i = 0; i < n; i++)
                {
                    double vp = Math.Max(inp.Mins[i] - inp.BaseStats[i],
                        Math.Min(inp.Maxs[i] - inp.BaseStats[i],
                            x[i] - inp.BaseStats[i] - lam * inp.Weights[i]));
                    xp[i] = inp.BaseStats[i] + vp;
                }
                double b = ComputeBudget(xp, inp);
                if (Math.Abs(b - inp.TargetBudget) < 0.01) { Array.Copy(xp, x, n); return; }
                if (b > inp.TargetBudget) lo = lam; else hi = lam;
            }
            double lam2 = (lo + hi) / 2.0;
            for (int i = 0; i < n; i++)
            {
                double vp = Math.Max(inp.Mins[i] - inp.BaseStats[i],
                    Math.Min(inp.Maxs[i] - inp.BaseStats[i],
                        x[i] - inp.BaseStats[i] - lam2 * inp.Weights[i]));
                x[i] = inp.BaseStats[i] + vp;
            }
        }

        private static void Finalize(double[] x, RecommendationInput inp)
        {
            int n = x.Length;
            for (int i = 0; i < n; i++) x[i] = Math.Round(x[i] * 10) / 10.0;
            ProjectToBudget(x, inp);
            for (int i = 0; i < n; i++)
                x[i] = Math.Max(inp.Mins[i], Math.Min(inp.Maxs[i],
                    Math.Round(x[i] * 10) / 10.0));
        }
    }
}
