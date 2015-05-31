using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContestsPortal.WebSite.Infrastructure.DataAttributes
{
     public class PhoneNumberAttribute : RegularExpressionAttribute
    {
        #region Fields

        private static readonly string _pattern;

        #endregion

        #region Constructors

        static PhoneNumberAttribute()
        {
            /*
         Will be passed: (according to: http://habrahabr.ru/post/110731/)
        +79261234567
        89261234567
        79261234567
        +7 926 123 45 67
        8(926)123-45-67
        123-45-67
        9261234567
        79261234567
       (495)1234567
       (495) 123 45 67
        89261234567
        8-926-123-45-67
        8 927 1234 234
        8 927 12 12 888
        8 927 12 555 12
        8 927 123 8 123      
      */
//              _pattern = @"^(8\d\d\d\d\d\d\d\d\d\d)";
              _pattern = @"^((8|\+7)[\- ]?)?(\(?\d{3}\)?[\- ]?)?[\d\- ]{7,10}$";
        }

        public PhoneNumberAttribute() : base(_pattern)
        { }

        #endregion
        
    }
}
