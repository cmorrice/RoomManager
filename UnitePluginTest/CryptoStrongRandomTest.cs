using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitePlugin.Utility;
using Xunit;

namespace UnitePluginTest
{
    public class CryptoStrongRandomTest
    {
        [Fact]
        public void Constructor()
        {
            //Given
            //When
            var sut = new CryptoStrongRandom();
            //Then
            Assert.NotNull(sut);
        }

        [Fact]
        public void NextIsValid()
        {
            //Given
            const byte maxNumber = 32;
            //When
            var sut = new CryptoStrongRandom();
            //Then
            for (var i = 0; i < maxNumber; i++)
            {
                Assert.InRange(sut.Next(maxNumber), 0, maxNumber);
            }
        }
    }
}
