using System;
using System.Linq;

namespace Assets.Scripts.Extensions.Numeric
{
    public static class DoubleExtensions
    {
        #region Functions

        /// <summary>
        /// Abbrivates this number with the 1000th indices. <br/>
        /// Example:<br/>
        /// 1,000 returns 1k <br/>
        /// 1,000,000 returns 1m <br/>
        /// 1,000,000,000 returns 1b <br/>
        /// and so on...<br/>
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string AbbrivateNumber(this double num)
        {
            if (num < 1000) return num.ToString("N0");
            int exp = (int)(Math.Log(num) / Math.Log(1000));
            string numStr = string.Format("{0:0.00}", num / Math.Pow(1000, exp));
            numStr = numStr.Substring(0, 4).Trim('.');

            string[] splitted = numStr.Split(new char[] { '.', ',' });
            if (splitted.Length > 1 && !String.IsNullOrEmpty(splitted[1]) && splitted[1].Equals("00"))
                numStr = splitted[0];

            return string.Format("{0}{1}", numStr, CurrencyAbbreviations.AbbreviationList.ElementAt(exp - 1));
        }

        /// <summary>
        /// Returns the string value of decimal portion of this number.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string ToIntString(this double num)
        {
            var result = num.ToString("#");
            return string.IsNullOrWhiteSpace(result) ? "0" : result;
        }

        /// <summary>
        /// Returns this number to the power of given pow value.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="pow"></param>
        /// <returns></returns>
        public static double Pow(this double val, int pow)
        {
            var result = 1d;
            for (int i = 0; i < pow; i++)
            {
                result *= val;
            }
            return result;
        }

        /// <summary>
        /// Clamps this value between given min and max values.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double Clamp(this double val, double min, double max)
        {
            return val < min ? min : (val > max ? max : val);
        }

        #endregion Functions
    }
}