using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSA_WPF
{
    class RSA
    {
        public int e { private set; get; }
        public int d { private set; get; }
        public int n { private set; get; }
        public int p { private set; get; }
        public int q { private set; get; }
        public int m { private set; get; }
        public string messageUser;
        private int mode;

        public RSA()
        {
            this.e = 0;
            this.d = 0;
            this.n = 0;
            this.p = 0;
            this.q = 0;
            this.mode = 0;
        }
        
        public void generateParams()
        {
            bool existSamePrimeNum;

            int[] primeNumbers;
            do
            {
                existSamePrimeNum = false;
                primeNumbers = searchPrimeNumber(2, 1, 1000);
                for (int i = 0; i < primeNumbers.Length; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (primeNumbers[i] == primeNumbers[j])
                            existSamePrimeNum = true;
                    }
                }

            } while (primeNumbers == null || existSamePrimeNum);

            n = 1;
            int counter = 0;
            foreach (int val in primeNumbers)
            {
                n *= val;
                if (counter == 0)
                    p = val;
                else
                    q = val;
                counter++;
            }
            int[] tempArray = null;
            e = 0;
            while (tempArray == null)
            {
                do
                {
                    tempArray = searchPrimeNumber(1, 1, primeNumbers[0]);

                } while (tempArray == null);

                e = tempArray[0];
            }

            m = (primeNumbers[0] - 1) * (primeNumbers[1] - 1);
            d = searchD(e, m);
        }

        public List<int> encryption(string message, int e, int n)
        {
            List<int> encodingMessage = new List<int>();

            foreach (var c in message)
            {
                encodingMessage.Add(power_modulo_fast(c, e, n));
            }

            return encodingMessage;
        }

        public string decryption(List<int> message, int d, int n)
        {
            string messageString = "";

            foreach(var msg in message)
            {
                messageString += (char)power_modulo_fast(msg, d, n);
            }

            return messageString;
        }

        public List<int> createSignature(string message, int d, int n)
        {
            List<int> encodingMessage = new List<int>();

            foreach (var c in message)
            {
                encodingMessage.Add(power_modulo_fast(c, d, n));
            }

            return encodingMessage;
        }

        public bool verificationSignature(List<int> signature, string message, int e, int n)
        {
            int i = 0;

            /*if (signature.Count != message.Length)
                return false;*/

            foreach (var c in signature)
            {
                if (c == 0)
                    break;
                if ( message[i] != (char)power_modulo_fast(c, e, n))
                {
                    return false;
                }
                i++;
            }

            return true;
        }

        public int searchD(int e, int m)
        {
            int i = 2;
            while(true)
            { 
                if (((i * e) % m) == 1)
                {
                    return i;
                }
                i++;
            }  
        }

        public string getPublicKey()
        {
            return "public;" + e + ";" + n;
        }

        public string getPrivateKey()
        {
            return "private;" + d + ";" + n;
        }

        public string getPrimeNumbers()
        {
            return p + ";" + q;
        }

        public int[] searchPrimeNumber(int amountPrimeNumbers, int subrangeFirst = 1, int subrangeSecond = 100000)
        {
            int lengthByteArray = 2048;
            byte[] randomRange;

            int rangeFirst = 0, rangeSecond = 0;
            int randomCounter = 0;
             
            List<int> primeNumbers = new List<int>();
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            int[] returnPrimeNumbers = new int[amountPrimeNumbers];

            do
            {
                randomRange = new byte[lengthByteArray];
                rng.GetBytes(randomRange);
                rangeFirst = BitConverter.ToInt32(randomRange, 0);
                rangeFirst = (int)rangeFirst % subrangeSecond + subrangeFirst;
            } while (rangeFirst < 0 || rangeFirst < 100 || rangeFirst > subrangeSecond - 10);

            do
            {
                randomRange = new byte[lengthByteArray];
                rng.GetBytes(randomRange);
                rangeSecond = BitConverter.ToInt32(randomRange, 0);
                rangeSecond = (int)rangeSecond % subrangeSecond + subrangeFirst;
            } while (rangeSecond < rangeFirst);

            for(int i=rangeFirst; i <= rangeSecond; i++)
            {
                int counter = 0;
                for(int j=1; j <= i; j++ )
                {
                    if (i % j == 0)
                        counter++;
                }
                if (counter == 2)
                {
                    primeNumbers.Add(i);
                    randomCounter++;
                }
            }

            if (primeNumbers.Count < 2)
                return null;

            int counterIndex = 0;
            List<int> saveIndexPrimeNumbers = new List<int>();
            for(int i=0; i < amountPrimeNumbers; i++)
            {
                
                randomRange = new byte[lengthByteArray];
                rng.GetBytes(randomRange);
                int randIndex = BitConverter.ToInt32(randomRange, 0);
                while (randIndex < 0)
                {
                    rng.GetBytes(randomRange);
                    randIndex = BitConverter.ToInt32(randomRange, 0);
                }
                randIndex = (int)randIndex % randomCounter + 1;
                if(randIndex < 0 || randIndex > returnPrimeNumbers.Length)
                {
                    randIndex = counterIndex++;
                }
                if(randIndex < 0 || randIndex > returnPrimeNumbers.Length-1)
                    returnPrimeNumbers[i] = primeNumbers[returnPrimeNumbers.Length-1];
                else
                     returnPrimeNumbers[i] = primeNumbers[randIndex];

            }
                        
            return returnPrimeNumbers;
        }

        public static int power_modulo_fast(int a, int b, int m)
        {
            int i;
            long result = 1;
            long x = a % m;

            for (i = 1; i <= b; i <<= 1)
            {
                x %= m;
                if ((b & i) != 0)
                {
                    result *= x;
                    result %= m;
                }
                x *= x;
            }

            return (int)result;
        }
    }
}
