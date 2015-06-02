using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContestsPortal.WebSite.Views.Shared.DisplayTemplates
{
    public static class StringExtensions
    {
        /// <summary>
        /// Обрезка текста правильно по тегам
        /// </summary>
        /// <param name="text">текст</param>
        /// <param name="maxLength">количесто букв показывать</param>
        /// <returns>обрезанный текст</returns>
        public static string CutLongText(string text, int maxLength, string appendText)
        {
            string FindChar1 = "<br";
            char FindChar2 = ' ';
            int EndIdx, CountRemove;
            CountRemove = text.Length;
            if (text.Length > maxLength)
            {
                EndIdx = text.LastIndexOf(FindChar1, maxLength);
                if (EndIdx < 1)
                    EndIdx = text.LastIndexOf(FindChar2, maxLength);
                text = text.Substring(0, EndIdx) + appendText;
            }
            return text;
        }
    }
}