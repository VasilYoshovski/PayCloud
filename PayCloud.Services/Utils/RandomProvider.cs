using System;
using System.Collections.Generic;
using System.Text;

namespace PayCloud.Services.Utils
{
    public class RandomProvider : IRandomProvider
    {
        private readonly Random randomField;

        public RandomProvider()
        {
            this.randomField = new Random();
        }

        public int Next => this.randomField.Next();
    }
}
