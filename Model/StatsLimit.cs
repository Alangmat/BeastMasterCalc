using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public static class StatsLimit
    {
        public const int MIN_VALUE = 0;
        public const int MAX_SKILL_COOLDOWN = 200;
        public const int MAX_ATTACK_SPEED = 70;
        public const int MAX_CRITICAL_HIT = 100;
        public const int MAX_CRITICAL_DAMAGE = 200;
        public const int MAX_PENETRATION = 100;
        public const int MAX_ACCURACY = 100;
        public const int MAX_ATTACK_STRENGTH = 100;
        public const int MAX_PIERCING_ATTACK = 50;
        public const int MAX_RAGE = 50;
        public const int MAX_FACILITATION = 50;
        public const int MAX_PROTECTION = 80;
        public const int MAX_DODGE = 60;
        public const int MAX_RESILIENCE = 60;

        public static double CheckLimit(double stat, int maxLimit)
        {
            stat = Math.Max(stat, MIN_VALUE);
            stat = Math.Min(stat, maxLimit);
            return stat;

        }
    }
}
