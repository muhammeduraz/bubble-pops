using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Extensions.Numeric
{
    public static class IntegerExtensions
    {
        #region Functions

        /// <summary>
        /// Returns the ordered text of this index <br/>
        /// Example: <br/>
        /// 1 returns 1st <br/>
        /// 2 return 2nd <br/>
        /// 3 return 3rd <br/>
        /// 4 return 4th <br/>
        /// and so on...
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetOrderText(this int index)
        {
            string result = "";
            int a = index % 100;
            if (a > 10 && a < 20)
            {
                result = string.Format("{0}th", index);
            }
            else
            {
                a %= 10;
                switch (a)
                {
                    case 1:
                        result = string.Format("{0}st", index);
                        break;
                    case 2:
                        result = string.Format("{0}nd", index);
                        break;
                    case 3:
                        result = string.Format("{0}rd", index);
                        break;
                    default:
                        result = string.Format("{0}th", index);
                        break;
                }
            }
            return result;
        }

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
        public static string AbbrivateNum(this int num)
        {
            if (num < 1000) return "" + num;
            int exp = (int)(Mathf.Log(num) / Mathf.Log(1000));
            string numStr = string.Format("{0:0.00}", num / Math.Pow(1000, exp));

            string[] splitted = numStr.Split(new char[] { '.', ',' });
            if (splitted.Length > 1 && !String.IsNullOrEmpty(splitted[1]) && splitted[1].Contains("00"))
                numStr = splitted[0];

            return $"{numStr:0.00}{CurrencyAbbreviations.AbbreviationList.ElementAt(exp - 1)}";
        }

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
        public static string AbbrivateNum(this ulong num)
        {
            if (num < 1000) return "" + num;
            int exp = (int)(Math.Log(num) / Math.Log(1000));
            string numStr = string.Format("{0:0.00}", num / Math.Pow(1000, exp));
            numStr = numStr.Substring(0, 4).Trim('.');
            return string.Format("{0}{1}", numStr, CurrencyAbbreviations.AbbreviationList.ElementAt(exp - 1));
        }

        /// <summary>
        /// Concatanates this integer array with the given integer array.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int[] AddRange(this int[] a, int[] b)
        {
            int[] result = new int[a.Length + b.Length];
            Array.Copy(a, result, a.Length);
            Array.ConstrainedCopy(b, 0, result, a.Length, b.Length);
            return result;
        }

        /// <summary>
        /// True if this number is between the given min and max values.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="inclusive"></param>
        /// <returns></returns>
        public static bool Between(this int x, int min, int max, bool inclusive = false)
        {
            if (inclusive)
            {
                return x <= max && x >= min;
            }
            else
            {
                return x < max && x > min;
            }
        }

        /// <summary>
        /// Returns this number to the power of given pow value.
        /// </summary>
        /// <param name="val"></param>
        /// <param name="pow"></param>
        /// <returns></returns>
        public static int Pow(this int val, int pow)
        {
            var result = 1;
            for (int i = 0; i < pow; i++)
            {
                result *= val;
            }
            return result;
        }

        /// <summary>
        /// Converts this color code to a color.
        /// </summary>
        /// <param name="colorCode"></param>
        /// <returns></returns>
        public static Color ToColor(this uint colorCode)
        {
            byte r = System.Convert.ToByte((colorCode & 0xFF000000) >> 24);
            byte g = System.Convert.ToByte((colorCode & 0x00FF0000) >> 16);
            byte b = System.Convert.ToByte((colorCode & 0x0000FF00) >> 8);
            byte a = System.Convert.ToByte(colorCode & 0x000000FF);

            return new Color32(r, g, b, a);
        }

        /// <summary>
        /// Converts this color code to a color.
        /// </summary>
        /// <param name="colorCode"></param>
        /// <returns></returns>
        public static Color ToColor(this int colorCode)
        {
            return unchecked((uint)colorCode).ToColor();
        }

        #endregion Functions
    }
}