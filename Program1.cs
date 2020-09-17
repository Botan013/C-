//This is Lab1 for KM by Botan013
using System;
using System.IO;
using System.Text;

namespace LabaKMShifrovanie
{
    public class CaesarCipher
    {
        private string CodeEncode(string fullAlfabet, string text, int k, int key)
        {
            var letterQty = fullAlfabet.Length;
            var retVal = "";
            if (key == 2)
                k = -k;
            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];
                var index = fullAlfabet.IndexOf(c);
                var codeIndex = (letterQty + index + k) % letterQty;
                retVal += fullAlfabet[codeIndex];
            }

            return retVal;
        }

        //шифрование текста
        public string Encrypt(string alph, string plainMessage, int k, int key)
            => CodeEncode(alph, plainMessage, k, key);
    }

    class Program
    {
        public static bool IsCoprime(int num1, int num2)
        {
            if (num1 == num2)
            {
                return num1 == 1;
            }
            else
            {
                if (num1 > num2)
                {
                    return IsCoprime(num1 - num2, num2);
                }
                else
                {
                    return IsCoprime(num2 - num1, num1);
                }
            }
        }

        public static string HillChieck(string fullalph, int aa, int ab, int ba, int bb, string text, int key)
        {
            var letterQty = fullalph.Length;
            var retVal = "";
            if (key == 2)
            {
                int opredelitel = aa * bb - ab * ba;
                int obratnOpred = MultiplicativeInverse(opredelitel, fullalph.Length);
                int boof = aa;
                aa = bb * obratnOpred % fullalph.Length;
                if (aa < 0)
                    aa += fullalph.Length;
                bb = boof * obratnOpred % fullalph.Length;
                if (bb < 0)
                    bb += fullalph.Length;
                ab = -ab * obratnOpred % fullalph.Length;
                if (ab < 0)
                    ab += fullalph.Length;
                ba = -ba * obratnOpred % fullalph.Length;
                if (ba < 0)
                    ba += fullalph.Length;
            }

            for (int i = 0; i < text.Length; i += 2)
            {
                var c = text[i];
                var index = fullalph.IndexOf(c);
                var cc = text[i + 1];
                var index2 = fullalph.IndexOf(cc);
                int let1 = (index * aa + index2 * ba) % fullalph.Length;
                int let2 = (index * ab + index2 * bb) % fullalph.Length;
                retVal += fullalph[let1];
                retVal += fullalph[let2];
            }

            return retVal;

        }

        static string Vigenere(string s, string key, string alphabet, int k)
        {
            int j = 0;
            StringBuilder ret = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                if (k == 1)
                    ret.Append(alphabet[(alphabet.IndexOf(s[i]) + alphabet.IndexOf(key[j])) % alphabet.Length]);
                else
                    ret.Append(alphabet[(alphabet.IndexOf(s[i]) - alphabet.IndexOf(key[j]) + alphabet.Length) % alphabet.Length]);

                j = (j + 1) % key.Length;
            }
            return ret.ToString();
        }

        public static string TrasporandCodding(string fullAlfabet, string textKey, string text, int key)
        {
            var letterQty = fullAlfabet.Length;
            var retVal = "";
            if (key == 2)
            {
                string boof = "";
                boof = fullAlfabet;
                fullAlfabet = textKey;
                textKey = boof;
            }
            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];
                var index = fullAlfabet.IndexOf(c);
                // var codeIndex = (letterQty + index + k) % letterQty;
                retVal += textKey[index];
            }

            return retVal;
        }

        public static string AffineDecrypt(string alphab, string cipherText, int a, int b, int key)
        {
            string plainText = "";

            if (key == 1)
            {
                for (int i = 0; i < cipherText.Length; ++i)
                {
                    var c = cipherText[i];
                    var index = alphab.IndexOf(c);
                    // var codeIndex = (alphab.Length + index + k) % alphab.Length;
                    var codeIndex = (a * index + b) % alphab.Length;
                    plainText += alphab[codeIndex];
                }
            }
            else
            {
                int aInverse = MultiplicativeInverse(a, alphab.Length);
                for (int i = 0; i < cipherText.Length; ++i)
                {
                    var c = cipherText[i];
                    var index = alphab.IndexOf(c);
                    // var codeIndex = (alphab.Length + index + k) % alphab.Length;
                    var codeIndex = aInverse * (index + alphab.Length - b) % alphab.Length;
                    plainText += alphab[codeIndex];
                }

            }
            return plainText;
        }

        public static string Transport(string fullAlfabet, string textKey, string text, int key)
        {

            string textBoof;
            int[][] nums = new int[text.Length / textKey.Length + 2][];

            for (int i = 0; i < text.Length / textKey.Length; ++i)
            {
                nums[i + 2] = new int[textKey.Length];
                textBoof = text.Substring(i * textKey.Length, textKey.Length);

                for (int j = 0; j < textKey.Length; ++j)
                {
                    var c = textBoof[j];
                    var index = fullAlfabet.IndexOf(c);
                    nums[i + 2][j] = index;
                }
            }

            nums[0] = new int[textKey.Length];
            for (int j = 0; j < textKey.Length; ++j)
                nums[0][j] = j;

            nums[1] = new int[textKey.Length];
            for (int i = 0; i < textKey.Length; i++)
            {
                var c = textKey[i];
                var index = fullAlfabet.IndexOf(c);
                nums[1][i] = index;
            }

            var retVal = "";

            int[] tempp = new int[textKey.Length];

            //if (key == 2)
            //{
            //    tempp = nums[1];
            //    nums[1] = nums[0];
            //    nums[0] = nums[1];
            //}
            if (key == 1)
            {
                int temp;
                for (int i = 0; i < nums[1].Length - 1; i++)
                {
                    for (int j = i + 1; j < nums[1].Length; j++)
                    {
                        if (nums[1][i] > nums[1][j])
                        {
                            for (int k = 0; k < text.Length / textKey.Length + 2; ++k)

                            {
                                temp = nums[k][i];
                                nums[k][i] = nums[k][j];
                                nums[k][j] = temp;
                            }
                        }
                    }
                }
            }

            if (key == 2)
            {
                for (int i = 0; i < nums[1].Length; i++)
                    tempp[i] = nums[1][i];


                for (int i = 0; i < nums[1].Length - 1; i++)
                {

                    for (int j = i + 1; j < nums[1].Length; j++)
                    {
                        if (nums[1][i] > nums[1][j])
                        {
                            int tt;
                            tt = nums[1][i];
                            nums[1][i] = nums[1][j];
                            nums[1][j] = tt;
                        }
                    }
                }

                for (int i = 0; i < textKey.Length; ++i)
                {
                    for (int j = 0; j < textKey.Length; ++j)
                        if (nums[1][i] == tempp[j])
                            tempp[j] = i;
                }

                int temp;
                for (int i = 0; i < nums[1].Length; i++)
                {
                    for (int j = 0; j < nums[1].Length; j++)
                    {
                        if (nums[0][i] == tempp[j])
                        {
                            for (int k = 0; k < text.Length / textKey.Length + 2; ++k)

                            {
                                temp = nums[k][i];
                                nums[k][i] = nums[k][j];
                                nums[k][j] = temp;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < text.Length / textKey.Length; ++i)
                for (int j = 0; j < textKey.Length; ++j)
                    retVal += fullAlfabet[nums[i + 2][j]];

            return retVal;
        }

        public static int MultiplicativeInverse(int a, int b)
        {
            for (int x = 1; x < b; ++x)
            {
                if ((a * x) % b == 1)
                    return x;
            }

            throw new Exception("No multiplicative inverse found!");
        }

        public int Position(string alph, string letter)
        {
            int position = -1;
            for (int i = 0; i < alph.Length; ++i)
            {
                if (alph[i] == letter[0])
                {
                    position = i;
                }
            }
            return position;
        }

        public bool CheckUnique(string str)
        {
            string one = "";
            string two = "";
            for (int i = 0; i < str.Length; i++)
            {
                one = str.Substring(i, 1);
                for (int j = 0; j < str.Length; j++)
                {
                    two = str.Substring(j, 1);
                    if ((one == two) && (i != j))
                        return false;
                }
            }
            return true;
        }

        public bool CheckText(string str, string str1)
        {
            string one = "";
            string two = "";
            int flag = 0;
            for (int i = 0; i < str.Length; i++)
            {
                one = str.Substring(i, 1);
                for (int j = 0; j < str1.Length; j++)
                {
                    two = str1.Substring(j, 1);
                    if (one == two)
                        flag++;
                }
                if (flag == 0)
                    return false;
                flag = 0;
            }
            return true;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the path to .txt, if alphabet does not exist in this path, then the standard alphabet will be selected." +
                "The last symbol should be '\\' ");

            string path = Console.ReadLine();
            string alphabet = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯ ";

            if (File.Exists(@path + "alphabet.txt"))
            {
                var fileStream = new FileStream(@path + "alphabet.txt", FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.Unicode))
                {
                    alphabet = streamReader.ReadLine();
                }
            }
            //Console.WriteLine(alphabet);//printing alphabet

            int lengthAlphabet = alphabet.Length;

            Program d = new Program();
            if (d.CheckUnique(alphabet) == false)
            {
                Console.WriteLine("There are repeating elements in the alphabet!");
                Environment.Exit(1);
            }


            string textIn = "";

            if (File.Exists(@path + "in.txt"))
            {
                var fileStream = new FileStream(@path + "in.txt", FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.Unicode))
                {
                    textIn = streamReader.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("There are no in.txt");
                Environment.Exit(1);
            }

            if (d.CheckText(textIn, alphabet) == false)
            {
                Console.WriteLine("Incorrect input file!");
                Environment.Exit(1);
            }

            string textKey = "";

            if (File.Exists(@path + "key.txt"))
            {
                var fileStream = new FileStream(@path + "key.txt", FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.Unicode))
                {
                    textKey = streamReader.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("There are no key.txt");
                Environment.Exit(1);
            }

            if (d.CheckText(textKey, alphabet) == false)
            {
                Console.WriteLine("Incorrect input file!");
                Environment.Exit(1);
            }

            if (String.IsNullOrEmpty(alphabet) || String.IsNullOrEmpty(textIn) || String.IsNullOrEmpty(textKey))
            {
                Console.WriteLine("There are empty .txt file");
                Environment.Exit(1);
            }

            /* --------------Начало--------------------*/

            Console.WriteLine("For codding press 1\n" +
                "For decodding press 2");

            int key = Convert.ToInt32(Console.ReadLine());
            if (key != 1 && key != 2)
            {
                Console.WriteLine("Incorrect data");
                Environment.Exit(1);
            }
            //Console.Clear();

            int keyLength = textKey.Length;

            Console.WriteLine("press:\n" +
                "1)Cesar\n" +
                "2)Aphin\n" +
                "3)Replace\n" +
                "4)Hill\n" +
                "5)Transposition\n" +
                "6)Vijner");

            string shifrKey = Console.ReadLine();
            Console.Clear();
            string otvet = "";
            switch (shifrKey)
            {
                case "1":
                    if (keyLength != 1)
                    {
                        Console.WriteLine("Incorrect data");
                        Environment.Exit(1);
                    }
                    int secretKey = d.Position(alphabet, textKey);
                    var cipher = new CaesarCipher();
                    var encryptedText = cipher.Encrypt(alphabet, textIn, secretKey, key);
                    otvet = encryptedText;
                    Console.WriteLine("message: " + encryptedText + "\nThe end process");
                    break;

                case "2":
                    if (keyLength != 2)
                    {
                        Console.WriteLine("Incorrect data");
                        Environment.Exit(1);
                    }
                    int aphin1 = d.Position(alphabet, textKey.Substring(0, 1));
                    int aphin2 = d.Position(alphabet, textKey.Substring(1, 1));
                    otvet = AffineDecrypt(alphabet, textIn, aphin1, aphin2, key);
                    Console.WriteLine("message: " + AffineDecrypt(alphabet, textIn, aphin1, aphin2, key) + "\nThe end process");
                    break;
                case "3":
                    if (keyLength != alphabet.Length || !d.CheckUnique(textKey))
                    {
                        Console.WriteLine("Incorrect data");
                        Environment.Exit(1);
                    }
                    otvet = TrasporandCodding(alphabet, textKey, textIn, key);
                    Console.WriteLine("message: " + TrasporandCodding(alphabet, textKey, textIn, key) + "\nThe end process");
                    break;
                case "4":
                    int aa, ab, ba, bb;
                    aa = d.Position(alphabet, textKey.Substring(0, 1));
                    ab = d.Position(alphabet, textKey.Substring(1, 1));
                    ba = d.Position(alphabet, textKey.Substring(2, 1));
                    bb = d.Position(alphabet, textKey.Substring(3, 1));
                    if (keyLength != 4 || (aa * bb - ab * ba) % alphabet.Length == 0 || !IsCoprime(aa * bb - ab * ba, alphabet.Length))
                    {
                        Console.WriteLine("Incorrect data");
                        Environment.Exit(1);
                    }
                    if (textIn.Length % 2 != 0)
                        textIn += alphabet[0];
                    otvet = HillChieck(alphabet, aa, ab, ba, bb, textIn, key);
                    Console.WriteLine("message: " + HillChieck(alphabet, aa, ab, ba, bb, textIn, key) + "\nThe end process");
                    break;
                case "5":

                    if (keyLength > alphabet.Length || !d.CheckUnique(textKey))
                    {
                        Console.WriteLine("Incorrect data");
                        Environment.Exit(1);
                    }
                    int ostatok = textIn.Length % textKey.Length;
                    if (ostatok != 0)
                    {
                        for (int i = 0; i < textKey.Length - ostatok; ++i)
                        {
                            textIn += alphabet[0];
                        }
                    }
                    otvet = Transport(alphabet, textKey, textIn, key);
                    Console.WriteLine("message: " + Transport(alphabet, textKey, textIn, key) + "\nThe end process");
                    break;

                case "6":
                    if (textKey.Length > alphabet.Length || !d.CheckUnique(textKey))
                    {
                        Console.WriteLine("Incorrect data");
                        Environment.Exit(1);
                    }
                    otvet = Vigenere(textIn, textKey, alphabet, key);
                    Console.WriteLine("message: " + Vigenere(textIn, textKey, alphabet, key) + "\nThe end process");
                    break;

                default:
                    Console.WriteLine("Incorrect symbol");
                    Environment.Exit(1);
                    break;
            }
            if (key == 1)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);

                using (FileStream fstream = new FileStream($"{path}\crypt.txt", FileMode.OpenOrCreate))
                {
                    // преобразуем строку в байты
                    byte[] array = System.Text.Encoding.Default.GetBytes(otvet);
                    // запись массива байтов в файл
                    fstream.Write(array, 0, array.Length);
                    Console.WriteLine("Текст записан в файл");
                }
            }
            if (key == 2)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);

                using (FileStream fstream = new FileStream($"{path}\decrypt.txt", FileMode.OpenOrCreate))
                {
                    // преобразуем строку в байты
                    byte[] array = System.Text.Encoding.Default.GetBytes(otvet);
                    // запись массива байтов в файл
                    fstream.Write(array, 0, array.Length);
                    Console.WriteLine("Текст записан в файл");
                }
            }
            Console.ReadKey();
        }
    }
}
