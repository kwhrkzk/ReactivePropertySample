using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObjects
{
    public class ExecutionTime : ValueObject
    {
        public static ExecutionTime Create(int _second)
        {
            if (_second <= 0)
                throw new ArgumentOutOfRangeException("second", _second, "実行時間は0秒以上の指定が必要です。");

            return new ExecutionTime(_second);
        }

        public int Second { get; }
        public int Millisecond => Second * 1000;

        private ExecutionTime(int _second) => Second = _second;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Second;
        }
    }
}
