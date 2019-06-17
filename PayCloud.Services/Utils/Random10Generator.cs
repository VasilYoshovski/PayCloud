using PayCloud.Services.Providers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PayCloud.Services.Utils
{
    public class Random10Generator : IRandom10Generator
    {
        private readonly IDateTimeNowProvider dateTimeNow;

        public Random10Generator(IDateTimeNowProvider dateTimeNow)
        {
            this.dateTimeNow = dateTimeNow ?? throw new ArgumentNullException(nameof(dateTimeNow));
        }
        public string GenerateNumber()
        {
            Random random = new Random();
            var now = dateTimeNow.Now;
            var dateString = now.ToString("yy", CultureInfo.InvariantCulture) + now.ToString("MM", CultureInfo.InvariantCulture)
                + now.ToString("dd", CultureInfo.InvariantCulture);
            string fmt = "0000";
            return dateString+random.Next(0, 9999).ToString(fmt);
        }
    }
}
