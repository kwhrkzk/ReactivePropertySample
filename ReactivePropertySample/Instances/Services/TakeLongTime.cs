using Domain.Services;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Instances.Services
{
    public class TakeLongTime : ITakeLongTime
    {
        public void Execute(ExecutionTime _executionTime)
        {
            Thread.Sleep(_executionTime.Millisecond);
        }
    }
}
