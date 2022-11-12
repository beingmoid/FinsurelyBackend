using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Text;

namespace NukesLab.Core.Repository
{
    public abstract class HRZValueGenerator: ValueGenerator
    {
        public override bool GeneratesTemporaryValues => false;
    }
}
