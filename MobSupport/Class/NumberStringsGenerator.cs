using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobSupport.Class
{
    internal static class NumberStringsGenerator
    {
        public static string[] GenerateShuffledZeroPaddedNumbers(int count)
        {
            int digits = count.ToString().Length;

            // 開始値が01なので、1からcountまでの数字を生成
            string[] result = Enumerable.Range(1, count)
                .Select(i => i.ToString().PadLeft(digits, '0'))
                .ToArray();

            // 配列をランダムにシャッフル
            Random rng = new Random();
            result = result.OrderBy(x => rng.Next()).ToArray();

            return result;
        }
    }
}
