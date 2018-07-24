using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ViewModule
{
    /// <summary>
    /// http://neue.cc/2013/03/06_399.html
    /// </summary>
    public static class RandomProvider
    {
        private static ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>(() =>
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var buffer = new byte[sizeof(int)];
                rng.GetBytes(buffer);
                var seed = BitConverter.ToInt32(buffer, 0);
                return new Random(seed);
            }
        });

        public static Random GetThreadRandom()
        {
            return randomWrapper.Value;
        }
    }
}
